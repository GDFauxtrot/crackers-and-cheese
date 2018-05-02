using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectChute : NetworkBehaviour {

    public List<GameObject> randomObjectsToSpawn;

    public GameObject spawnPoint;

    float cooldown = 1f;
    float currentCooldown;

    [Command]
    public void CmdSpawnObject(GameObject objectPrefab, bool applySomeFunRotation) {
        GameObject obj = Instantiate(objectPrefab);
        obj.transform.position = spawnPoint.transform.position;

        if (applySomeFunRotation) {
            if (obj.GetComponent<Rigidbody>() != null) {
                obj.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-100,100),Random.Range(-100,100),Random.Range(-100,100)));
            }
        }
        NetworkServer.Spawn(obj);
    }

    void Update() {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
    }

    public void SpawnObject(bool applySomeFunRotation = false) {
        if (currentCooldown > 0)
            return;

        CmdSpawnObject(randomObjectsToSpawn[Random.Range(0,randomObjectsToSpawn.Count-1)], applySomeFunRotation);
        currentCooldown = cooldown;

        //CmdSpawnObject(randomObjectsToSpawn[Random.Range(0,randomObjectsToSpawn.Count-1)], applySomeFunRotation);
    }

    //[Command]
    //void CmdSpawnObject(GameObject objectPrefab, bool applySomeFunRotation = false) {

    //}
}
