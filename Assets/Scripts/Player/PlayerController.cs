using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PlayerController : NetworkBehaviour {

    public GameObject grabPoint;

    [SyncVar]
    public GameObject grabbingObject;
    
    [SyncVar]
    public int playerNum;

    bool grounded = false;
    bool inLiquid;

    Rigidbody rb;

    public float runSpeed, jumpStrength, jumpLetGoVelocity;
    public float grabThrowStrength, grabThrowPlayerVelocityMultiplier;
    List<GameObject> grabbablesInRadius;

    public bool obeyCameraRange;

    public float respawnY;

    GameManager gameManager;

    void Start()
    {   

        grabbablesInRadius = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Debug.Log(gameManager.GetPlayerNumber(gameObject));
       if(playerNum==1)
       {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(86f / 255f, 21f / 255f, 21f / 255f, 1));
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_internalColor", new Color(1, 0, 0, 1));
            gameObject.layer = 15;// red collision

        }
        else{
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(37f / 255f, 37f / 255f, 188f / 255f, 1));
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_internalColor", new Color(0, 0, 1, 1));
            gameObject.gameObject.layer = 14; // blue collision

        }
        //Can set to whatever we want but allows us to do certain things to the local version of this object
      
    }
	
    [Command]
    void CmdGrabObject(GameObject go, bool grab) {
        if (grab) {
            go.transform.SetParent(grabPoint.transform);
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<GrabbableObject>().SetGrabbed(true);
        } else {
            go.transform.SetParent(null);
            go.GetComponent<GrabbableObject>().SetGrabbed(false);

            // Add force to the grabbable + current force from the player (yay math) * multiplier
            float playerForce = go.GetComponent<Rigidbody>().mass * (rb.velocity.magnitude / Time.fixedDeltaTime);
            go.GetComponent<Rigidbody>().AddForce(transform.forward * (playerForce + grabThrowStrength*grabThrowPlayerVelocityMultiplier));
        }
        RpcGrabObject(gameObject, go, grab);
    }

    [ClientRpc]
    void RpcGrabObject(GameObject owner, GameObject go, bool grab) {
        if (grab) {
            go.transform.SetParent(owner.GetComponent<PlayerController>().grabPoint.transform);
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<Rigidbody>().isKinematic = true;
            go.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncNone;
            go.layer = LayerMask.NameToLayer("GrabbedObject");
        } else {
            go.transform.SetParent(null);
            go.GetComponent<Rigidbody>().isKinematic = false;
            go.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            go.layer = go.GetComponent<GrabbableObject>().assignedLayer;
        }
    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

        // Jumping
        if (grounded && Input.GetButtonDown("Jump")) {
            rb.AddForce(new Vector3(0, jumpStrength, 0));
        }
        if (!grounded && Input.GetButtonUp("Jump") && rb.velocity.y > jumpLetGoVelocity) {
            rb.velocity = new Vector3(rb.velocity.x, jumpLetGoVelocity, rb.velocity.z);
        }

        // Grabbing
        if (Input.GetButtonDown("Grab")) {
            if (grabbingObject == null) {
                if (grabbablesInRadius.Count > 0) {
                    if (grabbablesInRadius.Count == 1) {
                        grabbingObject = grabbablesInRadius[0];
                    } else {
                        // Safe to sort like this, since the list shouldn't typically exceed more than like 3 objects
                        grabbablesInRadius = grabbablesInRadius.OrderBy(other => Vector3.Distance(transform.position, other.transform.position)).ToList();
                        // Closest object is at index 0
                        grabbingObject = grabbablesInRadius[0];
                    }
                    
                    grabbingObject.transform.SetParent(grabPoint.transform);
                    grabbingObject.transform.localPosition = Vector3.zero;
                    //grabbingObject.GetComponent<GrabbableObject>().SetGrabbed(true);
                    grabbingObject.GetComponent<Rigidbody>().isKinematic = true;
                    
                    grabbingObject.layer = LayerMask.NameToLayer("GrabbedObject");

                    // Send grab to server (and all clients)
                    CmdGrabObject(grabbingObject, true);
                }
            } else {
                grabbingObject.transform.SetParent(null);
                //grabbingObject.GetComponent<GrabbableObject>().SetGrabbed(false);
                grabbingObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbingObject.layer = grabbingObject.GetComponent<GrabbableObject>().assignedLayer;

                CmdGrabObject(grabbingObject, false);
                grabbingObject = null;
            }
        }

        // Keep player with camera
        if (obeyCameraRange) {
            transform.position = new Vector3(transform.position.x,transform.position.y,
                Mathf.Clamp(transform.position.z,
                Camera.main.transform.position.z + Camera.main.GetComponent<CameraFollowPlayers>().minZ-1,
                Camera.main.transform.position.z + Camera.main.GetComponent<CameraFollowPlayers>().maxZ+1));
        }

        // If player falls out of game world
        if (transform.position.y < respawnY) {
            int playerNum = gameManager.GetPlayerNumber(gameObject);
            
            float playerX = 0;
            if (playerNum == 1) {
                playerX = -5;
            } else if (playerNum == 2) {
                playerX = 5;
            }

            RaycastHit hit;
            Physics.Raycast(
                    new Vector3(Camera.main.transform.position.x + playerX, Camera.main.transform.position.y + 10,
                    Camera.main.transform.position.z + (Camera.main.GetComponent<CameraFollowPlayers>().minZ + Camera.main.GetComponent<CameraFollowPlayers>().maxZ)/2),
                    Vector3.down, out hit);

            if (hit.collider != null) {
                transform.position = new Vector3(hit.point.x, hit.point.y + 5, hit.point.z);
                rb.velocity = Vector3.zero;
            }
        }
    }

    void FixedUpdate() {
        if (!isLocalPlayer)
            return;

        float hSpeed = Input.GetAxisRaw("Horizontal") ;
        float vSpeed = Input.GetAxisRaw("Vertical") ;

        // Rotate to input angle (better than taking current velocity angle imo)
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            float desiredYRot = 0;
            Vector2 v = new Vector2(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            desiredYRot = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, desiredYRot, transform.rotation.eulerAngles.z));
        }

        if (inLiquid) {
            hSpeed /= 2;
            vSpeed /= 2;
        }
        Vector2 norm= new Vector2(hSpeed,vSpeed).normalized*runSpeed;
        rb.velocity = new Vector3(norm.x, rb.velocity.y, norm.y);
    }

    public override void OnStartLocalPlayer() {
       // Debug.Log(gameManager.name);

        

    }

    bool TagIsGrounded(string tag) {
        return tag == "Ground" || tag == "Button" || tag == "Platform" || tag == "Grabbable";
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

        if (c.gameObject.tag == "Grabbable" && c.gameObject.GetComponent<GrabbableObject>() != null) {
            grabbablesInRadius.Add(c.gameObject);
        }
    }

    void OnTriggerExit(Collider c) {
        if (c.gameObject.tag == "Liquid") {
            inLiquid = false;
        }

        if (c.gameObject.tag == "Grabbable" && c.gameObject.GetComponent<GrabbableObject>() != null) {
            grabbablesInRadius.Remove(c.gameObject);
        }
    }
}
