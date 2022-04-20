using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardDetection : MonoBehaviour
{
    GameObject guard;
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
            if (hit.collider.gameObject.tag == "Player")
            {
                GuardBehaviour guardBehaviour = guard.GetComponent<GuardBehaviour>();
                if(!guardBehaviour.state.Equals(GuardBehaviour.States.Persecute))
                {
                    NavMeshAgent myAgent = guard.GetComponent<NavMeshAgent>();
                    myAgent.SetDestination(hit.collider.gameObject.transform.position);

                    guardBehaviour.setSuspicious(hit.collider.gameObject);
                    guardBehaviour.setState(2);
                }
                
                
            }
            
        }
    }
}
