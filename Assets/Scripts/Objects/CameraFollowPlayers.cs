using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour {

    public GameManager gameManager;

    public float minZ, maxZ;

	void Start () {
		
	}
	
	void Update () {
		if (gameManager.player1 != null && gameManager.player2) {
            GameObject p1 = gameManager.player1;
            GameObject p2 = gameManager.player2;
            float minimumZ = Mathf.Min(p1.transform.position.z, p2.transform.position.z);
            float maximumZ = Mathf.Max(p1.transform.position.z, p2.transform.position.z);
            if (minimumZ < transform.position.z + minZ) {
                transform.position = new Vector3(transform.position.x, transform.position.y, minimumZ - minZ);
            }
            if (maximumZ > transform.position.z + maxZ) {
                transform.position = new Vector3(transform.position.x, transform.position.y, maximumZ - maxZ);
            }
        }
	}
}
