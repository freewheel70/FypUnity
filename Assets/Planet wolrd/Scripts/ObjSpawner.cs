using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ObjSpawner : NetworkBehaviour {
    public GameObject spawnee;
    public Boundary boundary;
    public int waveCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public bool oneTime = false;

   
    public override void OnStartServer(){              
        StartCoroutine(SpawnWaves());
    }

    void SpwanObjs(){
        Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), 
                                            0, 
                                            Random.Range(boundary.zMin, boundary.zMax));

        Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        GameObject obj = (GameObject)Instantiate(spawnee, spawnPosition, spawnRotation);

        NetworkServer.Spawn(obj);

    }

    IEnumerator SpawnWaves()  {
        yield return new WaitForSeconds(startWait);

        while (true){
            for (int i = 0; i < waveCount; i++){
                SpwanObjs();
               
                yield return new WaitForSeconds(spawnWait);
            }

            if (oneTime){
                break;
            }

            yield return new WaitForSeconds(waveWait);
        }
    }
}
