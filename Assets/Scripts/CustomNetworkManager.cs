using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// To override base NetworkManager functions and do our own thing, we must use this
public class CustomNetworkManager : NetworkManager {

    public GameManager gameManager;
    public NetworkUIManager startMenuUIManager;
    public NetworkStartPosition p1Start, p2Start;
    GameObject player1, player2;
    
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        //base.OnServerAddPlayer(conn, playerControllerId);
        startMenuUIManager.SetPanelIsHidden(true);

        if (player1 == null) {
            player1 = Instantiate(playerPrefab, p1Start.transform.position, p1Start.transform.rotation);
            NetworkServer.AddPlayerForConnection(conn, player1, playerControllerId); 
        } else if (player2 == null) {
            player2 = Instantiate(playerPrefab, p2Start.transform.position, p2Start.transform.rotation);
            NetworkServer.AddPlayerForConnection(conn, player2, playerControllerId); 
        } else {
            Debug.LogError("Too many players!! WTF?");
        }
        
        if (player1 != null && player2 != null) {
            gameManager.BeginGame(player1, player2);
        }
    }

    // OnServerDisconnect = disconnect run on server side (CLIENT disconnected)
    public override void OnServerDisconnect(NetworkConnection conn) {
        //base.OnServerDisconnect(conn);
        startMenuUIManager.SetPanelIsHidden(false);
        startMenuUIManager.ShowErrorOnMenu("ERROR: Client disconnected!", 5f);
        StopServer();
        //StopHost();
    }
    
    // OnClientDisconnect = disconect run on client side (SERVER drops out)
    public override void OnClientDisconnect(NetworkConnection conn) {
        //base.OnClientDisconnect(conn);
        startMenuUIManager.SetPanelIsHidden(false);
        startMenuUIManager.ShowErrorOnMenu("ERROR: Server disconnected!", 5f);
        StopClient();
        //StopHost();
    }
}
