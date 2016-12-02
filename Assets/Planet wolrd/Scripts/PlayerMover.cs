﻿using UnityEngine;
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
    public float smoothing;

    [SyncVar]
    public string playerName = "Player";

    [SyncVar]
    public Color playerColor = Color.white;

    [SyncVar]
    public float rotateYVal = 0.01f;

    Camera playerCam;
    Transform labelHolder;
	TextMesh playerIDLabel;
    TextMesh massLabel;
    Rigidbody rb;
    public GameObject flame;
    private Vector2 touchOrigin = -Vector2.one;
    private Vector2 smoothDirection;
    private Vector2 direction = Vector2.zero;
    private bool touched  = false;
    private Mass myMass;

    void Awake()
    {
        Debug.Log("This is " + name + " ; Tag : " + tag + " ; in PlayerMover");
        playerCam = GetComponentInChildren<Camera>();
        playerCam.gameObject.SetActive(false);        
        labelHolder = transform.Find("LabelHolder");
		playerIDLabel = labelHolder.Find("IDLabel").GetComponent<TextMesh>();
        massLabel = labelHolder.Find("MassLabel").GetComponent<TextMesh>();
        rb = GetComponent<Rigidbody>();
        myMass = this.gameObject.GetComponent<Mass>();
    }

    public void Respawn()
    {
        if (isLocalPlayer)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        }
    }

    void Start()
    {
        this.gameObject.transform.position = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));

        //GetComponent<MeshRenderer>().material.color = playerColor;
        playerIDLabel.color = playerColor;
        massLabel.color = playerColor;
        playerIDLabel.text = playerName;
    }


    void FixedUpdate()
    {
        if (isLocalPlayer)
        {

            Vector3 movement = Vector3.zero;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            movement = new Vector3(moveHorizontal, 0, moveVertical);
#else

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            if(myTouch.phase == TouchPhase.Began)
            {
                if (!touched)
                {
                    touchOrigin = myTouch.position;
                    touched = true;
                }
                
            }           
            else if (myTouch.phase == TouchPhase.Moved)
            {
                Vector2 touchEnd = myTouch.position;
                Vector2 directionRaw = touchEnd - touchOrigin;
                direction = directionRaw.normalized;                                                         
            }        
            else if(myTouch.phase == TouchPhase.Ended )
            {
                touched = false;
                direction = Vector2.zero;
            }
        }

        smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
        movement = new Vector3(smoothDirection.x, 0.0f, smoothDirection.y);
#endif
            //flame.localPosition = movement.normalized;
            //Debug.Log("flame.position : " + flame.position);

            //flame.rotation = movement;
            float massFactor = myMass.currentMass / myMass.initMass ;
            massFactor = 1 - (massFactor - 1.0f)  * 0.1f;
          
            float currentSpeed = speed * (massFactor < 0.4f ? 0.4f : massFactor);
            rb.velocity = movement * currentSpeed;
            rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

            float rotateY = 0.0f;
            if (movement.x == 0.0f && movement.z == 0.0f)
            {
                rotateY = 0.01f;
                //flame.transform.localPosition = new Vector3(0, 0, 0);
                flame.SetActive(false);
            }
            else
            {
                flame.SetActive(true);
                //flame.transform.localPosition = new Vector3(0, 0, -0.7f);
                if (movement.x == 0.0f)
                {
                    rotateY = (movement.z > 0) ? 0.0f : 180.0f;
                }
                else if (movement.z == 0.0f)
                {
                    rotateY = (movement.x > 0) ? 90.0f : -90.0f;
                }
                else
                {
                    rotateY = Mathf.Atan2(movement.z, -movement.x) * Mathf.Rad2Deg - 90;
                    if (rotateY < 0) rotateY += 360;
                }
            }
            //Debug.Log(movement.ToString() + " rotateY " + rotateY);

            //rb.transform.localRotation = Quaternion.Euler(0.0f, rotateY, 0.0f);
            //rb.rotation = Quaternion.Euler(0.0f, rotateY, 0.0f);      

            this.rotateYVal = rotateY;
            CmdUpdateRoateVal(rotateY);
        }

        rb.rotation = Quaternion.Euler(0.0f, rotateYVal, 0.0f);
        if (rotateYVal == 0.01f)
        {
            flame.SetActive(false);
        }
        else
        {
            flame.SetActive(true);
        }

    }

    [Command]
    public void CmdUpdateRoateVal(float rotateY)
    {
        this.rotateYVal = rotateY;
    }


    public override void OnStartLocalPlayer()
    {
        playerCam.gameObject.SetActive(true);       
    }

}
