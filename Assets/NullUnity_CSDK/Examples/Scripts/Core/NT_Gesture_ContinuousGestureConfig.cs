/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 周立熙

** 最终修改时间：2016-11-29

** 功能描述： NullTouch手势配置类

*********************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class NT_Gesture_ContinuousGestureConfigList
{
    //手势配置集合
    public NT_Gesture_ContinuousGestureConfig[] configs;
}

[Serializable]
public class NT_Gesture_ContinuousGestureConfig
{
    //连续手势id
    public int continuousGesturesId;

    //连续手势名称
    public string continuousGesturesName;

    //连续手势总手势数
    public int totalCount;

    //组成连续手势的手势集合
    public int[] gestureList;

    //等待判断的时间
    public float holdTime;

    //起始手势
    public int startGesture;
}
