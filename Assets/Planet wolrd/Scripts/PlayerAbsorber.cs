using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAbsorber : NetworkBehaviour
{

	private GameObject player;
    private Mass myMass;
    private MassViewController massView;

    public bool isAbsorbing = false;
    ArrayList enemyList = new ArrayList();
    
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
            enemyList.Add(enemy);            

            Debug.Log("PlayerAbsorber will StartAbsorb -- " + other.tag);
            isAbsorbing = true;
            massView.StartAbsorb();

            MassViewController enemyView = enemy.GetComponent<MassViewController>();
            enemyView.StartShrink();
        }		
	}

	void OnTriggerExit(Collider other) {

        enemyList.Remove(other.gameObject);

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

    public void checkEnemyList()
    {
        //if (!isServer)
        //   return;

        ArrayList deadEnemy = new ArrayList();
        IEnumerator enumerator = enemyList.GetEnumerator();
        while(enumerator.MoveNext())
        {
            GameObject enemy =(GameObject)enumerator.Current;
            if (enemy.GetComponent<Mass>().currentMass == 0)
            {                
                deadEnemy.Add(enemy);
               
            }
        }
        IEnumerator deadEnumerator = deadEnemy.GetEnumerator();
        while (deadEnumerator.MoveNext())
        {
            GameObject enemy = (GameObject)deadEnumerator.Current;
            enemyList.Remove(enemy);
            massView.StopAbsorb();
            Destroy(enemy);
        }
    }

}
