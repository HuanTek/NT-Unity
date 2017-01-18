/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 林晓峰

** 最终修改时间：2016-12-6

** 功能描述： 九轴四元数数据的重置和赋值

*********************************************************************************/
using UnityEngine;
using System.Collections;
using NT_CSDK_API;
public class NT_MemsData
{
    //传感器校准对象
    NT_CalibrateMEMS CalibrateMEMS;
    //左手控制器四元数
    Quaternion leftcontrollerQuaternion;
    //右手控制器四元数
    Quaternion rightcontrollerQuaternion;
    //左手控器制
    Transform LeftController;
    //右手控制器
    Transform RightController;
    public NT_MemsData()
    {
        //构造函数实例化
        CalibrateMEMS = new NT_CalibrateMEMS();
    }
    /// <summary>
    /// 设置左手九轴传感器四元数到模型上
    /// </summary>
    /// <param name="leftData">左手数据</param>
    /// <param name="hand">左手模型对象</param>
    public void SetLeftMemsData(NT_DATA LeftData,Transform hand)  
    {       
        //左手四元数
        Quaternion Left = Quaternion.identity;
        //获取四元数校正值
        Quaternion leftMems = CalibrateMEMS.LeftMEMSQuaternion(LeftData);
        Left.x = leftMems.x;
        Left.y =- leftMems.z;
        Left.z = leftMems.y;
        Left.w = leftMems.w;
        if (hand != null&&LeftController!=null)
        {          
            //赋值四元数到手部模型
            hand.localRotation = Left;            
            hand.rotation = leftcontrollerQuaternion * Quaternion.Inverse(LeftController.transform.rotation) * hand.rotation;
        }
    }
    /// <summary>
    /// 设置右手九轴传感器四元数到模型上
    /// </summary>
    /// <param name="rightData">右手数据</param>
    /// <param name="hand">右手模型</param>
    public void SetRightMemsData(NT_DATA RightData, Transform hand)
    {
        //右手四元数
        Quaternion Right = Quaternion.identity;
        //获取四元数校正值
        Quaternion rightMems = CalibrateMEMS.RightMEMSQuaternion(RightData);
        Right.x = rightMems.x;
        Right.y = -rightMems.z;
        Right.z = rightMems.y;
        Right.w = rightMems.w;
        if (hand != null&&RightController!=null)
        {
           
            //赋值四元数到手部模型
            hand.localRotation = Right;
            
            hand.rotation = rightcontrollerQuaternion * Quaternion.Inverse(RightController.transform.rotation) * hand.rotation;
           
        }
    }
    /// <summary>
    /// 获取校准后的右手九轴四元数数据
    /// </summary>
    /// <param name="HandData"></param>
    public Quaternion GetRightMemsQuaternion(NT_DATA HandData)
    {
        //右手四元数
        Quaternion Right = Quaternion.identity;
        //获取四元数校正值
        Quaternion rightMems = CalibrateMEMS.RightMEMSQuaternion(HandData);
        Right.x = -rightMems.x;
        Right.y = -rightMems.z;
        Right.z = rightMems.y;
        Right.w = rightMems.w;
        return Right;
    }
    /// <summary>
    /// 获取校准后的左手九轴四元数数据
    /// </summary>
    /// <param name="HandData"></param>
    /// <returns></returns>
    public Quaternion GetLeftMemsQuaternion(NT_DATA HandData)
    {
        //右手四元数
        Quaternion Left = Quaternion.identity;
        //获取四元数校正值
        Quaternion rightMems = CalibrateMEMS.LeftMEMSQuaternion(HandData);
        Left.x = -rightMems.x;
        Left.y = rightMems.z;
        Left.z = rightMems.y;
        Left.w = rightMems.w;
        return Left;
    }
    /// <summary>
    /// 重置九轴校准参数，控制器全局变量参数
    /// </summary>
    /// <param name="LeftController">左手控制器对象</param>
    /// <param name="RightController">右手控制器对象</param>
    public void RefreshMEMS(NT_DATA LeftData, NT_DATA RightData, Transform LeftController,Transform RightController)
    {

        //获取原生九轴数据
        CalibrateMEMS.GetOriginMEMS(LeftData, RightData);
        if (LeftController != null&& RightController!=null)
        { 
            //控制器          
            this.LeftController = LeftController;
            this.RightController = RightController;
            //控制器世界朝向
            leftcontrollerQuaternion = LeftController.rotation;
            rightcontrollerQuaternion = RightController.rotation;
        }
    }
    /// <summary>
    /// 重置九轴参数
    /// </summary>
    public void RefreshMEMS(NT_DATA LeftData, NT_DATA RightData)
    {

        //获取原生九轴数据
        CalibrateMEMS.GetOriginMEMS(LeftData, RightData);
    }


}
