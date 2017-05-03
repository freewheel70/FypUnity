using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BlackHoleAbsorber : NetworkBehaviour
{
    ArrayList victimList = new ArrayList();
    private int victimCount = 0;

    // Use this for initialization
    void Start(){

    }

    // Update is called once per frame
    void Update(){

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;
        }

        GameObject enemy = other.gameObject;
        Mass enemyMass = enemy.GetComponent<Mass>();

        if (enemy == null || enemyMass == null)
            return;

        
        victimList.Add(enemy);
        victimCount = victimList.Count;

        //StartAbsorb

        MassViewController enemyView = enemy.GetComponent<MassViewController>();
        PlayerID enemyId = enemy.GetComponent<PlayerID>();
        enemyView.StartShrink();

        if (enemy.tag == "Player"){
            Debug.Log("Call RpcPlayWarning !");
            //RpcPlayWarning();
        }

#if UNITY_ANDROID
        if (isLocalPlayer){
            Handheld.Vibrate();
        }
#endif
        
    }


    void OnTriggerExit(Collider other){
        GameObject enemy = other.gameObject;
        Mass enemyMass = enemy.GetComponent<Mass>();
        
        MassViewController enemyView = enemy.GetComponent<MassViewController>();
        enemyView.StopShrink();              

        victimList.Remove(other.gameObject);
        victimCount = victimList.Count;
    }

}
