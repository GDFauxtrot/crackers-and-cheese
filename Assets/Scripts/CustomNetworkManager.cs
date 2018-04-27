using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

// To override base NetworkManager functions and do our own thing, we must use this
public class CustomNetworkManager : NetworkManager {

    public GameManager gameManager;
    public NetworkUIManager networkUIManager;
    public NetworkStartPosition p1Start, p2Start;
    GameObject player1, player2;
    
    void Start() {
        //StartMatchMaker();
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        if (success) {
            networkUIManager.netMenuPanel.SetActive(false);
            networkUIManager.netToLanButton.gameObject.SetActive(false);
        } else {
            networkUIManager.DisconnectBackToLANMenu();
            networkUIManager.ShowErrorOnMenu("ERROR: Could not create net match!", 5);
        }
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo) {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        if (success) {
            networkUIManager.netConnectingPanel.SetActive(false);
        } else {
            networkUIManager.DisconnectBackToLANMenu();
            networkUIManager.ShowErrorOnMenu("ERROR: Could not join net match!", 5);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
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
        networkUIManager.DisconnectBackToLANMenu();
        networkUIManager.ShowErrorOnMenu("ERROR: Client disconnected!", 5f);
        StopServer();
    }
    
    // OnClientDisconnect = disconect run on client side (SERVER drops out)
    public override void OnClientDisconnect(NetworkConnection conn) {
        networkUIManager.DisconnectBackToLANMenu();
        networkUIManager.ShowErrorOnMenu("ERROR: Server disconnected!", 5f);
        StopClient();
    }
}
