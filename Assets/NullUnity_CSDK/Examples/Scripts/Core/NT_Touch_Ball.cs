/****************************************
 * ****************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch圆球碰撞类

*********************************************************************************/
using UnityEngine;
using System.Collections;

public class NT_Touch_Ball : NT_Touch_BaseObj
{

    public float colliderRadius = 0f;//碰撞半径

    public SphereCollider sphereCollider;
    public Rigidbody rigidbody;

    //   // Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void Init()
    {
      

        switch (attachTpye)
        {
            case NT_TouchAttachType.CHILD_OF_CONTROLLER:
                sphereCollider = gameObject.AddComponent<SphereCollider>();
                if (physicMaterial != null)
                {
                    sphereCollider.material = physicMaterial;
                }

                rigidbody = gameObject.AddComponent<Rigidbody>();
                break;

            case NT_TouchAttachType.JOINT:
                //capsuleCollider.
                GameObject go = new GameObject("JointCollider");
                go.transform.position = this.transform.position + centerCorrectPos;
                go.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + rotateCorrectDir);

                sphereCollider = go.AddComponent<SphereCollider>();
                if (physicMaterial != null)
                {
                    sphereCollider.material = physicMaterial;
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
                sphereCollider = gameObject.AddComponent<SphereCollider>();
                rigidbody = gameObject.AddComponent<Rigidbody>();
                break;
        }
        
        sphereCollider.radius = colliderRadius;

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
                sphereCollider.isTrigger = true;
                break;

            default:
                break;
        }

    }
}
