﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MassViewController : NetworkBehaviour{

    [SyncVar(hook = "updateScale")]
    public float currentScale = 1.0f;

    public float absorbTimeGap = 0.5f;

    public GameObject explosion;

    private Mass myMass;
    private GameObject player;   

    private Queue absorbTickets = Queue.Synchronized(new Queue());
    private bool isAbsorbing = false;

    private Queue shrinkTickets = Queue.Synchronized(new Queue());
    private bool isShrinking = false;

    private bool isDead = false;
    private AudioSource[] audios;


    public void Reset(){
        currentScale = 1.0f;
        isAbsorbing = false;
        isShrinking = false;
        isDead = false;
        absorbTickets.Clear();
        shrinkTickets.Clear();

        myMass.Reset();
    }

    // Use this for initialization
    void Start () {
        player = this.gameObject;
        myMass = player.GetComponent<Mass>();
        audios = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartAbsorb(){
    
        absorbTickets.Enqueue(new object());
        if (!isAbsorbing && absorbTickets.Count <= 1){
            StartCoroutine(AbsorbMass());
        }                        
    }

    public bool IsAbsorbing(){
        return (absorbTickets.Count > 0);
    }

    IEnumerator AbsorbMass(){
        isAbsorbing = true;

        while (absorbTickets.Count>0){
            myMass.grow(5 * absorbTickets.Count);

            float newsacle = myMass.currentMass * 1.0f / myMass.initMass;
            
            currentScale = newsacle;

            PlayerAbsorber playerAbsorber = player.GetComponent<PlayerAbsorber>();
            if (playerAbsorber != null){
                playerAbsorber.checkEnemyList();
            }
            
            yield return new WaitForSeconds(absorbTimeGap);
        }

        isAbsorbing = false;
        Debug.Log("Stop absorb");
    }

    public void StopAbsorb(){
        if (absorbTickets.Count > 0) { 
            absorbTickets.Dequeue();
        }
    }

    public bool IsShrinking(){
        return (shrinkTickets.Count > 0);
    }

    public void StartShrink(){
  
        //if (this.gameObject.tag == "Player")
        //{
        //    RpcPlayWarning(netId);
        //}
        shrinkTickets.Enqueue(new object());

        if (!isShrinking && shrinkTickets.Count <= 1 && !isDead){
            StartCoroutine(ShrinkMass());
        }        
                        
    }



    [ClientRpc]
    public void RpcPlayExplosion(NetworkInstanceId id){
        if (id.Equals(netId)) { 
            audios[0].Play();
        }
    }

    IEnumerator ShrinkMass(){
        isShrinking = true;

        while (shrinkTickets.Count > 0 && !isDead){
            isDead = (myMass.shrink(5 * shrinkTickets.Count) == 0);
            if (isDead){                                
                PlayExplosionEffects();
                break;
            }

            float newsacle = myMass.currentMass * 1.0f / myMass.initMass;

            currentScale = newsacle;

            Vector3 oldSpeed = player.GetComponent<Rigidbody>().velocity;
            player.GetComponent<Rigidbody>().velocity = new Vector3(oldSpeed.x / 2, oldSpeed.y / 2, oldSpeed.z / 2);

            yield return new WaitForSeconds(absorbTimeGap);
        }

        isShrinking = false;
        Debug.Log("Stop shink");
    }

    public void StopShrink(){
        if (shrinkTickets.Count > 0) { 
            shrinkTickets.Dequeue();
        }
    }

    public void updateScale(float currentScale){
        player.transform.localScale = new Vector3(currentScale, currentScale, currentScale);        
    }

    private void PlayExplosionEffects() {
        GameObject explo = (GameObject)Instantiate(explosion, this.transform.position, Quaternion.identity);
        NetworkServer.Spawn(explo);
        RpcPlayExplosion(netId);       
    }

   
}
