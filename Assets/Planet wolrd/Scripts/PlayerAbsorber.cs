﻿using UnityEngine;
using System.Collections;

public class PlayerAbsorber : MonoBehaviour {

	private GameObject player;
	private bool insideScope = false;
	public int absorbTimeGap;


	// Use this for initialization
	void Start () {
		player = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Boundary"){
			return;
		}

		Debug.Log (other.tag + " enter planet scope");
		insideScope = true;
		StartCoroutine (AbsorbMass (other));
	}

	void OnTriggerExit(Collider other) {
		Debug.Log (other.tag + " exit planet scope");
		insideScope = false;
	} 

	IEnumerator AbsorbMass(Collider other){
		while (insideScope) {

			Vector3 originScale = player.transform.localScale;

			player.transform.localScale = new Vector3 (originScale.x * 1.01f, originScale.y * 1.01f, originScale.z * 1.01f);

		
			yield return new WaitForSeconds (absorbTimeGap);
		}

		Debug.Log ("Stop absorb");
	}
}
