using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeToQuit : MonoBehaviour {

    void Start () {
        
    }
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
