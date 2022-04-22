using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCloseObjectDetector : MonoBehaviour
{
    GameObject guard;
    // Start is called before the first frame update
    void Start()
    {
        guard = transform.parent.transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            print("On shelf " + other.gameObject.GetComponent<ObjectGrounded>().isOnShelf());
            print("On floor " + other.gameObject.GetComponent<ObjectGrounded>().isGrounded());
            if (!other.gameObject.GetComponent<ObjectGrounded>().isOnShelf() && !other.gameObject.GetComponent<ObjectGrounded>().isGrounded())
            {
                guard.GetComponent<GuardBehaviour>().setSearch();
            }
        }
    }
}
