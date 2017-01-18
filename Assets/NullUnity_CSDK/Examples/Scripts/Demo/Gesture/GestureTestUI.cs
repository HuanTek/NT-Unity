using UnityEngine;
using System.Collections;

public class GestureTestUI : MonoBehaviour {

    public GestureMainUI gestureMain;
    public NT_Gesture_Controller gestureController;
    public GameObject current;
    GameObject UI;
    NT_Hand hand;
    public Transform LeftController;
    public Transform Camera;
    bool isInContinuousGesture = false;
    Vector3 offset;
    public float smothing = 5f;
    // Use this for initialization
    void Start () {
        Init();
        current = Resources.Load("GreenTurtle") as GameObject;       
        InstantiateUI();
        offset = UI.transform.position - Camera.position;
    }
	
	// Update is called once per frame

    
    void Init() {
        gestureController = gameObject.GetComponent<NT_Gesture_Controller>();
        gestureController.OnGestureStart += SingleGestureStartTest;
        hand = GetComponent<NT_Hand>();
    }

    void SingleGestureStartTest(int gestureType) {
        if (!isInContinuousGesture)
        {
            if (gestureType == 2)
            {           
                DoDisappear();
            }
            else if(gestureType==4)
            {
                DoAppear();
            }
        }
    }
     public void DoAppear()
    {
        if (UI)
        {
            UI.GetComponent<InterfaceAnimManager>().startAppear();
            UI.SetActive(true);
        }
     
    }
    public void DoDisappear()
    {       
        if (UI)
        {
            UI.GetComponent<InterfaceAnimManager>().startDisappear();
        }  
    }
    void InstantiateUI()
    {
        UI = Instantiate(current.gameObject, Camera.position ,Camera.rotation,Camera) as GameObject ;
        UI.transform.localPosition = UI.transform.localPosition + Vector3.forward * 0.5f;
    }

}
