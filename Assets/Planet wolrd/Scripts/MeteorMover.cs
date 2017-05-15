using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeteorMover : Mover{

    public GameObject explosion;
    public int damage = -20;
    
    void OnTriggerEnter(Collider other)
    {
        if (!isServer)
        {
            return;
        }

        if (other.tag == "Boundary")
        {
            return;
        }

        if (other.tag == "Player")
        {
            Mass mass = other.transform.GetComponent<Mass>();
            mass.grow(damage);

            MassViewController viewController = other.transform.GetComponent<MassViewController>();
            viewController.setScale(mass.currentMass * 1.0f / mass.initMass);
        }

        //Debug.Log("Meteor Crash into " + other.tag + " !");
        
        Explode(this.gameObject, this.transform.position);
    }

    //[Command]
    public void Explode(GameObject obj, Vector3 pos)
    {       
        GameObject explo = (GameObject)Instantiate(explosion, pos, Quaternion.identity);

        NetworkServer.Spawn(explo);

        Destroy(obj);
    }
}
