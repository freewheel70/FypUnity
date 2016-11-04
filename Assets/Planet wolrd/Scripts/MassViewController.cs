using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MassViewController : NetworkBehaviour
{
    [SyncVar(hook = "updateScale")]
    public float currentScale = 1.0f;

    public float absorbTimeGap = 0.5f;

    private Mass myMass;
    private GameObject player;   

    private Queue absorbTickets = Queue.Synchronized(new Queue());
    private bool isAbsorbing = false;

    private Queue shrinkTickets = Queue.Synchronized(new Queue());
    private bool isShrinking = false;

    private bool isDead = false;

    // Use this for initialization
    void Start () {
        Debug.Log("This is " + name + " ; Tag : " + tag + " ; in MassViewController");
        player = this.gameObject;
        myMass = player.GetComponent<Mass>();        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartAbsorb()
    {
        absorbTickets.Enqueue(new object());
        if (!isAbsorbing && absorbTickets.Count<=1)
        {
            StartCoroutine(AbsorbMass());
        }        
    }

    IEnumerator AbsorbMass()
    {
        isAbsorbing = true;
        while (absorbTickets.Count>0)
        {
            myMass.grow(5 * absorbTickets.Count);

            Debug.Log(name + " current mass " + myMass.currentMass);

            float newsacle = myMass.currentMass * 1.0f / myMass.initMass;
            
            currentScale = newsacle;

            PlayerAbsorber playerAbsorber = player.GetComponent<PlayerAbsorber>();
            if (playerAbsorber != null)
            {
                playerAbsorber.checkEnemyList();
            }
            
            yield return new WaitForSeconds(absorbTimeGap);
        }

        isAbsorbing = false;
        Debug.Log("Stop absorb");
    }

    public void StopAbsorb()
    {
        absorbTickets.Dequeue();
    }

    public void StartShrink()
    {
        shrinkTickets.Enqueue(new object());
        if(!isShrinking && shrinkTickets.Count <= 1 && !isDead)
        {
            StartCoroutine(ShrinkMass());
        }                
    }

    IEnumerator ShrinkMass()
    {
        isShrinking = true;
        while (shrinkTickets.Count > 0 && !isDead)
        {
            isDead = (myMass.shrink(5 * shrinkTickets.Count) == 0);
            if (isDead)
            {                              
                break;
            }

            float newsacle = myMass.currentMass * 1.0f / myMass.initMass;

            currentScale = newsacle;            

            yield return new WaitForSeconds(absorbTimeGap);
        }

        isShrinking = false;
        Debug.Log("Stop shink");
    }

    public void StopShrink()
    {
        shrinkTickets.Dequeue();
    }

    public void updateScale(float currentScale)
    {
        player.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

}
