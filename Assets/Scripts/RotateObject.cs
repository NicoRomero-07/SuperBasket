using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotateObject : MonoBehaviour
{
    private float speed = 60f;
    public int spin = 1;
    private GameObject guard;
    // Start is called before the first frame update
    void Start()
    {
        guard = transform.parent.gameObject;
        if(guard.tag != "Guard")
        {
            guard = guard.transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (guard.GetComponent<GuardBehaviour>().stopped)
        {

        }
        else
        {
            float delta = Time.deltaTime;
            float rads = 4 * Time.time;
            transform.Rotate(Vector3.right * Mathf.Sin(rads) * delta * speed * spin);
        }
        
    }
}
