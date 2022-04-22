using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour
{
    public enum States { Rest, Patrol, Analyze, Persecute, Search, Arrest }
    public States state;

    public GameObject[] patrolPoints;
    private int actualPoint = 0;

    public NavMeshAgent myAgent;

    private GameObject suspicious;
    private GuardObjectDetection scriptObjectDetector;

    public GameObject arrestedPoint;
    public GameObject guardRoom;
    private int numVueltas;

    private float startRest;
    private float startLostSuspicious;
    private float startAnalyze;
    private float startPatrol;

    private float rotation;
    private float startRotation;

    private bool suspiciousLost;


    private GuardDetection[] rays;

    // Start is called before the first frame update
    void Start()
    {
        rays = transform.Find("Core").gameObject.transform.Find("Head").gameObject.GetComponentsInChildren<GuardDetection>();

        suspiciousLost = true; 
        numVueltas = 0;

        rotation = 0;

        if (myAgent == null) myAgent = GetComponent<NavMeshAgent>();
        state = States.Patrol;

        arrestedPoint = transform.Find("ArrestedPoint").gameObject;
        myAgent.SetDestination(patrolPoints[actualPoint].transform.position);
        GameObject objectDetector = transform.Find("ObjectDetector").gameObject;
        scriptObjectDetector = objectDetector.GetComponent<GuardObjectDetection>();

    }

    // Update is called once per frame
    void Update()
    {
        print("Estado guarda: " + state);
        if(state == States.Patrol) {
            patrol();
        }else if (state == States.Analyze)
        {
            analyze();
        }else if(state == States.Persecute)
        {
            persecute();
        }else if(state == States.Rest)
        {
            rest();
        }
        else if (state == States.Search)
        {
            search();
        }
        else if (state == States.Arrest)
        {
            arrest();
        }
    }

    private void arrest()
    {
        suspicious.transform.position = arrestedPoint.gameObject.transform.position;
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            setPatrol();
        }
    }

    private void setArrest()
    {
        setState(5);
        myAgent.SetDestination(guardRoom.transform.position);
        myAgent.stoppingDistance = 1.0f;
    }

    private void search()
    {
        rotation += Time.deltaTime * 80;
        transform.rotation = Quaternion.Euler(0, rotation, 0);

        //print("Rotacion actual = " + rotation);
        if (rotation > startRotation+360)
        {
            rotation = startRotation; 
            setPatrol();
        }
    }

    public void setSearch()
    {
        myAgent.SetDestination(transform.position);
        rotation = transform.rotation.y;
        startRotation = rotation;
        //print("Rotacion inicial = " + startRotation);
        setState(4);
    }

    private void rest()
    {
        myAgent.SetDestination(guardRoom.transform.position);
        float currentTime = Time.realtimeSinceStartup;
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            if (currentTime - startRest > 10)
            {
                setPatrol();
            }
        }
    }

    public void setRest()
    {
        numVueltas = 0;
        startRest = Time.realtimeSinceStartup;
        myAgent.stoppingDistance = 0;
        setState(0);
    }
    
    private void persecute()
    {
        myAgent.SetDestination(suspicious.transform.position);
        Vector3 supiciousXZ = new Vector3(suspicious.transform.position.x, transform.position.y, suspicious.transform.position.z);
        transform.LookAt(supiciousXZ); 

        if(Vector3.Distance(suspicious.transform.position, transform.position) < 1f)
        {
            setArrest();
        }

        if (!suspiciousLost && !inSight(suspicious))
        {
            suspiciousLost = true;
            startLostSuspicious = Time.realtimeSinceStartup;
        }

        if (suspiciousLost)
        {
            float currentTime = Time.realtimeSinceStartup;
            if(currentTime-startLostSuspicious > 3)
            {
                setPatrol();
            }
        }
    }

   

    public void setPersecute()
    {
        myAgent.SetDestination(suspicious.transform.position);
        myAgent.stoppingDistance = 0;
        setState(3);
    }

    private bool inSight(GameObject suspicious)
        {
            bool sight = false;
            foreach(GuardDetection ray in rays)
            {
                if(ray.hitObject == suspicious)
                {
                    sight = true;
                }
            }
            return sight;
        }

    

    public void setState(int newState)
    {
        if (newState == 0) state = States.Rest;
        if (newState == 1) state = States.Patrol;
        if (newState == 2) state = States.Analyze;
        if (newState == 3) state = States.Persecute;
        if (newState == 4) state = States.Search;
        if (newState == 5) state = States.Arrest;
    }
    public void setSuspicious(GameObject newSuspicious)
    {
        suspicious = newSuspicious;
        
    }
    public void setSuspiciousLost(bool b)
    {
        suspiciousLost = b;
    }
    private void analyze()
    {
        Vector3 supiciousXZ = new Vector3(suspicious.transform.position.x, transform.position.y, suspicious.transform.position.z);
        transform.LookAt(supiciousXZ);

        float currentTime = Time.realtimeSinceStartup;
        if (currentTime - startAnalyze > 5)
        {
            setPatrol();
        }
        else
        {
            if (myAgent.remainingDistance <= myAgent.stoppingDistance)
            {

                if (badBehaviour())
                {
                    setPersecute();
                }
            }
        }
       
    }
    public void setAnalyze(GameObject analized)
    {
        myAgent.SetDestination(analized.transform.position);
        myAgent.stoppingDistance = 3;
        startAnalyze = Time.realtimeSinceStartup;
        setSuspiciousLost(false);
        setSuspicious(analized);
        setState(2);
    }
    private bool badBehaviour()
    {
        bool bad = false;
        if (scriptObjectDetector.getNumObject() > 0 )
        {
            List<GameObject> objects = scriptObjectDetector.getObjects();
            if(scriptObjectDetector.getNumObject() > 3)
            {
                bad = true;
            }
            else
            {
                bool objetoLejos = objects.Exists(o => Vector3.Distance(suspicious.transform.position, o.transform.position) >= 2.5f);
                if (objetoLejos)
                {
                    bad = true;
                }
            }

        }
        
        return bad;
    }

    private void patrol()
    {
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        {
            actualPoint++;
            if (actualPoint >= patrolPoints.Length)
            {
                numVueltas++;
                actualPoint = 0;
                if (numVueltas == 1)
                {
                    setRest();
                }
            }
            myAgent.destination = patrolPoints[actualPoint].transform.position;
        }
    }

    public float getTimeStartPatrol()
    {
        return startPatrol;
    }

    public void setPatrol()
    {
        myAgent.stoppingDistance = 2;
        startPatrol = Time.realtimeSinceStartup;
        myAgent.SetDestination(patrolPoints[actualPoint].transform.position);
        setState(1);
    }
}
