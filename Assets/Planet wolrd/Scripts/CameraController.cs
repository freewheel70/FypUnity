using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private GameObject player;

    private Vector3 offset;

    private Quaternion rotation;

    // Use this for initialization
    void Start()
    {
        player = this.transform.parent.gameObject;
        offset = transform.position - player.transform.position;
        rotation = transform.rotation;
        Debug.Log(rotation);        
    }

    void Update()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        transform.rotation = rotation;
    }
}
