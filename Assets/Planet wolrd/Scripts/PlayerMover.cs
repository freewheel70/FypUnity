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
    public string playerName;

    [SyncVar(hook = "OnPlayerColorChanged")]
    public Color playerColor;

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

    void Start()
    {
        this.gameObject.transform.position = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
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


    public override void OnStartLocalPlayer()
    {
        playerCam.gameObject.SetActive(true);
    }

    public override void OnStartClient()
    {
        OnPlayerIDChanged(playerName);
    }

    void OnPlayerIDChanged(string newValue)
    {
        Debug.Log("Player ID : " + newValue);
        
		playerIDLabel.text = newValue;
    }

    void OnPlayerColorChanged(Color newColor)
    {
        GetComponent<MeshRenderer>().material.color = newColor;
    }
}
