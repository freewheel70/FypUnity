using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HazardSpawner : NetworkBehaviour {
    public GameObject hazard;
    public Boundary boundary;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    private bool gameOver;

    // Use this for initialization
    public override void OnStartServer()
    {
        Debug.Log("GameController isServer ");
        gameOver = false;
        StartCoroutine(SpawnWaves());
       
    }

    void SpwanHazards()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 0, Random.Range(boundary.zMin, boundary.zMax));
        Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GameObject haza = (GameObject)Instantiate(hazard, spawnPosition, spawnRotation);
        NetworkServer.Spawn(haza);
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                SpwanHazards();

                if (gameOver)
                {
                    break;
                }
                yield return new WaitForSeconds(spawnWait);
            }

            if (gameOver)
            {
                break;
            }
            yield return new WaitForSeconds(waveWait);

        }
    }
}
