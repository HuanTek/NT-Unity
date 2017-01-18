/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch碰撞基础类

*********************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// 碰撞的物理类型
/// </summary>
public enum NT_TouchType {
    PHYSICALBODY = 0,//受物理影响碰撞体
    BLOCKBODY = 1,//不受物理影响碰撞体
    TRIGGERBODY = 2//无物理碰撞检测区
}

/// <summary>
/// 碰撞体挂靠类型
/// </summary>
public enum NT_TouchAttachType {
    JOINT = 0,
    TRACK_OBJECT = 1,
    CHILD_OF_CONTROLLER = 2,
    CLIMBALE = 3
}


/// <summary>
/// 
/// </summary>
public class NT_Touch_BaseObj : MonoBehaviour {

    //碰撞的物理类型
    public NT_TouchType touchType;
    //可碰撞的层次
    public LayerMask touchLayers;
    //当前碰撞的物体
    public GameObject curToucedhObj;
    //修正角度
    public Vector3 rotateCorrectDir;
    //修正中心位置
    public Vector3 centerCorrectPos;
    //碰撞体挂靠类型
    public NT_TouchAttachType attachTpye;
    //表面物理材质
    public PhysicMaterial physicMaterial;

    public delegate void TouchEventHandler(GameObject gameObject, NT_Touch_BaseObj touchBaseObj);
    //碰撞进入事件
    public event TouchEventHandler OnTouchEnter;
    //碰撞持续事件
    public event TouchEventHandler OnTouchStay;
    //碰撞离开事件
    public event TouchEventHandler OnTouchEnd;

    /// <summary>
    /// 判断碰撞进入事件是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsTouchEnterNull() {
        return OnTouchEnter == null;
    }

    /// <summary>
    /// 判断碰撞持续事件是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsTouchStayNull()
    {
        return OnTouchStay == null;
    }

    /// <summary>
    /// 判断碰撞离开事件是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsTouchEndNull()
    {
        return OnTouchEnd == null;
    }

    /// <summary>
    /// 调用碰撞进入事件
    /// </summary>
    /// <param name="gameObject"></param>
    public void CallTouchEnter(GameObject gameObject, NT_Touch_BaseObj touchBaseObj) {
        OnTouchEnter(gameObject,this);
    }

    /// <summary>
    /// 调用碰撞持续事件
    /// </summary>
    /// <param name="gameObject"></param>
    public void CallTouchStay(GameObject gameObject, NT_Touch_BaseObj touchBaseObj)
    {
        OnTouchStay(gameObject, this);
    }

    /// <summary>
    /// 调用碰撞离开事件
    /// </summary>
    /// <param name="gameObject"></param>
    public void CallTouchEnd(GameObject gameObject, NT_Touch_BaseObj touchBaseObj)
    {
        OnTouchEnd(gameObject, this);
    }

    public virtual void Init() {
        Function.DebugLog("its inited");
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (!IsTouchEnterNull() && touchType != NT_TouchType.TRIGGERBODY)
        {
            CallTouchEnter(collisionInfo.gameObject, this);
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (!IsTouchEndNull() && touchType != NT_TouchType.TRIGGERBODY)
        {
            CallTouchEnd(collisionInfo.gameObject, this);
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (!IsTouchStayNull() && touchType != NT_TouchType.TRIGGERBODY)
        {
            CallTouchStay(collisionInfo.gameObject, this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsTouchEnterNull() && touchType == NT_TouchType.TRIGGERBODY)
        {
            CallTouchEnter(other.gameObject, this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsTouchEndNull() && touchType == NT_TouchType.TRIGGERBODY)
        {
            CallTouchEnd(other.gameObject, this);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!IsTouchStayNull() && touchType == NT_TouchType.TRIGGERBODY)
        {
            CallTouchStay(other.gameObject, this);
        }
    }
}
