using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarObjeto : MonoBehaviour
{
    private GameObject pickedObject;
    public GameObject handPoint;
    public Camera cam;
    public int range;
    // Start is called before the first frame update
    void Start()
    {
        range = 1;
        

    }

    // Update is called once per frame
    void Update()
    {
        
        if (pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                pickedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
                if (hit.collider.gameObject.tag == "Object")
                {
                    print("Objeto en frente " + hit.collider.gameObject.tag + " a distancia: " + hit.distance);
                    if (Input.GetKey("e") && pickedObject == null)
                    {
                        hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        hit.collider.gameObject.transform.position = handPoint.gameObject.transform.position;
                        hit.collider.gameObject.transform.SetParent(handPoint.gameObject.transform);
                        pickedObject = hit.collider.gameObject;
                    }
                }
            }
        }
        
    }
}
