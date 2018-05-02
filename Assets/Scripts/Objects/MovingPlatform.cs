using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Normal - platform moves until activated by a button
// NormalInvertedActivation - platform doesn't move until activated
// StartToEnd - platform rests at start point, activation moves it to end point

public enum PlatformMovementType { Normal, NormalInvertedActivation, StartToEnd };

public class MovingPlatform : MonoBehaviour {

    public Vector3 startPoint, endPoint;
    public float speed;
    public PlatformMovementType movementType;
    
    public bool activated;

    GameManager gameManager;

    bool warned;

	void Start () {
		transform.localPosition = startPoint;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	void FixedUpdate () {
        if (gameManager.gameIsRunning) {
            KeyValuePair<int, float> info = gameManager.GetPlatformInfoFromStruct(gameObject);
            if (info.Key != -1) {
                if (movementType == PlatformMovementType.Normal || movementType == PlatformMovementType.NormalInvertedActivation) {
                    transform.localPosition = Vector3.Lerp(startPoint, endPoint, (Mathf.Sin(info.Value * speed)+1)/2f);
                } else if (movementType == PlatformMovementType.StartToEnd) {
                    transform.localPosition = Vector3.Lerp(startPoint, endPoint, info.Value);
                }
            } else {
                if (!warned) {
                    Debug.LogError("Moving platform at " + gameObject.transform.localPosition + " was not picked up by the network manager for synchronization!");
                    warned = true;
                }
                transform.localPosition = Vector3.Lerp(startPoint, endPoint, (Mathf.Sin(gameManager.roundTime * speed)+1)/2f);
            }
        }
	}
}
