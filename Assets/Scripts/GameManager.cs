using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    [SyncVar]
    public bool gameIsRunning;

    [SyncVar]
    public GameObject player1, player2;

    [SyncVar]
    public float roundTime;

	void Start () {
		
	}
	
	void Update () {
		if (gameIsRunning) {
            roundTime += Time.deltaTime;
        }
	}

    public void BeginGame(GameObject p1, GameObject p2) {
        player1 = p1;
        player2 = p2;
        gameIsRunning = true;
    }
}
