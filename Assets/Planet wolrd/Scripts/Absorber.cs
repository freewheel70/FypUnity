using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class Absorber : NetworkBehaviour
{

    protected ArrayList victimList = new ArrayList();
    protected int victimCount = 0;


    void OnTriggerEnter(Collider other){
        handleEnter(other);
    }

    void OnTriggerExit(Collider other){
        handlerExit(other);
    }

    protected void handleEnter(Collider other) {
        if (other.tag == "Boundary"){
            return;
        }

        GameObject victim = other.gameObject;
        Mass enemyMass = victim.GetComponent<Mass>();

        if (victim == null || enemyMass == null)
            return;

        if (shouldAbsorb(enemyMass))
        {
            victimList.Add(victim);
            victimCount = victimList.Count;
          
            growUpByOne();

            victim.GetComponent<MassViewController>().StartShrink();

            playEffects(victim);           

        }
    }

    protected abstract void playEffects(GameObject victim);
    protected abstract void growUpByOne();
    protected abstract bool shouldAbsorb(Mass victimMass);

   
    protected void handlerExit(Collider other)
    {
        GameObject enemy = other.gameObject;
        Mass victimMass = enemy.GetComponent<Mass>();

        if (shouldStopAbsorb(victimMass))
        {
            enemy.GetComponent<MassViewController>().StopShrink();
            
            stopGrowUpByOne();
        }

        victimList.Remove(other.gameObject);
        victimCount = victimList.Count;
    }

    protected abstract void stopGrowUpByOne();
    protected abstract bool shouldStopAbsorb(Mass victimMass);

    public void checkEnemyList()
    {

        ArrayList deadEnemy = new ArrayList();

        IEnumerator enumerator = victimList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            GameObject enemy = (GameObject)enumerator.Current;
            if (enemy != null && enemy.GetComponent<Mass>().currentMass == 0)
            {
                deadEnemy.Add(enemy);
            }
        }        

        IEnumerator deadEnumerator = deadEnemy.GetEnumerator();
        while (deadEnumerator.MoveNext())
        {
            GameObject enemy = (GameObject)deadEnumerator.Current;
            victimList.Remove(enemy);
            if (enemy == null)
                return;

            stopGrowUpByOne();
            CmdDestory(enemy);
        }       
    }

    [Command]
    public void CmdDestory(GameObject gameObj)
    {
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
    void RpcRespawn(GameObject gameObj)
    {
        gameObj.GetComponent<PlayerMover>().Respawn();
    }
}
