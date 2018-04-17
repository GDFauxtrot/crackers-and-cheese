using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float speedX, speedY;

    public override void OnStartLocalPlayer() {
        GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

    void Update () {
        if (!isLocalPlayer)
            return;

        transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * speedX, 0);
        transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * speedY);
    }
}
