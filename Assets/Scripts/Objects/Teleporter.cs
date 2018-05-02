using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Teleporter : NetworkBehaviour {

    public ParticleSystem particles;
    public Teleporter otherTeleporter;
    public GameObject spawnPoint;

    List<GameObject> teleportCollidingObjects;

    float cooldown = 1f;
    float currentCooldown;

	void Start () {
		teleportCollidingObjects = new List<GameObject>();
	}
	
	void Update () {
		if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
	}

    public void DoTeleport() {
        if (otherTeleporter == null) {
            Debug.LogError("Other teleporter hasn't been connected!");
            return;
        }
        if (teleportCollidingObjects.Count == 0) {
            return;
        }
        if (currentCooldown > 0)
            return;

        currentCooldown = cooldown;

        particles.Play();

        // Creating a "zone" for objects to spawn requires time that we don't have, so just do one at a time
        teleportCollidingObjects[0].transform.position = otherTeleporter.spawnPoint.transform.position;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Grabbable") {
            teleportCollidingObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Grabbable") {
            teleportCollidingObjects.Remove(other.gameObject);
        }
    }
}
