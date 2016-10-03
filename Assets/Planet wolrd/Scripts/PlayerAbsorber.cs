﻿using UnityEngine;
using System.Collections;

public class PlayerAbsorber : MonoBehaviour {

	private GameObject player;
    private Mass myMass;
    private MassViewController massView;

    public bool isAbsorbing = false;
    
    void Start () {
        Debug.Log("This is " + name + " ; Tag : " + tag + " ; in PlayerAbsorber");
        player = this.gameObject;
        myMass = player.GetComponent<Mass>();
        massView = player.GetComponent<MassViewController>();
    }

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Boundary"){
			return;
		}

        Debug.Log("PlayerAbsorber OnTriggerEnter -- " + other.tag);

        GameObject enemy = other.gameObject;
        Mass enemyMass = enemy.GetComponent<Mass>();        

        if (myMass.currentMass > enemyMass.currentMass)
        {
            Debug.Log("PlayerAbsorber will StartAbsorb -- " + other.tag);
            isAbsorbing = true;
            massView.StartAbsorb();

            MassViewController enemyView = enemy.GetComponent<MassViewController>();
            enemyView.StartShrink();
        }		
	}

	void OnTriggerExit(Collider other) {

        Debug.Log("PlayerAbsorber OnTriggerExit -- " + other.tag);
        if (isAbsorbing)
        {

            Debug.Log("PlayerAbsorber will StopAbsorb -- " + other.tag);

            massView.StopAbsorb();
            isAbsorbing = false;

            GameObject enemy = other.gameObject;
            MassViewController enemyView = enemy.GetComponent<MassViewController>();
            enemyView.StopShrink();
        }
	}

}
