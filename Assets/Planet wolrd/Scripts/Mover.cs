using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mover : NetworkBehaviour
{

    Rigidbody rb;
    public float speed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

}
