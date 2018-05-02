using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectChute : NetworkBehaviour {

    public List<GameObject> randomObjectsToSpawn;

    public GameObject spawnPoint;

    float cooldown = 1f;
    float currentCooldown;

    public void SpawnObject(GameObject objectPrefab, bool applySomeFunRotation = false) {
        GameObject obj = Instantiate(objectPrefab);
        NetworkServer.Spawn(obj);
        obj.transform.position = spawnPoint.transform.position;

        if (applySomeFunRotation) {
            if (obj.GetComponent<Rigidbody>() != null) {
                obj.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-100,100),Random.Range(-100,100),Random.Range(-100,100)));
            }
        }
    }

    void Update() {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
    }

    public void SpawnObject(bool applySomeFunRotation = false) {
        if (currentCooldown > 0)
            return;

        SpawnObject(randomObjectsToSpawn[Random.Range(0,randomObjectsToSpawn.Count-1)], applySomeFunRotation);
        currentCooldown = cooldown;
    }
}
