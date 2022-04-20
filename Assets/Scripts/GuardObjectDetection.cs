using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardObjectDetection : MonoBehaviour
{

    private List<GameObject> objects;
    // Start is called before the first frame update
    void Start()
    {
        objects = new List<GameObject>();
    }
    public List<GameObject> getObjects()
    {
        return objects;
    }
    public int getNumObject()
    {
        return objects.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {

            if (other.gameObject.GetComponent<ObjectGrounded>().isGrounded())
            {
                objects.Remove(other.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {

            if (other.gameObject.GetComponent<ObjectGrounded>().isGrounded())
            {
                objects.Add(other.gameObject);
            }
        }
    }
}
