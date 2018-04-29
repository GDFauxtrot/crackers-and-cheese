using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

    int assignedLayer; // For switching back from grabbed

    public bool isGrabbed;

	void Start () {
		assignedLayer = gameObject.layer;
	}
	
    public void SetGrabbed(bool grabbed) {
        isGrabbed = grabbed;

        if (isGrabbed) {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("GrabbedObject");
        } else {
            GetComponent<Rigidbody>().isKinematic = false;
            gameObject.layer = assignedLayer;
        }
    }
}
