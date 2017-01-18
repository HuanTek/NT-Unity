/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch指节碰撞类

*********************************************************************************/
using UnityEngine;
using System.Collections;
using System;

public class NT_Touch_Kunckle : NT_Touch_BaseObj
{
    //碰撞半径
    public float colliderRadius = 0.01f;
    //碰撞高度
    public float colliderHeight = 0.02f;
    //力学关节可动距离
    public float jointRange = 0.001f;
    //胶囊碰撞体
    public CapsuleCollider capsuleCollider;
    //刚体
    public Rigidbody rigidbody;

    //// Use this for initialization
    //void Start () {
    //       Init();

    //   }

    //   // Update is called once per frame
    //   void Update () {

    //}

    /// <summary>
    /// 初始化方法，根据挂靠方案、物理碰撞方案，生成碰撞体
    /// </summary>
    public override void Init()
    { 
        switch (attachTpye) {
            case NT_TouchAttachType.CHILD_OF_CONTROLLER:
                capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                if (physicMaterial != null)
                {
                    capsuleCollider.material = physicMaterial;
                }
                rigidbody = gameObject.AddComponent<Rigidbody>();
                break;

            case NT_TouchAttachType.JOINT:
                //capsuleCollider.
                GameObject go = new GameObject("JointCollider");
                go.transform.position = this.transform.position + centerCorrectPos;                                                                                              
                go.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + rotateCorrectDir);

                capsuleCollider = go.AddComponent<CapsuleCollider>();
                if (physicMaterial != null)
                {
                    capsuleCollider.material = physicMaterial;
                }
                NT_Touch_JointObj jointObj = go.AddComponent<NT_Touch_JointObj>();
                jointObj.touchObj = this;

                Rigidbody rigidSelf = gameObject.GetComponent<Rigidbody>();
                if(rigidSelf == null)
                {
                    rigidSelf = gameObject.AddComponent<Rigidbody>();
                }
                rigidSelf.constraints = RigidbodyConstraints.FreezeAll;

                rigidbody = go.GetComponent<Rigidbody>();
                if (rigidbody == null)
                {
                    rigidbody = go.AddComponent<Rigidbody>();
                }

                ConfigurableJoint joint = go.AddComponent<ConfigurableJoint>();
                joint.connectedBody = rigidSelf;
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;

                SoftJointLimit sjl = new SoftJointLimit();
                sjl.limit = 0.01f;
                //sjl.contactDistance = 0.004f;
                //sjl.bounciness = 1f;

                joint.linearLimit = sjl;

                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
                break;

            default:
                capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                rigidbody = gameObject.AddComponent<Rigidbody>();
                break;
        }

        capsuleCollider.direction = 0;
        capsuleCollider.radius = colliderRadius;
        capsuleCollider.height = colliderHeight;

        switch (touchType) {
            case NT_TouchType.PHYSICALBODY:
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.useGravity = false;
                break;

            case NT_TouchType.BLOCKBODY:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                rigidbody.useGravity = false;
                break;

            case NT_TouchType.TRIGGERBODY:
                rigidbody.isKinematic = true;
                capsuleCollider.isTrigger = true;
                rigidbody.useGravity = false;
                break;

            default:
                break;
        }
        
    }
}
