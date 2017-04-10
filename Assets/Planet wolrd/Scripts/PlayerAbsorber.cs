using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAbsorber : NetworkBehaviour{

    private GameObject player;
    private Mass myMass;
    private MassViewController massView;
  
    ArrayList enemyList = new ArrayList();
    public int enemyCount = 0;
    private AudioSource[] audios;
    private PlayerID myPlayerId;

    void Start(){        
        player = this.gameObject;
        myMass = player.GetComponent<Mass>();
        massView = player.GetComponent<MassViewController>();
        audios = GetComponents<AudioSource>();
        myPlayerId = player.GetComponent<PlayerID>();
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Boundary"){
            return;
        }     

        GameObject enemy = other.gameObject;
        Mass enemyMass = enemy.GetComponent<Mass>();

        if (enemy == null || enemyMass == null)
            return;

        if (myMass.currentMass > enemyMass.currentMass){
            enemyList.Add(enemy);
            enemyCount = enemyList.Count;

            massView.StartAbsorb();

            MassViewController enemyView = enemy.GetComponent<MassViewController>();
            PlayerID enemyId = enemy.GetComponent<PlayerID>();
            enemyView.StartShrink();
            if (enemy.tag == "Player"){
                Debug.Log("Call RpcPlayWarning !");
                RpcPlayWarning();
            }

#if UNITY_ANDROID
            if (isLocalPlayer) { 
                Handheld.Vibrate();
            }
#endif             
                
        }
    }

    [ClientRpc]
    public void RpcPlayWarning(){               
        if (!isLocalPlayer) {            
            audios[0].Play();          
        }
    }

    void OnTriggerExit(Collider other){

        GameObject enemy = other.gameObject;
        Mass enemyMass = enemy.GetComponent<Mass>();

        if (myMass.currentMass > enemyMass.currentMass){
            MassViewController enemyView = enemy.GetComponent<MassViewController>();
            enemyView.StopShrink();
            massView.StopAbsorb();
        }

        enemyList.Remove(other.gameObject);
        enemyCount = enemyList.Count;
    }

    public void checkEnemyList(){

        ArrayList deadEnemy = new ArrayList();
        IEnumerator enumerator = enemyList.GetEnumerator();
        while (enumerator.MoveNext()){
            GameObject enemy = (GameObject)enumerator.Current;
            if (enemy != null && enemy.GetComponent<Mass>().currentMass == 0){
                deadEnemy.Add(enemy);
            }
        }
        IEnumerator deadEnumerator = deadEnemy.GetEnumerator();
        while (deadEnumerator.MoveNext()){
            GameObject enemy = (GameObject)deadEnumerator.Current;
            enemyList.Remove(enemy);
            if (enemy == null)
                return;

            massView.StopAbsorb();            
            CmdDestory(enemy);
        }
    }

    [Command]
    public void CmdDestory(GameObject gameObj){
        if (gameObj == null)
            return;

        if (gameObj.tag == "Player"){           
            RpcRespawn(gameObj);
            gameObj.GetComponent<MassViewController>().Reset();
        }else{
            Destroy(gameObj);
        }
    }

    [ClientRpc]
    void RpcRespawn(GameObject gameObj){        
        gameObj.GetComponent<PlayerMover>().Respawn();            
    }

}
