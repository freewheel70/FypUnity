using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AutoDestory : NetworkBehaviour{
	
	void Start () {       
        StartCoroutine(AutoClear(2, this.gameObject));
    }

    IEnumerator AutoClear(float time,GameObject obj){
        yield return new WaitForSeconds(time);
      
        Destroy(obj);
    }
}
