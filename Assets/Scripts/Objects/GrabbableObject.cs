using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrabbableObject : NetworkBehaviour {

    [SyncVar]
    public int assignedLayer; // For switching back from grabbed

    [SyncVar]
    public bool isGrabbed;

	void Start () {
		assignedLayer = gameObject.layer;
	}

    [ClientRpc]
    void RpcGrabbed(bool grabbed) {
        if (isGrabbed) {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("GrabbedObject");
            Debug.Log("buh");
        } else {
            GetComponent<Rigidbody>().isKinematic = false;
            gameObject.layer = assignedLayer;
        }
    }

    public void SetGrabbed(bool grabbed) {
        isGrabbed = grabbed;
        //RpcGrabbed(grabbed);
    }
}
