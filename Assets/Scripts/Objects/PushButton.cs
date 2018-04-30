using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour {

    public GameObject buttonBase, buttonTop;
    public SpriteMask timerMask;
    public GameObject timerSprite;

    public float releaseTime;
    public bool pressed;
    bool timerPressed;

    float currentReleaseTime;

    public List<GameObject> onListeners, offListeners;

    public ActivationCombinator combinator;

    void Start() {

    }

    void OnDrawGizmos() {
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

    void Update() {
        if (timerPressed && !pressed && currentReleaseTime > 0) {
            currentReleaseTime -= Time.deltaTime;
            if (currentReleaseTime <= 0) {
                currentReleaseTime = 0;
            }
        }

        timerMask.enabled = timerPressed;

        if (timerPressed) {
            if (currentReleaseTime > 0) {
                timerMask.alphaCutoff = 1 - (currentReleaseTime / releaseTime);
            } else {
                timerPressed = false;
                GetComponent<Animator>().Play("ButtonUp");
            }
        }

        // Billboard the fuck out of the timer sprite and mask
        timerMask.gameObject.transform.LookAt(Camera.main.transform);
        timerMask.gameObject.transform.Rotate(new Vector3(0, 180, 0));
        timerSprite.transform.LookAt(Camera.main.transform);
    }

    public void Press(bool press) {
        pressed = press;
        if (releaseTime <= 0) {
            currentReleaseTime = releaseTime;
            if (press) {
                GetComponent<Animator>().Play("ButtonDown");
            } else {
                GetComponent<Animator>().Play("ButtonUp");
            }
        } else {
            if (press == true) {
                if (!timerPressed) {
                    GetComponent<Animator>().Play("ButtonDown");
                }
                timerPressed = true;
                currentReleaseTime = releaseTime;
            }
        }
        
        // Activate like normally if this button isn't connected to a combinator (logic gate)
        if (combinator == null) {
            if (pressed) {
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
        } else {
            // No need to notify the combinator or anything -- it's always listening. <_<
        }
    }
}
