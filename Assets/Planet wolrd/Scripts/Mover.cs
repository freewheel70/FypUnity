using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mover : NetworkBehaviour{

    Rigidbody rb;
    public float speed;

    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

}
