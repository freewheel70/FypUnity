using UnityEngine;
using System.Collections;
using System;

public class BlackHoleAbsorber : Absorber
{
    private bool isAbsorbing = false;
    private float absorbTimeGap = 0.5f;

    protected override bool shouldAbsorb(Mass victimMass)
    {
        Debug.Log("BlackHoleAbsorber shouldAbsorb ! ");
        return true;
    }

    protected override void playEffects(GameObject victim)
    {
        //TODO
    }

    protected override void growUpByOne()
    {
        if(!isAbsorbing && victimCount > 0)
        {
            isAbsorbing = true;
            StartCoroutine(AbsorbNearMass());
        }
        return;//Not need to grow black hole
    }

    IEnumerator AbsorbNearMass()
    {
        while(isAbsorbing && victimCount > 0)
        {
            checkEnemyList();
            yield return new WaitForSeconds(absorbTimeGap);
        }
        isAbsorbing = false;
    }

    protected override void stopGrowUpByOne()
    {
        return;//Not need to stop grow black hole
    }

    protected override bool shouldStopAbsorb(Mass victimMass)
    {
        return true;
    }

    protected override string getID()
    {
        return "BlackHoleAbsorber";
    }
}
