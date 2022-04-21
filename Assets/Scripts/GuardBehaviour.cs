using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour
{
    public enum States { Rest, Patrol, Analyze, Persecute }
    public States state;
    public GameObject[] patrolPoints;
    private int actualPoint = 0;
    public NavMeshAgent myAgent;
    private GameObject suspicious;
    private GuardObjectDetection scriptObjectDetector;
    // Start is called before the first frame update
    void Start()
    {
        if (myAgent == null) myAgent = GetComponent<NavMeshAgent>();
        state = States.Patrol;
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
        }
    }

    private void persecute()
    {
        myAgent.SetDestination(suspicious.transform.position);
        
    }

    public void setState(int newState)
    {
        if (newState == 0) state = States.Rest;
        if (newState == 1) state = States.Patrol;
        if (newState == 2) state = States.Analyze;
        if (newState == 3) state = States.Persecute;
    }
    public void setSuspicious(GameObject newSuspicious)
    {
        suspicious = newSuspicious;
    }

    private void analyze()
    {
        myAgent.SetDestination(suspicious.transform.position);
        
        if (myAgent.remainingDistance <= myAgent.stoppingDistance)
        { 

            if (badBehaviour())
            {
                myAgent.stoppingDistance = 0;
                setState(3);
            }
        }
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
                bool objetoLejos = objects.Exists(o => Vector3.Distance(suspicious.transform.position, o.transform.position) >= 1);
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
            if (actualPoint >= patrolPoints.Length) actualPoint = 0;
            myAgent.destination = patrolPoints[actualPoint].transform.position;
        }
    }
}
