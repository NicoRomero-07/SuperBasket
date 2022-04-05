using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    public Image powerBar;
    private float startTime;
    private bool pressed;
    private ObjectManager scriptObject;
    public float currentPower;
    private float maxPower;

    // Start is called before the first frame update
    void Start()
    {
        pressed = false;
        GameObject hand = GameObject.Find("Hand");
        scriptObject = hand.GetComponent<ObjectManager>();
        maxPower = scriptObject.maxPressingTime;
        print(maxPower);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTime = Time.time;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
            powerBar.fillAmount = 0;
        }
        if (pressed)
        {
            currentPower = Time.time - startTime;
            powerBar.fillAmount = currentPower / maxPower;
        }
        
        
    }
}
