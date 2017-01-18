/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： 受物理影响碰撞的力学关节物体

*********************************************************************************/
using UnityEngine;
using System.Collections;

public class NT_Touch_JointObj : MonoBehaviour {

    /// <summary>
    /// 碰撞挂靠本体物体
    /// </summary>
    public NT_Touch_BaseObj touchObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (!touchObj.IsTouchEnterNull() && touchObj.touchType != NT_TouchType.TRIGGERBODY) {
            touchObj.CallTouchEnter(collisionInfo.gameObject, touchObj);
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (!touchObj.IsTouchEndNull() && touchObj.touchType != NT_TouchType.TRIGGERBODY)
        {
            touchObj.CallTouchEnd(collisionInfo.gameObject, touchObj);
        }
    }

    void OnCollisionStay(Collision collisionInfo) {
        if (!touchObj.IsTouchStayNull() && touchObj.touchType != NT_TouchType.TRIGGERBODY)
        {
            touchObj.CallTouchStay(collisionInfo.gameObject, touchObj);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!touchObj.IsTouchEnterNull() && touchObj.touchType == NT_TouchType.TRIGGERBODY)
        {
            touchObj.CallTouchEnter(other.gameObject, touchObj);
        }
    }

    void OnTriggerExit(Collider other) {
        if (!touchObj.IsTouchEndNull() && touchObj.touchType == NT_TouchType.TRIGGERBODY)
        {
            touchObj.CallTouchEnd(other.gameObject, touchObj);
        }
    }

    void OnTriggerStay(Collider other) {
        if (!touchObj.IsTouchStayNull() && touchObj.touchType == NT_TouchType.TRIGGERBODY)
        {
            touchObj.CallTouchStay(other.gameObject, touchObj);
        }
    }

}
