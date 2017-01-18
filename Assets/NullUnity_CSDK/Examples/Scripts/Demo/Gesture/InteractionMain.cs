using UnityEngine;
using System.Collections;

public class InteractionMain : MonoBehaviour {

    public GameObject curSelectObj;
    public bool isScaleLeft = false;
    public bool isScaleRight = false;
    public float handDis = 0;
    public bool isScale = false;
    private float scaleTemp = 1;

    // Use this for initialization
    void Start () {
        NT_Controller_Manager.GetInstance().Init();
        //NT_Controller_Manager.GetInstance().leftHand.InitDefaultTouch(NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY, NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY);
        ((InteractionTest)NT_Controller_Manager.GetInstance().leftHand.gameObject.AddComponent<InteractionTest>()).gestureMain = this;


       // NT_Controller_Manager.GetInstance().rightHand.InitDefaultTouch(NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY, NT_TouchAttachType.CHILD_OF_CONTROLLER, NT_TouchType.BLOCKBODY);
        ((InteractionTest)NT_Controller_Manager.GetInstance().rightHand.gameObject.AddComponent<InteractionTest>()).gestureMain = this;
    }
	
	// Update is called once per frame
	void Update () {
        if (isScaleLeft && isScaleRight) {
            if (!isScale) {
                handDis = GetHandDis();
                isScale = true;
            }
            float scale = GetHandDis() / handDis;
            scaleTemp *= scale;
            if (scaleTemp > 2)
            {
                scaleTemp = 2;
            }
            else if (scaleTemp < 0.2f) {
                scaleTemp = 0.2f;
            }
            curSelectObj.transform.localScale = new Vector3(scaleTemp, scaleTemp, scaleTemp);
        }
	}

    public float GetHandDis() {
        return Vector3.Distance(NT_Controller_Manager.GetInstance().leftHand.gameObject.transform.position, 
            NT_Controller_Manager.GetInstance().rightHand.gameObject.transform.position);
    }
}
