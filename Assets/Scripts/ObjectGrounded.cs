using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrounded : MonoBehaviour
{
    private bool grounded;
    private bool onShelf;
    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
        onShelf = true;
    }
    public bool isGrounded()
    {
        return grounded;
    }

    public bool isOnShelf()
    {
        return onShelf;
    }
    public void setGrounded(bool grounded)
    {
        this.grounded = grounded;
    }

    public void setOnShelf(bool onShelf)
    {
        this.onShelf = onShelf;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Shelf"))
        {
            onShelf = false;
        }
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
