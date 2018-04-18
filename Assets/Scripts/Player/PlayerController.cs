using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    bool grounded = false;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 350, 0));
        }


    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Ground")
            grounded = true;
    }

    void OnCollisionExit(Collision c)
    {
        if (c.gameObject.tag == "Ground")
            grounded = false;
    }
}
