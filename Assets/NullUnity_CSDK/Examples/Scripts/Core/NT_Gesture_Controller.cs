/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch手势控制手部类

*********************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NT_Hand))]
public class NT_Gesture_Controller : MonoBehaviour {

    /// <summary>
    /// 手势控制类
    /// </summary>
    public NT_Gesture gesture;

    //当前手部
    public NT_Hand curHand;
    

    //上一次手势类型
    public int preGestureType = -1;
    //当前手势类型
    public int curGestureType = -1;

    //是否在连续状态中
    public bool isReadyForContinuous = false;
    //当前连续手势标记长度
    public int curContinuousCount;
    //最后更新时间
    private float lastUpdateTime = 0;

    //连续手势配置文件
    private List<NT_Gesture_ContinuousGestureConfig> continuousGestureList = new List<NT_Gesture_ContinuousGestureConfig>();
    private List<NT_Gesture_ContinuousGestureConfig> continuousGestureOnReady = new List<NT_Gesture_ContinuousGestureConfig>();
    private List<NT_Gesture_ContinuousGestureConfig> continuousGestureForDelete = new List<NT_Gesture_ContinuousGestureConfig>();

    //当前执行手势配置
    private NT_Gesture_ContinuousGestureConfig actionContinuousGesture;

    //初始化手势配置信息
    public void InitGestureConfig()
    {
        if (NT_Gesture.GetInstance().jsonText){
            NT_Gesture_ContinuousGestureConfigList gestures = JsonUtility.FromJson<NT_Gesture_ContinuousGestureConfigList>(NT_Gesture.GetInstance().jsonText.ToString());
            continuousGestureList = new List<NT_Gesture_ContinuousGestureConfig>(gestures.configs);
        }
       
    }

    public delegate void GestureEventHandler(int gestureType);
    //进入手势
    public event GestureEventHandler OnGestureStart;
    //维持手势
    public event GestureEventHandler OnGestureStay;
    //结束手势
    public event GestureEventHandler OnGestureEnd;
    public delegate void ContinuousGestureEventHandler(string gestureName);
    //进入连续手势
    public event ContinuousGestureEventHandler OnContinuousGestureStart;
    public event ContinuousGestureEventHandler OnContinuousGestureStay;
    public event ContinuousGestureEventHandler OnContinuousGestureEnd;

    /// <summary>
    /// 手势变化检测方法
    /// </summary>
    /// <param name="handId"></param>
    /// <param name="GestureType"></param>
    private void OnGestureTypeChange(int handId, int gestureType) {
        //if (handId != curHand.handId || gestureType == 0) {
        if (handId != curHand.handId)
        {
            return;
        }

        Function.DebugLog("hand gesture change");

        preGestureType = curGestureType;
        curGestureType = gestureType;

        if (!isReadyForContinuous)
        {
            if (actionContinuousGesture != null)
            {
                OnContinuousGestureEnd(actionContinuousGesture.continuousGesturesName);
                actionContinuousGesture = null;
            }

            for (int i = 0; i < continuousGestureList.Count; i++)
            {
                if (continuousGestureList[i].startGesture == curGestureType)
                {
                    Function.DebugLog("进入连续状态_" + curGestureType);
                    continuousGestureOnReady.Add(continuousGestureList[i]);
                }
            }

            if (continuousGestureOnReady.Count > 0)
            {
                curContinuousCount = 0;
                isReadyForContinuous = true;
                lastUpdateTime = Time.time;
            }
        }
        else {
            curContinuousCount++;
            for (int i = 0; i < continuousGestureOnReady.Count; i++)
            {
                if (continuousGestureOnReady[i].gestureList[curContinuousCount] == curGestureType && continuousGestureOnReady[i].totalCount - 1 == curContinuousCount) {
                    actionContinuousGesture = continuousGestureOnReady[i];
                }
                else if (continuousGestureOnReady[i].gestureList[curContinuousCount] != curGestureType) {
                    continuousGestureForDelete.Add(continuousGestureOnReady[i]);
                }
            }

            if (actionContinuousGesture != null)
            {
                if (OnContinuousGestureStart != null) {
                    OnContinuousGestureStart(actionContinuousGesture.continuousGesturesName);
                }
                
                //actionContinuousGesture = null;
                continuousGestureOnReady.Clear();
                continuousGestureForDelete.Clear();
                isReadyForContinuous = false;
                //Function.DebugLog("shoot！");
            }
            else {
                for (int i = 0; i < continuousGestureForDelete.Count; i++)
                {
                    continuousGestureOnReady.Remove(continuousGestureForDelete[i]);
                }
                continuousGestureForDelete.Clear();

                

                if (continuousGestureOnReady.Count <= 0) {
                    isReadyForContinuous = false;
                }
                lastUpdateTime = Time.time;
            }
        }

        if(OnGestureEnd != null)
        {
            OnGestureEnd(preGestureType);
        }
        if (OnGestureStart != null)
        {
            OnGestureStart(curGestureType);
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        curHand = gameObject.GetComponent<NT_Hand>();
        InitGestureConfig();
        NT_Gesture.GetInstance().OnGestureChange += OnGestureTypeChange;
        //OnContinuousGestureStart += TestConGestures;
    }

    /// <summary>
    /// 删除超时的手势
    /// </summary>
    private void DeleteOverTimeGestures() {
        if (isReadyForContinuous)
        {
            for (int i = 0; i < continuousGestureOnReady.Count; i++)
            {
                if (continuousGestureOnReady[i].holdTime != -1 && Time.time - lastUpdateTime > continuousGestureOnReady[i].holdTime) {
                    continuousGestureForDelete.Add(continuousGestureOnReady[i]);
                }
            }

            for (int i = 0; i < continuousGestureForDelete.Count; i++)
            {
                continuousGestureOnReady.Remove(continuousGestureForDelete[i]);
            }
            continuousGestureForDelete.Clear();

            if (continuousGestureOnReady.Count <= 0)
            {
                isReadyForContinuous = false;
            }
        }
    }

    // Use this for initialization
    void Start () {
        Init();

    }
	
	// Update is called once per frame
	void Update () {
        if (OnGestureStay != null) {
            OnGestureStay(curGestureType);
        }

        if (actionContinuousGesture != null && OnContinuousGestureStay != null)
        {
            OnContinuousGestureStay(actionContinuousGesture.continuousGesturesName);
        }
    }

    void FixedUpdate() {
        DeleteOverTimeGestures();
    }

    void OnDestroy() {
        NT_Gesture.GetInstance().OnGestureChange -= OnGestureTypeChange;
    }

    void TestConGestures(string gestureName) {
        Function.DebugLog(gesture);
    }
}
