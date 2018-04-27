using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressChild : MonoBehaviour {

    public PushButton dad;

    void OnTriggerEnter(Collider other) {
        dad.Press(true);
    }

    void OnTriggerExit(Collider other) {
        dad.Press(false);
    }
}
