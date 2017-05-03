using UnityEngine;
using System.Collections;
using System;

public class BlackHoleAbsorber : Absorber
{

    protected override bool shouldAbsorb(Mass victimMass)
    {
        return true;
    }

    protected override void playEffects(GameObject victim)
    {
        //TODO
    }

    protected override void growUpByOne()
    {
        return;//Not need to grow black hole
    }

    protected override void stopGrowUpByOne()
    {
        return;//Not need to stop grow black hole
    }

    protected override bool shouldStopAbsorb(Mass victimMass)
    {
        return true;
    }
}
