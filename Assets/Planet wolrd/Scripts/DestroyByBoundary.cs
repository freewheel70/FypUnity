using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
	void OnTriggerExit (Collider other) 
	{
		Debug.Log ("DestroyByBoundary Destory object : " + other.tag);
        if (other.tag != "Player")
        {
            Destroy(other.gameObject);
        }
		
	}
}