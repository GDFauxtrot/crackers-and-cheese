using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;

public class NetworkUIManager : MonoBehaviour {

    public Button hostButton, joinButton;
    public InputField ipInputField;
    public Button clientCancel;
    public GameObject errorPanel;
    public Text clientConnecting;

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
    }

    public void JoinButtonPressed() {
        if (ipInputField.text.Length == 0) {
            networkManager.networkAddress = "localhost";
        } else {
            networkManager.networkAddress = ipInputField.text;
        }
        networkManager.StartClient();

        SetShowClientConnecting(true);
    }

    public void SetPanelIsHidden(bool hide) {
        // Rather than nuke the manager with .SetActive(false), just hide the components so the manager doesn't deactivate
        GetComponent<Image>().enabled = !hide;
        foreach (Transform go in transform) {
            go.gameObject.SetActive(!hide);
        }
        clientCancel.gameObject.SetActive(false);
        clientConnecting.gameObject.SetActive(false);
    }

    public void SetShowClientConnecting(bool show) {
        clientCancel.gameObject.SetActive(show);
        clientConnecting.gameObject.SetActive(show);
        hostButton.gameObject.SetActive(!show);
        joinButton.gameObject.SetActive(!show);
        ipInputField.gameObject.SetActive(!show);
    }

    public void ClientConnectingCancelPressed() {
        networkManager.StopClient();
        
        SetShowClientConnecting(false);
    }

    public async void ShowErrorOnMenu(string error, float time) {
        if (GetComponent<Image>().enabled) {
            errorPanel.transform.GetChild(0).GetComponent<Text>().text = error;
            errorPanel.SetActive(true);

            await Task.Delay(TimeSpan.FromSeconds(time));

            errorPanel.SetActive(false);
            errorPanel.transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }
}
