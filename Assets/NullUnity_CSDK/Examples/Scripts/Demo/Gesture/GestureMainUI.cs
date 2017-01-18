using UnityEngine;
using System.Collections;

public class GestureMainUI : MonoBehaviour {

    public Transform Camera;

	// Use this for initialization
	void Start () {
        NT_Controller_Manager.GetInstance().Init();
        NT_Controller_Manager.GetInstance().leftHand.InitDefaultTouch(NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY, NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY);
        ((GestureTestUI)NT_Controller_Manager.GetInstance().leftHand.gameObject.AddComponent<GestureTestUI>()).gestureMain = this;
        ((GestureTestUI)NT_Controller_Manager.GetInstance().leftHand.gameObject.GetComponent<GestureTestUI>()).Camera = Camera;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
