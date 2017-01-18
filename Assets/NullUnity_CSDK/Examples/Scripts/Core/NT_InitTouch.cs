using UnityEngine;
using System.Collections;

public class NT_InitTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NT_Controller_Manager.GetInstance().Init();
        NT_Controller_Manager.GetInstance().leftHand.InitDefaultTouch(NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY, NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY);
        NT_Controller_Manager.GetInstance().rightHand.InitDefaultTouch(NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY, NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
