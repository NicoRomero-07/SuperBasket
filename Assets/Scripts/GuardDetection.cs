using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardDetection : MonoBehaviour
{
    GameObject guard;
    public GameObject hitObject;
    // Start is called before the first frame update
    void Start()
    {
        guard = transform.parent.transform.parent.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            hitObject = hit.collider.gameObject;

            if (hit.collider.gameObject.tag == "Player")
            {
                GuardBehaviour guardBehaviour = guard.GetComponent<GuardBehaviour>();
                if(guardBehaviour.state.Equals(GuardBehaviour.States.Patrol))
                {
                    
                    if (Vector3.Distance(hit.collider.transform.position, guard.transform.position)<=10 && (Time.realtimeSinceStartup - guardBehaviour.getTimeStartPatrol() > 5))
                    {
                        guardBehaviour.setAnalyze(hit.collider.gameObject);
                    }
                   
                }
                else if(guardBehaviour.state.Equals(GuardBehaviour.States.Persecute))
                {
                    guardBehaviour.setSuspiciousLost(false);
                }else if (guardBehaviour.state.Equals(GuardBehaviour.States.Search))
                {
                    guardBehaviour.setSuspicious(hit.collider.gameObject);
                    guardBehaviour.setPersecute();
                }
                
                
            }
            
        }
    }
}
