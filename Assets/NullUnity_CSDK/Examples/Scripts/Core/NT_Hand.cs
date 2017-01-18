/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch手部显示类

*********************************************************************************/
using UnityEngine;
using System.Collections;
using NT_CSDK_API;

public class NT_Hand : MonoBehaviour {

    public static readonly float[][] FINGER_ROTATE_ANGLE = new float[][] {
          new float[] { 0.4f,0.2f,0},new float[] {1,0.8f,0.8f},new float[] {1,0.8f,0.8f},new float[] {1,0.8f,0.8f},new float[] { 1, 0.8f, 0.8f }};


    //手部id
    public int handId;

    //是否显示模型外观
    public bool isShowModel = true;
    //手部模型
    public GameObject handModel;
    //是否跟新位置信息
    public bool isModelUpdatePos = true;

    //拇指指节 0~3分别从上到下，指头到手掌，第一到第三关节
    public Transform[] thumbTrans;
    //食指指节 0~3分别从上到下，指头到手掌，第一到第三关节
    public Transform[] indexTrans;
    //中指指节 0~3分别从上到下，指头到手掌，第一到第三关节
    public Transform[] middleTrans;
    //无名指指节 0~3分别从上到下，指头到手掌，第一到第三关节
    public Transform[] ringTrans;
    //尾指指节 0~3分别从上到下，指头到手掌，第一到第三关节
    public Transform[] pinkyTrans;
    //手掌部分
    public Transform palmTran;

    //拇指起始角度 0~3分别从上到下，指头到手掌，第一到第三关节
    private Vector3[] thumbOriginDir;
    //食指起始角度 0~3分别从上到下，指头到手掌，第一到第三关节
    private Vector3[] indexOriginDir;
    //中指起始角度 0~3分别从上到下，指头到手掌，第一到第三关节
    private Vector3[] middleOriginDir;
    //无名指起始角度 0~3分别从上到下，指头到手掌，第一到第三关节
    private Vector3[] ringOriginDir;
    //尾指起始角度 0~3分别从上到下，指头到手掌，第一到第三关节
    private Vector3[] pinkyOriginDir;

    //拇指碰撞修正角度 0~3分别从上到下，指头到手掌，第一到第三关节
    public Vector3[] thumbCenterPos = new Vector3[3];
    //食指碰撞修正角度 0~3分别从上到下，指头到手掌，第一到第三关节
    public Vector3[] indexCenterPos = new Vector3[3];
    //中指碰撞修正角度 0~3分别从上到下，指头到手掌，第一到第三关节
    public Vector3[] middleCenterPos = new Vector3[3];
    //无名指碰撞修正角度 0~3分别从上到下，指头到手掌，第一到第三关节
    public Vector3[] ringCenterPos = new Vector3[3];
    //尾指碰撞修正角度 0~3分别从上到下，指头到手掌，第一到第三关节
    public Vector3[] pinkyCenterPos = new Vector3[3];

    //动态生成碰撞体的角度修正值
    public Vector3[] thumbRotateCorrectDir = new Vector3[3];
    public Vector3[] indexRotateCorrectDir = new Vector3[3];
    public Vector3[] middleRotateCorrectDir = new Vector3[3];
    public Vector3[] ringRotateCorrectDir = new Vector3[3];
    public Vector3[] pinkyRotateCorrectDir = new Vector3[3];

    public Vector3 palmCenterPos = Vector3.zero;

    //手部物理材质
    public PhysicMaterial handPhyMaterial;

    public delegate void HandChangeEventHandler();
    //手部数据变更事件
    public event HandChangeEventHandler HandChange;

    /// <summary>
    /// 初始化手部，设置手指起始角度
    /// </summary>
    public void InitHand() {
        thumbOriginDir = new Vector3[thumbTrans.Length];
        for (int i = 0; i < thumbTrans.Length; i++)
        {
            thumbOriginDir[i] = thumbTrans[i].localRotation.eulerAngles;
        }

        indexOriginDir = new Vector3[indexTrans.Length];
        for (int i = 0; i < indexTrans.Length; i++)
        {
            indexOriginDir[i] = indexTrans[i].localRotation.eulerAngles;
        }

        middleOriginDir = new Vector3[middleTrans.Length];
        for (int i = 0; i < middleTrans.Length; i++)
        {
            middleOriginDir[i] = middleTrans[i].localRotation.eulerAngles;
        }

        ringOriginDir = new Vector3[ringTrans.Length];
        for (int i = 0; i < ringTrans.Length; i++)
        {
            ringOriginDir[i] = ringTrans[i].localRotation.eulerAngles;
        }

        pinkyOriginDir = new Vector3[pinkyTrans.Length];
        for (int i = 0; i < pinkyTrans.Length; i++)
        {
            pinkyOriginDir[i] = pinkyTrans[i].localRotation.eulerAngles;
        }
    }

