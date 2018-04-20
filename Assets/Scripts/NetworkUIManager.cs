using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkUIManager : MonoBehaviour {

    public Button hostButton, joinButton;
    public InputField ipInputField;

    public CustomNetworkManager networkManager;

	void Start () {
        // This is muy importante
		if (networkManager == null) {
            Debug.LogError("Network Manager was not properly set up! Doing GO.Find to locate as backup method...");
            GameObject nm = GameObject.Find("NetworkManager");
            if (nm == null) {
                nm = GameObject.Find("Network Manager");
                if (nm == null) {
                    Debug.Log("Yep, couldn't find it. Fix this.");
                } else {
                    networkManager = nm.GetComponent<CustomNetworkManager>();
                }
            } else {
                networkManager = nm.GetComponent<CustomNetworkManager>();
            }
        }
	}
	
	void Update () {
		
	}

    public void HostButtonPressed() {
        if (ipInputField.text.Length == 0) {
            networkManager.networkAddress = "localhost";
        } else {
            networkManager.networkAddress = ipInputField.text;
        }
        networkManager.StartHost();
        //networkManager.StartHost(networkManager.connectionConfig, networkManager.maxConnections);

        SetPanelIsHidden(true);
    }

    public void JoinButtonPressed() {
        if (ipInputField.text.Length == 0) {
            networkManager.networkAddress = "localhost";
        } else {
            networkManager.networkAddress = ipInputField.text;
        }
        Debug.Log(networkManager.connectionConfig);
        networkManager.StartClient();
        //networkManager.StartClient(networkManager.matchInfo, networkManager.connectionConfig, networkManager.networkPort);

        SetPanelIsHidden(true);
    }

    void SetPanelIsHidden(bool hide) {
        // Rather than nuke the manager with .SetActive(false), just hide the components so the manager doesn't deactivate
        GetComponent<Image>().enabled = !hide;
        foreach (Transform go in transform) {
            go.gameObject.SetActive(!hide);
        }
    }
}
