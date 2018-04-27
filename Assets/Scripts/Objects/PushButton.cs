using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour {

    public GameObject buttonBase, buttonTop;

    public bool pressed;

    public void Press(bool press) {
        pressed = press;

        if (pressed) {
            GetComponent<Animator>().Play("ButtonDown");
        } else {
            GetComponent<Animator>().Play("ButtonUp");
        }
    }
}
