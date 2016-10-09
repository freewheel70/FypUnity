using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mass : NetworkBehaviour {
  
    TextMesh massLabel;

    [SyncVar(hook = "updateMassLabel")]
    public int currentMass = 100;

    public int initMass;

    // Use this for initialization
    void Start()
    {       
        currentMass = initMass;
      
        massLabel = transform.Find("LabelHolder").Find("MassLabel").GetComponent<TextMesh>();

        updateMassLabel(currentMass);
        Debug.Log("This is " + name + " ; Tag : " + tag + " ; currentMass : "+ currentMass);
    }

    public void grow(int amount)
    {
        if (!isServer)
            return;
        currentMass += amount;
    }

    //return value
    //-1 ignored because not server
    // 0 current mass is 0, the gameobject is dead
    // 1 current mass is greater than 0, the gameobject is still alive
    public int shrink(int amount)
    {
        if (!isServer)
            return -1;
           
        currentMass -= amount;
        if (currentMass <= 0)
        {
            currentMass = 0;
            Debug.Log("Death!");           
            return 0;
        }
        return 1;
    }

    private void updateMassLabel(int currentMass)
    {
        if (massLabel == null)
        {
            Debug.Log("This is " + name + "massLabel == null");
        }
        else
        {
            massLabel.text = "Current Mass: " + currentMass;
        }
        
    }

}
