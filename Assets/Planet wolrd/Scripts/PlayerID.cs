using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour{

    [SyncVar]
    public string playerName = "Player";

    [SyncVar]
    public Color playerColor = Color.white;

    Transform labelHolder;
    TextMesh playerIDLabel;
    TextMesh massLabel;

    private Mass myMass;

    void Awake(){
        labelHolder = transform.Find("LabelHolder");
        massLabel = labelHolder.Find("MassLabel").GetComponent<TextMesh>();
        playerIDLabel = labelHolder.Find("IDLabel").GetComponent<TextMesh>();
    }
       
    void Start () {
        playerIDLabel.color = playerColor;
        massLabel.color = playerColor;
        playerIDLabel.text = playerName;
        myMass = this.gameObject.GetComponent<Mass>();
    }
	
}
