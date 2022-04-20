using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Object")
        {
            print("ObjetoEncestado");
        }
    }
}
