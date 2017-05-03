using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mover : NetworkBehaviour{
     
    public float speed;

    void Start(){
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

}
