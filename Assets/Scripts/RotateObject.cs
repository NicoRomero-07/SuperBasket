using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private float speed = 2f;
    private float maxRotation = 45f;
    public int spin = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left, maxRotation * Time.deltaTime * Mathf.Sin(Time.time * speed) * spin);
    }
}
