using UnityEngine;
using System.Collections;

public class GravityZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Box")) {
            Rigidbody rigidBody = other.gameObject.GetComponent<Rigidbody>();
            if (rigidBody.useGravity) {
                rigidBody.useGravity = false;
                rigidBody.velocity = rigidBody.velocity/400f;
            }
        }
    }
}
