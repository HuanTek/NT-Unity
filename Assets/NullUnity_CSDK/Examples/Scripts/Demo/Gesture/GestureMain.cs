using UnityEngine;
using System.Collections;

public class GestureMain : MonoBehaviour {

    public GameObject curSelectObj;

	// Use this for initialization
	void Start () {
        NT_Controller_Manager.GetInstance().Init();
        NT_Controller_Manager.GetInstance().leftHand.InitDefaultTouch(NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY, NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY);
        ((GestureTest)NT_Controller_Manager.GetInstance().leftHand.gameObject.AddComponent<GestureTest>()).gestureMain = this;


        NT_Controller_Manager.GetInstance().rightHand.InitDefaultTouch(NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY, NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY);
        ((GestureTest)NT_Controller_Manager.GetInstance().rightHand.gameObject.AddComponent<GestureTest>()).gestureMain = this;

    }

    // Update is called once per frame
    void Update () {
	
	}
}
