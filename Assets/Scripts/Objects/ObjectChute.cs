using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChute : MonoBehaviour {

    public GameObject spawnPoint;

    public void SpawnObject(GameObject objectPrefab, bool applySomeFunRotation = false) {
        GameObject obj = Instantiate(objectPrefab);
        obj.transform.position = spawnPoint.transform.position;

        if (applySomeFunRotation) {
            if (obj.GetComponent<Rigidbody>() != null) {
                obj.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-100,100),Random.Range(-100,100),Random.Range(-100,100)));
            }
        }
    }
}
