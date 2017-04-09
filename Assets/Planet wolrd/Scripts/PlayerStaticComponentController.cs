using UnityEngine;
using System.Collections;

public class PlayerStaticComponentController : MonoBehaviour {
	protected GameObject player;

    protected Vector3 offset;
    protected Quaternion rotation;

	// Use this for initialization
	void Start()
	{
		player = this.transform.parent.gameObject;
		offset = transform.position - player.transform.position;        
		rotation = transform.rotation;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		transform.position = player.transform.position + offset;
		transform.rotation = rotation;        
	}
}
