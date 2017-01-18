using UnityEngine;
using System.Collections;

public class CubeBullet : MonoBehaviour {

    Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(-gameObject.transform.forward*250, ForceMode.Acceleration);
        Destroy(this.gameObject, 5);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
