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

    private GameObject handPoint;
    private GameObject cartPoint;
    private GameObject player;

    [Header("HUD")]
    public Image powerBar;

    [Header("Shot Options")]
    public float maxPressingTime = 2;
    public int power = 5;
    public int range = 2;

    [Header("Other")]
    
    public Camera cam;
    

    // Start is called before the first frame update
    void Start()
    {
        pressed = false;
        powerBar.fillAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        bool asignedCart=false;
        bool asignedHand = false;
        int numOfChilds = player.transform.childCount;
        GameObject current = null;
        int i = 0;
        while(i<numOfChilds && (!asignedCart || !asignedHand))
        {
            current = player.transform.GetChild(i).gameObject;
            if(current.name == "Hand")
            {
                handPoint = current.transform.GetChild(0).gameObject;
            }else if (current.name == "CartPoint")
            {
                cartPoint = current;
            }
            i++;
        }

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
                    if (Input.GetKey("e") && pickedObject == null)
                    {
                        take(hit.collider.gameObject);
                    }
                }else if (hit.collider.gameObject.tag == "Cart")
                {
                    if (Input.GetKey("e") && pickedObject == null)
                    {
                        takeCart(hit.collider.gameObject);
                    }
                }
            }
        }
        
    }

    private void takeCart(GameObject gameObject)
    {
        gameObject.transform.localEulerAngles = player.transform.localEulerAngles;
        gameObject.transform.rotation = cartPoint.gameObject.transform.rotation;
        gameObject.transform.position = cartPoint.gameObject.transform.position;
        gameObject.transform.SetParent(cartPoint.gameObject.transform);
       
        pickedObject = gameObject.gameObject;
    }

    private void take(GameObject gameObject)
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.position = handPoint.gameObject.transform.position;
        gameObject.transform.SetParent(handPoint.gameObject.transform);
        
        pickedObject = gameObject.gameObject;
        pickedObject.GetComponent<ObjectGrounded>().setGrounded(false);
        pickedObject.GetComponent<ObjectGrounded>().setOnShelf(false);
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
