using UnityEngine;
using System.Collections;

public class InteractionTest : MonoBehaviour {

    public InteractionMain gestureMain;
    public NT_Gesture_Controller gestureController;
    public GameObject rayObj;

    NT_Hand hand;
    OutLineObject outline;

    RaycastHit hit;

    bool isInContinuousGesture = false;

    // Use this for initialization
    void Start () {
        Init();
        rayObj = Instantiate(Resources.Load("Ray")) as GameObject;
        rayObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void Init()
    {
        gestureController = gameObject.GetComponent<NT_Gesture_Controller>();
        gestureController.OnContinuousGestureStart += GestureStartTest;
        gestureController.OnContinuousGestureEnd += GestureEndTest;

        gestureController.OnGestureStart += SingleGestureStartTest;
        gestureController.OnGestureStay += SingleGestureStayTest;
        gestureController.OnGestureEnd += SingleGestureEndTest;

        hand = GetComponent<NT_Hand>();
    }

    void SingleGestureStartTest(int gestureType)
    {
        if (!isInContinuousGesture)
        {
            if (gestureType == 6)
            {
                if (!rayObj.activeSelf)
                {
                    rayObj.SetActive(true);
                }
            }
            else if (gestureType == 8)
            {
                if (hand.handId == 0)
                {
                    gestureMain.isScaleLeft = true;
                }
                else
                {
                    gestureMain.isScaleRight = true;
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
                Vector3 start = hand.indexTrans[0].position - gameObject.transform.forward * 0.2f;
                bool ishit = Physics.Raycast(start, -gameObject.transform.forward, out hit, 1000);
                if (ishit)
                {
                    outline = hit.collider.gameObject.GetComponent<OutLineObject>();
                    if (outline)
                    {
                        outline.ShowOutLine();
                       // NT_Controller_Manager.GetInstance().SetHandVibration(hand.handId, 100, 250);
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
                else
                {
                    rayObj.transform.localScale = new Vector3(1, 1, 500f);
                    rayObj.transform.position = (start - gameObject.transform.forward * 1000)/2f;
                    rayObj.transform.rotation = gameObject.transform.rotation;

                }

            }
            else if (gestureType == 7)
            {
                if (gestureMain.curSelectObj)
                {
                    //gestureMain.curSelectObj.GetComponent<Rigidbody>().AddExplosionForce(-10, hand.transform.position, 1000);
                    if (gestureMain.curSelectObj.GetComponent<Rigidbody>())
                    {
                        gestureMain.curSelectObj.GetComponent<Rigidbody>().useGravity = false;
                    }
                    gestureMain.curSelectObj.transform.position = Vector3.Lerp(gestureMain.curSelectObj.transform.position, hand.transform.position, 0.05f);
                    gestureMain.curSelectObj.transform.rotation = hand.transform.rotation;
                }
            }
            //else if (gestureType == 8)
            //{
            //    if (hand.handId == 0)
            //    {
            //        gestureMain.isScaleLeft = true;
            //    }
            //    else {
            //        gestureMain.isScaleRight = true;
            //    }
            //}
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
            else if (gestureType == 7)
            {
                if (gestureMain.curSelectObj)
                {
                    //gestureMain.curSelectObj.GetComponent<Rigidbody>().AddExplosionForce(-10, hand.transform.position, 1000);
                    if (gestureMain.curSelectObj.GetComponent<Rigidbody>())
                    {
                        gestureMain.curSelectObj.GetComponent<Rigidbody>().useGravity = true;
                    }
                }
            }
            else if (gestureType == 8)
            {
                if (hand.handId == 0)
                {
                    gestureMain.isScaleLeft = false;
                }
                else
                {
                    gestureMain.isScaleRight = false;
                }
                gestureMain.isScale = false;
            }
        }
    }

    void GestureStartTest(string gestureName)
    {
        isInContinuousGesture = true;
    }

    void GestureEndTest(string gestureName)
    {
        isInContinuousGesture = false;
    }
}
