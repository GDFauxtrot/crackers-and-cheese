using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Vector3 startPoint, endPoint;
    public float speed;

    GameManager gameManager;

	void Start () {
		transform.position = startPoint;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	void FixedUpdate () {
        transform.position = Vector3.Lerp(startPoint, endPoint, (Mathf.Sin(gameManager.roundTime * speed)+1)/2f);
	}
}
