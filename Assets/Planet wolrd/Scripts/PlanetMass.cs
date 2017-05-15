using UnityEngine;
using System.Collections;

public class PlanetMass : Mass {

    protected override void initMassLabel(int currentMass)
    {
        massLabel = transform.Find("LabelHolder").Find("MassLabel").GetComponent<TextMesh>();

        updateMassLabel(currentMass);
    }
}
