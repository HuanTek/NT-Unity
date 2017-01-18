/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch设备控制管理类

*********************************************************************************/
using UnityEngine;
using System.Collections;
using NT_CSDK_API;
using UnityEngine.UI;

public class NT_Controller_Manager : MonoBehaviour {

    //单例
    private static NT_Controller_Manager instance = null;

    /// <summary>
    /// 获取单例对象，场景中的NT_Controller_Manager组件必须在名为NT_Controller的gameobject上，为保持其唯一性，否则将会重新创建。
    /// </summary>
    /// <returns></returns>
    public static NT_Controller_Manager GetInstance() {
        //单例是否为空
        if (instance == null) {
            GameObject managerObj = GameObject.Find("NT_Controller");
            //挂靠的gameobject是否为空
            if (managerObj == null) {
                managerObj = new GameObject("NT_Controller");
            }

            instance = managerObj.GetComponent<NT_Controller_Manager>();
            if(instance == null)
            {
                instance = managerObj.AddComponent<NT_Controller_Manager>();
            }
            
        }

        return instance;
    }
    //是否永不删除
    public bool isDontDestroyOnLoad = false;
    //是否已经执行过sdk初始化
    public static bool IS_SDK_ALREADY_INITED = false;

    //连接状态
    private NT_CONN_ERR_CODE connectState;
    //是否已经初始化
    private bool isInit;
    //是否拥有左手
    public bool isHasLeftHand = true;
    //是否拥有右手
    public bool isHasRightHand = true;
    //NT_Hand左手实例
    public NT_Hand leftHand;
    //NT_Hand右手实例
    public NT_Hand rightHand;
    //左手锚点
    public GameObject leftAnchorPoint;
    //右手锚点
    public GameObject rightAnchorPoint;
    //左手数据
    private NT_DATA left_Data = new NT_DATA();
    //右手数据
    private NT_DATA right_Data = new NT_DATA();
    //vive控制器 左手transform
    public Transform leftController;
    //vive控制器 右手transform
    public Transform rightController;
    //左手套设备连接状态反馈结构体
    NT_STATE LeftGloveState = new NT_STATE();
    //右手套设备连接状态反馈结构体
    NT_STATE RightGloveState = new NT_STATE();
    //九轴四元数数据传输类
    NT_MemsData memsData = new NT_MemsData();
    //获取设备状态间隔时间
    float Timer=2f;
    
    /// <summary>
    /// 连接手套设备
    /// </summary>
    private void ConnectGlove()
    {
        Function.DebugLog("手套连接进入");
        connectState = NT_CSDK_Function.NT_ConnectGlove();
        switch (connectState)
        {
            case NT_CONN_ERR_CODE.NT_TWO_CONN_OK:
                Debug.Log("成功连接设备");
                break;
            case NT_CONN_ERR_CODE.NT_NO_BLUE_HOST:
                Debug.LogError("没有插入蓝牙设备");
                break;
            case NT_CONN_ERR_CODE.NT_ONE_HOST_AND_NO_CONN:
                Debug.Log("插入一个蓝牙设备并且连接不成功");
                break;
            case NT_CONN_ERR_CODE.NT_ONE_HOST_AND_CONN_LEFT:
                Debug.Log("插入一个蓝牙设备，左手连接成功");
                break;
            case NT_CONN_ERR_CODE.NT_ONE_HOST_AND_CONN_RIGHT:
                Debug.Log("插入一个蓝牙设备，右手连接成功");
                break;
            case NT_CONN_ERR_CODE.NT_TWO_HOST_AND_NO_CONN:
                Debug.LogError("插入两个蓝牙设备，并且两个设备同时连接不成功");
                break;
            case NT_CONN_ERR_CODE.NT_TWO_HOST_AND_CONN_LEFT:
                Debug.Log("插入两个蓝牙设备，并且只有左手连接成功");
                break;
            case NT_CONN_ERR_CODE.NT_TWO_HOST_AND_CONN_RIGHT:
                Debug.Log("插入两个蓝牙设备，并且只有右手连接成功");
                break;
        }
    }  
    /// <summary>
    /// 初始化方法，没有参数，生成默认手套
    /// </summary>
    public void Init() {
        //手套如果已经执行过初始化，就不能再执行第二次
        if (!IS_SDK_ALREADY_INITED)
        {
            //判断
            isInit = InitGlove.Init();
            if (!isInit)
            {
                return;
            }

            ConnectGlove();
            if (connectState == NT_CONN_ERR_CODE.NT_NO_BLUE_HOST)
            {
                Function.DebugLog("初始化失败");
                return;
            }
            IS_SDK_ALREADY_INITED = true;
        }
        

        if (isHasLeftHand && leftHand == null) {
            GameObject leftObj = Instantiate<GameObject>(Resources.Load("Left_Hand") as GameObject);
            leftHand = leftObj.GetComponent<NT_Hand>();
            leftHand.handId = 0;
            leftHand.InitHand();
            //leftHand.InitDefaultTouch();
        }
        if (isHasRightHand && rightHand == null)
        {
            GameObject rightObj = Instantiate<GameObject>(Resources.Load("Right_Hand") as GameObject);
            rightHand = rightObj.GetComponent<NT_Hand>();
            rightHand.handId = 1;
            rightHand.InitHand();
            //rightHand.InitDefaultTouch();
        }
        Invoke("SetMems", 1);
    }

