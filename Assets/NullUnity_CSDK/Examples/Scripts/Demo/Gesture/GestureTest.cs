using UnityEngine;
using System.Collections;

public class GestureTest : MonoBehaviour {

    public GestureMain gestureMain;
    public NT_Gesture_Controller gestureController;
    public GameObject cubeBullet;
    public GameObject rayObj;

    NT_Hand hand;
    OutLineObject outline;

    RaycastHit hit;
    //Vector3 curAngle;
    Vector3 angleTemp;

    bool isInContinuousGesture = false;

    // Use this for initialization
    void Start () {
        Init();
        cubeBullet = Resources.Load("CubeBullet") as GameObject;
        rayObj = Instantiate(Resources.Load("Ray")) as GameObject;
        rayObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void Init() {
        gestureController = gameObject.GetComponent<NT_Gesture_Controller>();
        gestureController.OnContinuousGestureStart += GestureStartTest;
        gestureController.OnContinuousGestureEnd += GestureEndTest;

        gestureController.OnGestureStart += SingleGestureStartTest;
        gestureController.OnGestureStay += SingleGestureStayTest;
        gestureController.OnGestureEnd += SingleGestureEndTest;

        hand = GetComponent<NT_Hand>();
    }

    void SingleGestureStartTest(int gestureType) {
        if (!isInContinuousGesture)
        {
            if (gestureType == 10)
            {
                if (gestureMain.curSelectObj)
                {
                    //curAngle = gestureMain.curSelectObj.transform.rotation.eulerAngles;
                    angleTemp = hand.transform.rotation.eulerAngles;
                }
            }
            else if (gestureType == 6)
            {
                if (!rayObj.activeSelf)
                {
                    rayObj.SetActive(true);
                }
            }
        }
    }

   

    void SingleGestureStayTest(int gestureType)
    {
        if (!isInContinuousGesture)
        {
            if (gestureType == 6)
            {
                //if (!rayObj.activeSelf)
                //{
                //    rayObj.SetActive(true);
                //}
                Vector3 start = hand.indexTrans[0].position;
                bool ishit = Physics.Raycast(start, -gameObject.transform.forward, out hit, 1000);
                if (ishit)
                {
                    outline = hit.collider.gameObject.GetComponent<OutLineObject>();
                    if (outline)
                    {
                        outline.ShowOutLine();
                        NT_Controller_Manager.GetInstance().SetHandVibration(hand.handId, 100, 250);
                        if (gestureMain.curSelectObj && gestureMain.curSelectObj != outline.gameObject)
                        {
                            gestureMain.curSelectObj.GetComponent<OutLineObject>().HideOutLine();
                        }

                        gestureMain.curSelectObj = outline.gameObject;
                    }
                    float dis = Vector3.Distance(hit.collider.gameObject.transform.position, start);
                    rayObj.transform.localScale = new Vector3(1, 1, dis / 2f);
                    
                    rayObj.transform.position = start - gameObject.transform.forward * dis / 4f;
                    rayObj.transform.rotation = gameObject.transform.rotation;
                }
                else {
                    rayObj.transform.localScale = new Vector3(1, 1, 500f);
                    rayObj.transform.position = start - gameObject.transform.forward * 500f;
                    rayObj.transform.rotation = gameObject.transform.rotation;

                }
                
            }
            else if (gestureType == 10) {
                if (gestureMain.curSelectObj) {
                    gestureMain.curSelectObj.transform.rotation = Quaternion.Euler(gestureMain.curSelectObj.transform.rotation.eulerAngles + hand.transform.rotation.eulerAngles - angleTemp);
                    angleTemp = hand.transform.rotation.eulerAngles;
                    //gestureMain.curSelectObj.transform.rotation = Quaternion.Euler(hand.transform.rotation.eulerAngles + curAngle);
                }
            }
        }
    }

    void SingleGestureEndTest(int gestureType)
    {
        if (!isInContinuousGesture)
        {
            if (gestureType == 6)
            {
                rayObj.SetActive(false);
            }
        }
    }

    void GestureStartTest(string gestureName)
    {
        if (gestureName.Equals("shoot")) {
            InvokeRepeating("Shoot", 0, 0.2f);
        }
        isInContinuousGesture = true;
    }

    void GestureEndTest(string gestureName)
    {
        if (gestureName.Equals("shoot"))
        {
            CancelInvoke("Shoot");
        }
        isInContinuousGesture = false;
    }

    void Shoot() {
        GameObject temp = Instantiate(cubeBullet, gameObject.transform.position-gameObject.transform.forward * 0.3f, gameObject.transform.rotation) as GameObject;
    }

    
}
