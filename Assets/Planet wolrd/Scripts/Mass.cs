using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mass : NetworkBehaviour {
  
    TextMesh massLabel;

    [SyncVar(hook = "updateMassLabel")]
    public int currentMass = 100;

    // Use this for initialization
    void Start()
    {
        if (tag == "asteroid")
        {
            currentMass = 50;
        }else
        {
            currentMass = 100;
        }
        massLabel = transform.Find("LabelHolder").Find("MassLabel").GetComponent<TextMesh>();
   

        Debug.Log("This is " + name + " ; Tag : " + tag + " ; currentMass : "+ currentMass);
    }

    public void grow(int amount)
    {
        if (!isServer)
            return;
        currentMass += amount;
    }

    public void shrink(int amount)
    {
        if (!isServer)
            return;
           
        currentMass -= amount;
        if (currentMass <= 0)
        {
            currentMass = 0;
            Debug.Log("Death!");
        }
    }

    private void updateMassLabel(int currentMass)
    {
        if (massLabel == null)
        {
            Debug.Log("massLabel == null");
        }
        else
        {
            massLabel.text = "Current Mass: " + currentMass;
        }
        
    }

}
