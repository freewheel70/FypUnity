using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PlayerAbsorber : Absorber{

    private GameObject player;
    private Mass myMass;
    private MassViewController massView;
  
    
    private AudioSource[] audios;
    private PlayerID myPlayerId;

    void Start(){        
        player = this.gameObject;
        myMass = player.GetComponent<Mass>();
        massView = player.GetComponent<MassViewController>();
        audios = GetComponents<AudioSource>();
        myPlayerId = player.GetComponent<PlayerID>();
    }

    void OnTriggerEnter(Collider other){
        if(other.tag== "blackhole"){
            audios[0].Play();
        }else{
            handleEnter(other);
        }        
    }


    protected override void growUpByOne()
    {
        massView.StartAbsorb();
    }

    protected override bool shouldAbsorb(Mass victimMass)
    {
        return (victimMass==null)?false:myMass.currentMass > victimMass.currentMass;
    }

    protected override void playEffects(GameObject victim)
    {
        if (victim.tag == "Player")
        {
            Debug.Log("Call RpcPlayWarning !");
            RpcPlayWarning();
        }

#if UNITY_ANDROID
        if (isLocalPlayer)
        {
            Handheld.Vibrate();
        }
#endif
    }

    [ClientRpc]
    public void RpcPlayWarning(){               
        if (!isLocalPlayer) {            
            audios[0].Play();          
        }
    }


    protected override void stopGrowUpByOne(){
        massView.StopAbsorb();
    }

    protected override bool shouldStopAbsorb(Mass victimMass){
        return (victimMass == null) ? false: myMass.currentMass > victimMass.currentMass;
    }

    protected override string getID()
    {
        return "PlayerAbsorber";
    }
}
