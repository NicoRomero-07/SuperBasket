using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrounded : MonoBehaviour
{
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
    }
    public bool isGrounded()
    {
        return grounded;
    }
    public void setGrounded(bool grounded)
    {
        this.grounded = grounded;
    }
    void OnCollisionEnter(Collision other)
    {
        // Hemos puesto un tag "Ground" sobre el suelo
        if (other.gameObject.CompareTag("Floor"))
        {
            grounded = true;
        }
    }
}
