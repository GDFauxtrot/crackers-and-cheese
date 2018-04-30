using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is basically a logic gate component for activators in the game (like buttons). They need to be activated in some logic gate-like fashion to perform some action.

public enum CombinationGateType { AND, OR, XOR }

public class ActivationCombinator : MonoBehaviour {

    public CombinationGateType gateType;
    public List<GameObject> activators;
    public List<GameObject> onListeners, offListeners;

    public bool activated;
    bool activatedPreviously;

	void Start () {
		
	}
	
    // To help see it (won't show up in-game, obviously)
    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 1);
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.color = Color.yellow;
        foreach (GameObject a in activators) {
            Gizmos.DrawLine(transform.position, a.transform.position);
        }
        Gizmos.color = Color.red;
        foreach (GameObject on in onListeners) {
            Gizmos.DrawLine(transform.position, on.transform.position);
        }
        foreach (GameObject off in offListeners) {
            if (onListeners.Contains(off))
                Gizmos.color = new Color(1, 0, 1);
            else
                Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, off.transform.position);
        }
    }

	void Update () {
        bool allActivated = true;
        bool oneActivated = false;
		foreach (GameObject activator in activators) {
            PushButton button = activator.GetComponent<PushButton>();
            if (button != null) {
                if (button.pressed) {
                    oneActivated = true;
                } else {
                    allActivated = false;
                }
            }
        }

        activatedPreviously = activated;

        switch (gateType) {
            case CombinationGateType.AND:
                if (allActivated)
                    activated = true;
                break;
            case CombinationGateType.OR:
                if (oneActivated)
                    activated = true;
                break;
            case CombinationGateType.XOR:
                if (oneActivated && !allActivated)
                    activated = true;
                break;
        }

        // Check to see if a change happened
        if (activated != activatedPreviously) {
            if (activated) {
                foreach (GameObject listener in onListeners) {
                    // All types of listeners go here to keep things nice n tidy
                    MovingPlatform mp = listener.GetComponent<MovingPlatform>();
                    ObjectChute oc = listener.GetComponent<ObjectChute>();

                    if (mp != null) {
                        mp.activated = true;
                    }
                    if (oc != null) {
                        oc.SpawnObject();
                    }
                }
            } else {
                foreach (GameObject listener in offListeners) {
                    // All types of listeners go here to keep things nice n tidy
                    MovingPlatform mp = listener.GetComponent<MovingPlatform>();
                    ObjectChute oc = listener.GetComponent<ObjectChute>();

                    if (mp != null) {
                        mp.activated = false;
                    }
                    if (oc != null) {
                        oc.SpawnObject();
                    }
                }
            }
        }
	}
}