    public void InitDefaultTouch(NT_TouchAttachType fingerAttachType, NT_TouchType fingerTouchType, NT_TouchAttachType palmAttachType, NT_TouchType palmTouchType) {
        for (int i = 0; i < thumbTrans.Length; i++)
        {
            NT_Touch_BaseObj touch = thumbTrans[i].gameObject.AddComponent<NT_Touch_Kunckle>();
            touch.rotateCorrectDir = thumbRotateCorrectDir[i];
            touch.physicMaterial = handPhyMaterial;
            touch.attachTpye = fingerAttachType;
            touch.touchType = fingerTouchType;
        }
        for (int i = 0; i < indexTrans.Length; i++)
        {
            NT_Touch_BaseObj touch = indexTrans[i].gameObject.AddComponent<NT_Touch_Kunckle>();
            touch.rotateCorrectDir = indexRotateCorrectDir[i];
            touch.physicMaterial = handPhyMaterial;
            touch.attachTpye = fingerAttachType;
            touch.touchType = fingerTouchType;
        }
        for (int i = 0; i < middleTrans.Length; i++)
        {
            NT_Touch_BaseObj touch = middleTrans[i].gameObject.AddComponent<NT_Touch_Kunckle>();
            touch.rotateCorrectDir = middleRotateCorrectDir[i];
            touch.physicMaterial = handPhyMaterial;
            touch.attachTpye = fingerAttachType;
            touch.touchType = fingerTouchType;
        }
        for (int i = 0; i < ringTrans.Length; i++)
        {
            NT_Touch_BaseObj touch = ringTrans[i].gameObject.AddComponent<NT_Touch_Kunckle>();
            touch.rotateCorrectDir = ringRotateCorrectDir[i];
            touch.physicMaterial = handPhyMaterial;
            touch.attachTpye = fingerAttachType;
            touch.touchType = fingerTouchType;
        }
        for (int i = 0; i < pinkyTrans.Length; i++)
        {
            NT_Touch_BaseObj touch = pinkyTrans[i].gameObject.AddComponent<NT_Touch_Kunckle>();
            touch.rotateCorrectDir = pinkyRotateCorrectDir[i];
            touch.physicMaterial = handPhyMaterial;
            touch.attachTpye = fingerAttachType;
            touch.touchType = fingerTouchType;
        }

        NT_Touch_Palm palmTouch = palmTran.gameObject.AddComponent<NT_Touch_Palm>();
        palmTouch.physicMaterial = handPhyMaterial;
        palmTouch.centerCorrectPos = palmCenterPos;
        palmTouch.attachTpye = palmAttachType;
        palmTouch.touchType = palmTouchType;
    }

    //设置手部可视化
    public void SetHandVisible(bool isVisible) {

    }
    float[,] coef5 = new float[15, 6];
    float[] max = new float[15];
    float[] min = new float[15];
    float[] Degree=new float[15];
    /// <summary>
    /// 设置手指旋转
    /// </summary>
    /// <param name="fingers"></param>
    public void SetFingerRotate(NT_FINGERS fingers) {

        for (int i = 0; i < FINGER_ROTATE_ANGLE[0].Length; i++)
        {
            thumbTrans[i].localRotation = Quaternion.Euler(thumbOriginDir[i].x, thumbOriginDir[i].y, thumbOriginDir[i].z + fingers.thumb * FINGER_ROTATE_ANGLE[0][i] * 100f);
        }
        for (int i = 0; i < FINGER_ROTATE_ANGLE[1].Length; i++)
        {
            indexTrans[i].localRotation = Quaternion.Euler(indexOriginDir[i].x, indexOriginDir[i].y, indexOriginDir[i].z + fingers.index * FINGER_ROTATE_ANGLE[1][i] * 100f);
        }
        for (int i = 0; i < FINGER_ROTATE_ANGLE[2].Length; i++)
        {
            middleTrans[i].localRotation = Quaternion.Euler(middleOriginDir[i].x, middleOriginDir[i].y, middleOriginDir[i].z + fingers.middle * FINGER_ROTATE_ANGLE[2][i] * 100f);
        }
        for (int i = 0; i < FINGER_ROTATE_ANGLE[3].Length; i++)
        {
            ringTrans[i].localRotation = Quaternion.Euler(ringOriginDir[i].x, ringOriginDir[i].y, ringOriginDir[i].z + fingers.ring * FINGER_ROTATE_ANGLE[3][i] * 100f);
        }
        for (int i = 0; i < FINGER_ROTATE_ANGLE[4].Length; i++)
        {
            pinkyTrans[i].localRotation = Quaternion.Euler(pinkyOriginDir[i].x, pinkyOriginDir[i].y, pinkyOriginDir[i].z + fingers.pinky * FINGER_ROTATE_ANGLE[4][i] * 100f);
        }        
    }

    // Use this for initialization
    void Start () {
        //InitDefaultTouch();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
