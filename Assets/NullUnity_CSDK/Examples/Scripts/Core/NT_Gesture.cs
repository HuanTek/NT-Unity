/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch手势控制类

*********************************************************************************/
using UnityEngine;
using System.Collections;
using NT_Gest;
//手势类型
public enum NT_GestureType {
    Good = 0,
    Fist = 1,
    OK = 2,
    Nico = 3,
    Paper = 4,
    Point = 5,
    Catch = 6,
    Scale = 7,
    Gun = 8,
    Rotate = 9
}


public class NT_Gesture : MonoBehaviour {

    //单例
    private static NT_Gesture instance = null;

    /// <summary>
    /// 获取单例对象，场景中的NT_Gesture组件必须在名为NT_Controller的gameobject上，为保持其唯一性，否则将会重新创建。
    /// </summary>
    /// <returns></returns>
    public static NT_Gesture GetInstance()
    {
        //单例是否为空
        if (instance == null)
        {
            GameObject managerObj = GameObject.Find("NT_Controller");
            //挂靠的gameobject是否为空
            if (managerObj == null)
            {
                managerObj = new GameObject("NT_Controller");
            }

            instance = managerObj.GetComponent<NT_Gesture>();
            if (instance == null)
            {
                instance = managerObj.AddComponent<NT_Gesture>();
            }

        }

        return instance;
    }

    private NT_Gest_Handler LeftGestureHandler;
    private NT_Gest_Handler RightGestureHandler;

    //左手手势类型
    public int leftHandGestureType = -1;
    //右手手势类型
    public int rightHandGestureType = -1;

    public int leftHandGestureTypeTemp = -1;
    public int rightHandGestureTypeTemp = -1;

    //手势录入过滤时间间隔
    public float identificationDelay = 0.02f;
    private float leftHandLastUpdateTime = 0;
    private float rightHandLastUpdateTime = 0;

    //更新时间间隔
    public float during = 0.01f;
    //测试json数据
    public TextAsset jsonText;

    public delegate void UpdateGestureEventHandler(int handId, int gestureType);
    //进入手势
    public event UpdateGestureEventHandler OnGestureChange ;

    // Use this for initialization
    void Start () {
        LeftGestureHandler = new NT_Gest_Handler();
        RightGestureHandler = new NT_Gest_Handler();
        LoadHandModel();
        InvokeRepeating("GestureUpdate", 0, during);
    }
	
	// Update is called once per frame
	void Update () {
    }

    //重设手势检测时间间隔
    public void ResetGestureDuring(float time) {
        if (IsInvoking("GestureUpdate")){
            CancelInvoke("GestureUpdate");
            during = time;
            InvokeRepeating("GestureUpdate", 0, during);
        }
    }

    //更新手势状态
    public void GestureUpdate()
    {
        int leftHandValue = RecongLeftGest(new float[] { NT_Controller_Manager.GetInstance().GetHandDate(0).fingers.thumb,
            NT_Controller_Manager.GetInstance().GetHandDate(0).fingers.index,NT_Controller_Manager.GetInstance().GetHandDate(0).fingers.middle,
        NT_Controller_Manager.GetInstance().GetHandDate(0).fingers.ring,NT_Controller_Manager.GetInstance().GetHandDate(0).fingers.pinky}, 5);
        Function.DebugLog(leftHandValue);
        if (leftHandGestureType != leftHandValue) {
            if (leftHandGestureTypeTemp == leftHandValue && Time.time - leftHandLastUpdateTime >= identificationDelay)
            {
                leftHandGestureType = leftHandValue;
                Function.DebugLog("左手事件更新！");
                if (OnGestureChange != null)
                {
                    OnGestureChange(0, leftHandGestureType);
                }
            }
            else if (leftHandGestureTypeTemp != leftHandValue) {
                leftHandGestureTypeTemp = leftHandValue;
                leftHandLastUpdateTime = Time.time;
                //Function.DebugLog(leftHandGestureTypeTemp + ",time:" + leftHandLastUpdateTime);
            }
        }

        int rightHandvalue = RecongRightGesture(new float[] { NT_Controller_Manager.GetInstance().GetHandDate(1).fingers.thumb,
            NT_Controller_Manager.GetInstance().GetHandDate(1).fingers.index,NT_Controller_Manager.GetInstance().GetHandDate(1).fingers.middle,
        NT_Controller_Manager.GetInstance().GetHandDate(1).fingers.ring,NT_Controller_Manager.GetInstance().GetHandDate(1).fingers.pinky}, 5);
       // Function.DebugLog(rightHandvalue);
        if (rightHandGestureType != rightHandvalue)
        {
            if (rightHandGestureTypeTemp == rightHandvalue && Time.time - rightHandLastUpdateTime >= identificationDelay)
            {
                rightHandGestureType = rightHandvalue;
                if (OnGestureChange != null)
                {
                    OnGestureChange(1, rightHandGestureType);
                }
            }
            else if (rightHandGestureTypeTemp != rightHandvalue)
            {
                rightHandGestureTypeTemp = rightHandvalue;
                rightHandLastUpdateTime = Time.time;
            }
            
        }
    }

    /// <summary>
    /// 加载手势模型
    /// </summary>
    public void LoadHandModel()
    {
        string LeftModelPath = @"Model_L_1.3.dat";
        string RightModelPath = @"Model_R_1.3.dat";
        LeftGestureHandler.LoadModel(LeftModelPath);
        RightGestureHandler.LoadModel(RightModelPath);
    }
    /// <summary>
    /// 识别左手手势
    /// </summary>
    /// <param name="hand">弯曲传感器数据</param>
    /// <param name="size">长度值</param>
    /// <param name="error">偏差值 允许误差为：0.25</param>
    /// <returns></returns>
    public int RecongLeftGest(float[] hand,int size,double error = 0.2f)
    {
        /**
        返回值说明：
       -1:读取模型失败，或者传入数据有误；
        0:识别误差超过给定的允许误差， 拒绝识别
       其余:手势编号。          
        **/
        return LeftGestureHandler.Recognize(hand, size, error);        
    }
    /// <summary>
    /// 识别右手手势
    /// </summary>
    /// <param name="hand">弯曲传感器数据</param>
    /// <param name="size">长度值</param>
    /// <param name="error">偏差值 允许误差为：0.25</param>
    /// <returns></returns>
    public int RecongRightGesture(float[] hand, int size, double error = 0.2f)
    {
        /**
         返回值说明：
        -1:读取模型失败，或者传入数据有误；
         0:识别误差超过给定的允许误差， 拒绝识别
        其余:手势编号。          
        **/
        return RightGestureHandler.Recognize(hand, size, error);
    }
}
 