    public void Init(NT_Hand left, NT_Hand right) {
        //手套如果已经执行过初始化，就不能再执行第二次
        if (!IS_SDK_ALREADY_INITED)
        {
            isInit = InitGlove.Init();
            ConnectGlove();
            if (connectState == NT_CONN_ERR_CODE.NT_NO_BLUE_HOST)
            {
                Function.DebugLog("初始化失败");
                return;
            }
        }

        leftHand = left;
        leftHand.handId = 0;

        rightHand = right;
        rightHand.handId = 1;

        Invoke("SetMems", 1);
    }
    //更新手部数据
    public void UpdateHand() {
       
        if (isHasLeftHand)
        {
            NT_CSDK_Function.NT_GetState(NT_HAND.NT_LEFT, ref LeftGloveState);
            if (LeftGloveState.bConnectState == -2)
                return;            
                NT_CSDK_Function.NT_GetData(NT_HAND.NT_LEFT, NT_TYPE.NT_QUATERNION_FLEX, ref left_Data);
            if (leftHand.isModelUpdatePos && isMemsAlreadySet) {              
                memsData.SetLeftMemsData(left_Data, leftAnchorPoint.transform);
                leftHand.SetFingerRotate(left_Data.fingers);
                leftHand.transform.position = leftAnchorPoint.transform.position;
                leftHand.transform.rotation = Quaternion.Euler(leftAnchorPoint.transform.rotation.eulerAngles + Vector3.forward * 180 + Vector3.up * 180);
            }
        }
        if (isHasRightHand)
        {
            NT_CSDK_Function.NT_GetState(NT_HAND.NT_RIGHT, ref RightGloveState);
            if (RightGloveState.bConnectState == -2)
                return;
            NT_CSDK_Function.NT_GetData(NT_HAND.NT_RIGHT, NT_TYPE.NT_QUATERNION_FLEX, ref right_Data);
            if (rightHand.isModelUpdatePos && isMemsAlreadySet)
            {
                memsData.SetRightMemsData(right_Data, rightAnchorPoint.transform);
                rightHand.SetFingerRotate(right_Data.fingers);
                rightHand.transform.position = rightAnchorPoint.transform.position;
                rightHand.transform.rotation = Quaternion.Euler(rightAnchorPoint.transform.rotation.eulerAngles + Vector3.forward * 180+Vector3.up*180);
            }
        }
        if(LeftGloveState.bConnectState==-3|| RightGloveState.bConnectState == -3)
        {
            Invoke("SetMems", 4);
        }
    }

    //根据id 获取手部数据， 0为左手 1为右手
    public NT_DATA GetHandDate(int handId) {
        if (handId == 0)
            return left_Data;
        else
            return right_Data;
    }

    //震动接口，手部id 0为左手 1为右手 2双手，force为强度 0-100
    public void SetHandVibration(int handId,uint strength, uint time) {
        NT_VBT_MOD modTemp = new NT_VBT_MOD();
        modTemp.eMotors = 7;
        modTemp.strength = strength;
        modTemp.time = time;
        NT_CSDK_Function.NT_SetVibration((NT_HAND)handId, modTemp);
    }

    public void ShakeHand(NT_HAND hand,NT_VBT_MOD mod)
    {
        NT_CSDK_Function.NT_SetVibration(hand, mod);
    }

    //重置手套
    public void ResetHand() {

    }

    void Start () {
        //ConnectGlove();
      
    }
	
	void Update () {
        if (!isInit || connectState == NT_CONN_ERR_CODE.NT_NO_BLUE_HOST)
        {
            return;
        }
       
        UpdateHand();

    }
    public void OnApplicationQuit()
    {
        //关闭设备串口
        InitGlove.AllExit();
        Debug.Log("关闭串口");
    }


    private bool isMemsAlreadySet = false;
    public void SetMems()
    {
        
        memsData.RefreshMEMS(this.left_Data,this.right_Data,leftController, rightController);
        leftAnchorPoint = new GameObject("leftAnchorPoint");
        leftAnchorPoint.transform.position = leftController.transform.position;
        leftAnchorPoint.transform.rotation = leftHand.transform.rotation;
        leftAnchorPoint.transform.SetParent(leftController.transform);

        rightAnchorPoint = new GameObject("rightAnchorPoint");
        rightAnchorPoint.transform.position = rightController.transform.position;
        rightAnchorPoint.transform.rotation = rightHand.transform.rotation;
        rightAnchorPoint.transform.SetParent(rightController.transform);

        isMemsAlreadySet = true;
    }
}
