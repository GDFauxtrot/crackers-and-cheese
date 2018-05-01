using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour {

    public GameManager gameManager;

    public float closeAngle;
    public float closeMinZ;
    public float farAngle;
    public float farMinZ;
    public float backZ;
    public float minZ, maxZ;

	void Start () {
		
	}
	
	void Update () {
        //if (gameManager.player1 != null && gameManager.player2) {
        //    GameObject p1 = gameManager.player1;
        //    GameObject p2 = gameManager.player2;
        //    float minimumZ = Mathf.Min(p1.transform.position.z, p2.transform.position.z);
        //    float maximumZ = Mathf.Max(p1.transform.position.z, p2.transform.position.z);
        //    if (minimumZ < transform.position.z + minZ) {
        //        transform.position = new Vector3(transform.position.x, transform.position.y, minimumZ - minZ);
        //    }
        //    if (maximumZ > transform.position.z + maxZ) {
        //        transform.position = new Vector3(transform.position.x, transform.position.y, maximumZ - maxZ);
        //    }
        //}

        if (gameManager.player1 != null && gameManager.player2 != null) {
            GameObject p1 = gameManager.player1;
            GameObject p2 = gameManager.player2;

            // Get middle point
            float middleZ = (p1.transform.position.z + p2.transform.position.z) / 2f;

            // Set camera rotation
            transform.LookAt(new Vector3(transform.position.x, 0, middleZ)); // CHANGE 0 TO Y VALUE WHEN THAT BECOMES IMPLEMENTED
            float currentAngle = Mathf.Clamp(transform.rotation.eulerAngles.x, farAngle, closeAngle);
            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.right);

            // Now comes the fun part - adjust position based on angle & player positions

            // Percentage of x between two values (a,b) = (x-a)/(b-a)
            float percent = (currentAngle - farAngle) / (closeAngle - farAngle);
            
            minZ = Mathf.Lerp(farMinZ, closeMinZ, percent);
            //maxZ = Mathf.Lerp(farMaxZ, closeMaxZ, percent);

            if (p1.transform.position.z > transform.position.z + backZ && p2.transform.position.z > transform.position.z + backZ) {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Min(p1.transform.position.z, p2.transform.position.z) - backZ);
            } 
        }
    }
}
