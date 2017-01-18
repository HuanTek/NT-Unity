/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch手掌碰撞类

*********************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class NT_Touch_Palm : NT_Touch_BaseObj
{

    public BoxCollider boxCollider;
    public Rigidbody rigidbody;
  
    //public Vector3 center = new Vector3(0.08f, 0, 0);
    public Vector3 center = new Vector3(-0.05f, 0, 0);
    public Vector3 size = new Vector3(0.06f,0.02f,0.06f);

    public override void Init()
    {

        switch (attachTpye)
        {
            case NT_TouchAttachType.CHILD_OF_CONTROLLER:
                boxCollider = gameObject.AddComponent<BoxCollider>();
                if (physicMaterial != null)
                {
                    boxCollider.material = physicMaterial;
                }
                rigidbody = gameObject.AddComponent<Rigidbody>();
                break;

            case NT_TouchAttachType.JOINT:
                GameObject go = new GameObject("PalmJointCollider");
                go.transform.position = this.transform.position + centerCorrectPos;
                go.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + rotateCorrectDir);

                boxCollider = go.AddComponent<BoxCollider>();
                if (physicMaterial != null)
                {
                    boxCollider.material = physicMaterial;
                }
                NT_Touch_JointObj jointObj = go.AddComponent<NT_Touch_JointObj>();
                jointObj.touchObj = this;

                Rigidbody rigidSelf = gameObject.AddComponent<Rigidbody>();
                rigidSelf.constraints = RigidbodyConstraints.FreezeAll;

                rigidbody = go.AddComponent<Rigidbody>();


                ConfigurableJoint joint = go.AddComponent<ConfigurableJoint>();
                joint.connectedBody = rigidSelf;
                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;

                SoftJointLimit sjl = new SoftJointLimit();
                sjl.limit = 0.001f;

                joint.linearLimit = sjl;

                //SoftJointLimitSpring sjls = new SoftJointLimitSpring();
                //sjls.spring = 10f;

                //sjl.bounciness = 100000;

                //joint.linearLimitSpring = sjls;

                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
                break;

            default:
                boxCollider = gameObject.AddComponent<BoxCollider>();
                rigidbody = gameObject.AddComponent<Rigidbody>();
                break;
        }

        //capsuleCollider.direction = 0;
        //capsuleCollider.radius = colliderRadius;
        //capsuleCollider.height = colliderHeight;
        boxCollider.size = size;
        boxCollider.center = center;

        switch (touchType)
        {
            case NT_TouchType.PHYSICALBODY:
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.useGravity = false;
                break;

            case NT_TouchType.BLOCKBODY:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                break;

            case NT_TouchType.TRIGGERBODY:
                rigidbody.isKinematic = true;
                boxCollider.isTrigger = true;
                break;

            default:
                break;
        }
        //boxCollider.center
    }
}
