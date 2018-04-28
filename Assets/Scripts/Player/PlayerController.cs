using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public GameObject grabPoint;

    bool grounded = false;
    bool inLiquid;

    Rigidbody rb;

    public float runSpeed, jumpStrength, jumpLetGoVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

        if (grounded && Input.GetButtonDown("Jump")) {
            rb.AddForce(new Vector3(0, jumpStrength, 0));
        }
        if (!grounded && Input.GetButtonUp("Jump") && rb.velocity.y > jumpLetGoVelocity) {
            rb.velocity = new Vector3(rb.velocity.x, jumpLetGoVelocity, rb.velocity.z);
        }
    }

    void FixedUpdate() {
        if (!isLocalPlayer)
            return;

        float hSpeed = Input.GetAxis("Horizontal") * runSpeed;
        float vSpeed = Input.GetAxis("Vertical") * runSpeed;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            float desiredYRot = 0;
            Vector2 v = new Vector2(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            desiredYRot = Mathf.Atan2(v.y, v.x)*Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, desiredYRot, transform.rotation.eulerAngles.z));
        }

        if (inLiquid) {
            hSpeed /= 2;
            vSpeed /= 2;
        }

        rb.velocity = new Vector3(hSpeed, rb.velocity.y, vSpeed);
    }

    public override void OnStartLocalPlayer() {
        //Can set to whatever we want but allows us to do certain things to the local version of this object
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    bool TagIsGrounded(string tag) {
        return tag == "Ground" || tag == "Button" || tag == "Platform";
    }

    void OnCollisionStay(Collision c) {
        if (TagIsGrounded(c.gameObject.tag))
            grounded = true;

        if (c.gameObject.tag == "Platform")
            transform.parent = c.transform;
    }

    void OnCollisionExit(Collision c) {
        if (TagIsGrounded(c.gameObject.tag))
            grounded = false;

        if (c.gameObject.tag == "Platform")
            transform.parent = null;
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
