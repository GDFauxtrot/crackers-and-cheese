using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public GameObject grabPoint;

    bool grounded = false;
    bool inLiquid;

    Rigidbody rb;

    public float runSpeed, jumpStrength;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        //This stops the update if it's not happening to the local system's player object
        if (!isLocalPlayer)
            return;

        float hSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * runSpeed;
        float vSpeed = Input.GetAxis("Vertical") * Time.deltaTime * runSpeed;

        if (inLiquid) {
            hSpeed /= 2;
            vSpeed /= 2;
        }

        rb.velocity = new Vector3(hSpeed, rb.velocity.y, vSpeed);

        if(grounded && Input.GetButtonDown("Jump")) {
            rb.AddForce(new Vector3(0, jumpStrength, 0));
        }
    }

    public override void OnStartLocalPlayer()
    {
        //Can set to whatever we want but allows us to do certain things to the local version of this object
        GetComponent<MeshRenderer>().material.color = Color.blue;
        //GameObject.Find("NetworkUIPanel").GetComponent<NetworkUIManager>().lanConnectingPanel.SetActive(false);
        //GameObject.Find("NetworkUIPanel").GetComponent<NetworkUIManager>().netConnectingPanel.SetActive(false);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Ground")
            grounded = true;
        else if (c.gameObject.tag == "Platform") {
            grounded = true;
            transform.parent = c.transform;
        }
    }

    void OnCollisionExit(Collision c)
    {
        if (c.gameObject.tag == "Ground")
            grounded = false;
        else if (c.gameObject.tag == "Platform") {
            grounded = false;
            transform.parent = null;
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Liquid") {
            inLiquid = true;
        }
    }

    void OnTriggerExit(Collider c) {
        if (c.gameObject.tag == "Liquid") {
            inLiquid = false;
        }
    }
}
