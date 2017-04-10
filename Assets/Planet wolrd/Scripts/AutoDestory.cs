using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AutoDestory : NetworkBehaviour{

	// Use this for initialization
	void Start () {
        Debug.Log("AutoDestory Start");
        StartCoroutine(AutoClear(2, this.gameObject));
    }

    IEnumerator AutoClear(float time,GameObject obj){
        yield return new WaitForSeconds(time);
        Debug.Log("AutoClear Done");
        Destroy(obj);
    }
}
