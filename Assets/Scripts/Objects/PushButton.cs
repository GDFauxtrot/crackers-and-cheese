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

    void Start() {

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
        if (releaseTime == 0) {
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
        
    }
}
