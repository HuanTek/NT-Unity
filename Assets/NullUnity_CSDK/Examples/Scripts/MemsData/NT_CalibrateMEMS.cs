/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 林晓峰

** 最终修改时间：2016-12-6

** 功能描述： 九轴数据校准

*********************************************************************************/
using UnityEngine;
using System.Collections;
using NT_CSDK_API;
public class NT_CalibrateMEMS  {
    //左手校准四元数
    float[] LeftMemsAvgr = new float[4];
    //右手校准四元数
    float[] RightMemsAvgr = new float[4];
    /// <summarY>
    /// 校正左手九轴四元数数据
    /// </summarY>
    /// <param name="HandData"></param>
    /// <returns></returns>
    public Quaternion LeftMEMSQuaternion(NT_DATA HandData)
    {
        Quaternion quaternion;
        quaternion.w = HandData.quaternion.W * LeftMemsAvgr[0] + HandData.quaternion.X * LeftMemsAvgr[1] +
             HandData.quaternion.Y * LeftMemsAvgr[2] + HandData.quaternion.Z * LeftMemsAvgr[3];
        quaternion.x = (HandData.quaternion.X * LeftMemsAvgr[0] - HandData.quaternion.W * LeftMemsAvgr[1] +
             HandData.quaternion.Y * LeftMemsAvgr[3] - HandData.quaternion.Z * LeftMemsAvgr[2]);
        quaternion.y= (HandData.quaternion.Y * LeftMemsAvgr[0] - HandData.quaternion.W * LeftMemsAvgr[2] +
             HandData.quaternion.Z * LeftMemsAvgr[1] - HandData.quaternion.X * LeftMemsAvgr[3]);
        quaternion.z = (HandData.quaternion.Z * LeftMemsAvgr[0] - HandData.quaternion.W * LeftMemsAvgr[3] +
             HandData.quaternion.X * LeftMemsAvgr[2] - HandData.quaternion.Y * LeftMemsAvgr[1]);
        return quaternion;
    }
    /// <summarY>
    /// 校正右手九轴四元数数据
    /// </summarY>
    /// <param name="HandData"></param>
    /// <returns></returns>
    public Quaternion RightMEMSQuaternion(NT_DATA HandData)
    {
        Quaternion quaternion;
        quaternion.w = (HandData.quaternion.W * RightMemsAvgr[0] + HandData.quaternion.X * RightMemsAvgr[1] +
             HandData.quaternion.Y * RightMemsAvgr[2] + HandData.quaternion.Z * RightMemsAvgr[3]);
        quaternion.x = (HandData.quaternion.X * RightMemsAvgr[0] - HandData.quaternion.W * RightMemsAvgr[1] +
             HandData.quaternion.Y * RightMemsAvgr[3] - HandData.quaternion.Z * RightMemsAvgr[2]);
        quaternion.y = (HandData.quaternion.Y * RightMemsAvgr[0] - HandData.quaternion.W * RightMemsAvgr[2] +
             HandData.quaternion.Z * RightMemsAvgr[1] - HandData.quaternion.X * RightMemsAvgr[3]);
        quaternion.z = (HandData.quaternion.Z * RightMemsAvgr[0] - HandData.quaternion.W * RightMemsAvgr[3] +
             HandData.quaternion.X * RightMemsAvgr[2] - HandData.quaternion.Y * RightMemsAvgr[1]);
        return quaternion;
    }
    /// <summary>
    /// 获取初始四元素定值
    /// </summary>
    /// <param name="LeftData"></param>
    /// <param name="rightData"></param>
    public void GetOriginMEMS(NT_DATA LeftData,NT_DATA rightData)
    {
        RightMemsAvgr[0] = rightData.quaternion.W;
        RightMemsAvgr[1] = rightData.quaternion.X;
        RightMemsAvgr[2] = rightData.quaternion.Y;
        RightMemsAvgr[3] = rightData.quaternion.Z;       
        LeftMemsAvgr[0] = LeftData.quaternion.W;
        LeftMemsAvgr[1] = LeftData.quaternion.X;
        LeftMemsAvgr[2] = LeftData.quaternion.Y;
        LeftMemsAvgr[3] = LeftData.quaternion.Z;
    }
}
