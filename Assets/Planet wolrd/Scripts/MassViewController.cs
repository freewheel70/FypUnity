using UnityEngine;
using System.Collections;

public class MassViewController : MonoBehaviour {

   
    public int absorbTimeGap = 1;

    private Mass myMass;
    private GameObject player;   

    private Queue absorbTickets = Queue.Synchronized(new Queue());

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
        StartCoroutine(AbsorbMass());
    }

    IEnumerator AbsorbMass()
    {
        while (absorbTickets.Count!=0)
        {

            Vector3 originScale = player.transform.localScale;

            player.transform.localScale = new Vector3(originScale.x * 1.01f, originScale.y * 1.01f, originScale.z * 1.01f);

            myMass.grow(10);

            yield return new WaitForSeconds(absorbTimeGap);
        }

        Debug.Log("Stop absorb");
    }

    public void StopAbsorb()
    {
        absorbTickets.Dequeue();
    }

    public void StartShrink()
    {
        myMass.shrink(10);
        //Todo
    }

    public void StopShrink()
    {

    }
}
