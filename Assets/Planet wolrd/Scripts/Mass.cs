using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mass : NetworkBehaviour{

    TextMesh massLabel;

    [SyncVar(hook = "updateMassLabel")]
    public int currentMass = 100;

    public int initMass;

    public void Reset(){
        if (!isServer) { 
            return;
        }
        currentMass = 100;
    }

    // Use this for initialization
    void Start(){
        currentMass = initMass;

        massLabel = transform.Find("LabelHolder").Find("MassLabel").GetComponent<TextMesh>();

        updateMassLabel(currentMass);
    }

    public void grow(int amount){
        if (!isServer)
            return;
        currentMass += amount;
    }

    //return value
    //-1 ignored because not server
    // 0 current mass is 0, the gameobject is dead
    // 1 current mass is greater than 0, the gameobject is still alive
    public int shrink(int amount){
        if (!isServer)
            return -1;

        currentMass -= amount;
        if (currentMass <= 0){
            currentMass = 0;
            Debug.Log("Death!");
            return 0;
        }
        return 1;
    }

    private void updateMassLabel(int currentMass){
        if (massLabel == null){

        }else{
            massLabel.text = " " + currentMass;            
        }

    }

}