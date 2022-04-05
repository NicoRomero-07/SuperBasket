using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    private GameObject pickedObject;
    private float startTime;
    private bool pressed;

    public Image powerBar;
    public float maxPressingTime;
    public GameObject handPoint;
    public int power;
    public Camera cam;
    public int range;

    // Start is called before the first frame update
    void Start()
    {
        pressed = false;
        powerBar.fillAmount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (pickedObject != null)
        {
           
            if (Input.GetMouseButtonDown(0))
            {
                startTime = Time.time;
                pressed = true;
            }
            if (pressed)
            {
                float currentPower = Time.time - startTime;
                powerBar.fillAmount = currentPower / maxPressingTime;
            }
            if (Input.GetMouseButtonUp(0))
            {
                shot();
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

    private void shot()
    {
        float longPress = Time.time - startTime;
        if (longPress > maxPressingTime)
        {
            longPress = maxPressingTime;
        }
        pressed = false;
        powerBar.fillAmount = 0;

        Vector3 direction = cam.transform.forward * longPress * power;
        direction.y += 2;
        Rigidbody rbPickedObject = pickedObject.GetComponent<Rigidbody>();
        pickedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        pickedObject.GetComponent<Rigidbody>().isKinematic = false;
        pickedObject.transform.SetParent(null);
        pickedObject = null;

        rbPickedObject.AddForce(direction, ForceMode.Impulse);
    }
}
