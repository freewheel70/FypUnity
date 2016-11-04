using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}


public class PlayerMover : NetworkBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;

    [SyncVar(hook = "OnPlayerIDChanged")]
    public string playerID;

    Camera playerCam;
    Transform labelHolder;
	TextMesh playerIDLabel;
    TextMesh massLabel;

    void Awake()
    {
        Debug.Log("This is " + name + " ; Tag : " + tag + " ; in PlayerMover");
        playerCam = GetComponentInChildren<Camera>();
        playerCam.gameObject.SetActive(false);
        labelHolder = transform.Find("LabelHolder");
		playerIDLabel = labelHolder.Find("IDLabel").GetComponent<TextMesh>();
        massLabel = labelHolder.Find("MassLabel").GetComponent<TextMesh>();
    }


    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //float moveHorizontal = Input.acceleration.x;
        //float moveVertical = Input.acceleration.y;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);	
	
    }


    [Command]
    void CmdSetPlayerID(string newID)
    {
        playerID = newID;
    }

    public override void OnStartLocalPlayer()
    {

        GetComponent<MeshRenderer>().material.color = Color.red;

        string myPlayerID = string.Format("Player {0}", netId.Value);
        CmdSetPlayerID(myPlayerID);

        playerCam.gameObject.SetActive(true);
    }

    public override void OnStartClient()
    {
        OnPlayerIDChanged(playerID);
    }

    void OnPlayerIDChanged(string newValue)
    {
        Debug.Log("Player ID : " + newValue);
        
		playerIDLabel.text = newValue;
    }
}
