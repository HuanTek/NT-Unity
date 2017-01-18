/********************************************************************************
** 公司：广州幻境科技有限公司

** 作者： 林晓峰

** 最终修改时间：2016-11-23

** 功能描述： NullTouch设备的SDK接口与数据结构定义

*********************************************************************************/
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using NT_CSDK_API;
using System;
namespace NT_CSDK_API
{

    /// <summary>
    /// 九轴传感器四元数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NT_QUATERNION
    {
        private float w,x,y,z;

        public float W
        {
            get
            {
                return  float.Parse( w.ToString("F4"));
            }

            set
            {
                w = value;
            }
        }

        public float X
        {
            get
            {
                return float.Parse(x.ToString("F4"));
            }

            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return float.Parse(y.ToString("F4"));
            }

            set
            {
                y = value;
            }
        }

        public float Z
        {
            get
            {
                return float.Parse(z.ToString("F4"));
            }

            set
            {
                z = value;
            }
        }
        //public float w, x, y, z;
    }
    /// <summary>
    /// 九轴传感器三维向量
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NT_VECTOR
    {
        public float x, y, z;
    }
    /// <summary>
    /// 弯曲传感器弯曲度，标准化为0-1的范围。
    /// 手指的弯曲程度在校准后按比例输出0-1的数据
    /// 接近0表示手指张开，接近1表示手指最大弯曲。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NT_FINGERS
    {
        public float thumb, index, middle, ring, pinky;
    }
    /// <summary>
    /// 动捕手套的状态，表示一个手套。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NT_STATE
    {
        public int bConnectState;//表示手套的连接状态 对应NT_CONNECTED_STATE枚举
        public int bSelfTest;// 表示手套自检状态
        public int eEception;   //对应NT_EXCEPTIONS

    }

    
    /// <summary>
    /// 动捕手套的数据。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NT_DATA
    {
        public uint packetNumber;
        public int eValueData;
        public NT_QUATERNION quaternion;
        public NT_VECTOR euler;
        public NT_FINGERS fingers;
        public NT_VECTOR acc;
        public NT_VECTOR gyro;
        public NT_VECTOR mag;
    }
    /// <summary>
    /// 震动模式参数配置结构体。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NT_VBT_MOD
    {
        public int eMotors;//需要震动的电机枚举，对应NT_MOTORS
        public uint strength;//振动强度值为电机PWM的占空比，范围0-100
        public uint time;//振动时间 = 时间值 * 2ms, 时间值范围15-250
    }
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum NT_CONNECTED_STATE
    {
        NT_CONNECTED = 0,           //连接状态
        NT_CONNECTING_FALSE = -1,   //未连接上状态
        NT_CONNECTED_FALSE = -2,    //连接异常中断后的状态
        NT_CONNECTED_TWO_TRUE = -3, //连接一场中断后重新连接上的状态
    }
    /// <summary>
    /// Null Touch SDK成功、失败状态定义。
    /// </summary>
    public enum NT_RET_STATE
    {
        NT_TRUE = 0,
        NT_FALSE = -1
    }
    /// <summary>
    /// Null Touch文件操作结果。
    /// </summary>
    public enum NT_FILE_OPERATE
    {
        NT_FILE_OK = 0,          //操作文件信息正常
        NT_FILE_ERR = -1,        //操作文件异常
        NT_NO_PERMISSION = -2,
        NT_NO_CALIBREAT_VALUE = -3,
        NT_NO_WRITE = -4,
    }
    /// <summary>
    /// Null Touch连接错误代码定义。
    /// </summary>
    public enum NT_CONN_ERR_CODE
    {
        NT_TWO_CONN_OK = 0,                   //插入两个蓝牙，并且两个串口连接成功
        NT_NO_BLUE_HOST = 10,                 //没有插入蓝牙设备
        NT_ONE_HOST_AND_NO_CONN = 101,        //插入一个蓝牙设备并且连接不成功
        NT_ONE_HOST_AND_CONN_LEFT = 102,      //插入一个蓝牙设备，并且是左手，连接成功
        NT_ONE_HOST_AND_CONN_RIGHT = 103,     //插入一个蓝牙设备，并且是右手，连接成功
        NT_TWO_HOST_AND_NO_CONN = 200,        //插入两个蓝牙设备，并且两个设备同时连接不成功
        NT_TWO_HOST_AND_CONN_LEFT = 201,      //插入两个蓝牙设备，并且只有左手连接成功
        NT_TWO_HOST_AND_CONN_RIGHT = 202,     //插入两个蓝牙设备，并且只有右手连接成功
    }
    /// <summary>
    /// GetDate接口返回误码定义。
    /// </summary>
    public enum NT_ERR_GETDATA
    {
        NT_NORMAL = 0,
        NT_CRC_FAIL = -1,
        NT_DEV_DISCNN = -2,
    }
    /// <summary>
    /// 手套的异常的定义，枚举名称最后字段的5个二进制字符含义定义为按bit对应5个手指
    /// 从左至右bit5-bit0分别对应拇指、食指、中指、无名指、小指。
    /// </summary>
    public enum NT_EXCEPTIONS
    {
        NTEXP_GLOVE_DISCONNECT,//手套的蓝牙连接断开
        NTEXP_FLX_BROKE_01111,//弯曲传感器脱落,拇指
        NTEXP_FLX_BROKE_10111, //弯曲传感器脱落,食指
        NTEXP_FLX_BROKE_11011, //弯曲传感器脱落,中指
        NTEXP_FLX_BROKE_11101, //弯曲传感器脱落,无名指
        NTEXP_FLX_BROKE_11110, //弯曲传感器脱落,小指
        NTEXP_FLX_BROKE_00111, //弯曲传感器脱落, 拇指、食指
        NTEXP_FLX_BROKE_10011, //弯曲传感器脱落, 食指、中指
        NTEXP_FLX_BROKE_11001, //弯曲传感器脱落, 中指、无名指
        NTEXP_FLX_BROKE_11100, //弯曲传感器脱落, 无名指、小指
        NTEXP_FLX_BROKE_00011, //弯曲传感器脱落, 拇指、食指、中指
        NTEXP_FLX_BROKE_10001, //弯曲传感器脱落, 食指、中指、无名指
        NTEXP_FLX_BROKE_11000, //弯曲传感器脱落, 中指、无名指、小指
        NTEXP_FLX_BROKE_00001, //弯曲传感器脱落, 拇指、食指、中指、无名指
        NTEXP_FLX_BROKE_10000, //弯曲传感器脱落, 食指、中指、无名指、小指
        NTEXP_FLX_BROKE_00000//弯曲传感器全部脱落, 拇指、食指、中指、无名指、小指

    }
    /// <summary>
    /// 定义请求数据的类型
    /// </summary>
    public enum NT_TYPE
    {
        NT_QUATERNION_FLEX = 0, //四元数+弯曲数据
        NT_EULER_FLEX,  //欧拉角+弯曲数据
        NT_ACCELERATION,//加速度数据
        NT_GYRO,         //陀螺仪数据
        NT_MAGNETIC     //磁力计数据

    }
    /// <summary>
    /// 制定震动电机的枚举，可制定多个电机，以相同的模式进行震动。
    /// </summary>
    public enum NT_MOTORS
    {
        NTM_THUMB,
        NTM_INDEX,
        NTM_PALM,
        NTM_THUMB_INDEX,
        NTM_THUMB_PALM,
        NTM_INDEX_PALM,
        NTM_ALL,
    }
    /// <summary>
    /// 手的枚举，区分左右手。
    /// </summary>
    public enum NT_HAND
    {
        NT_LEFT = 1,          //左手
        NT_RIGHT = 2,         //右手
        NT_DOUBLE_HAND = 3,   //双手
    }
    public class NT_CSDK_Interop
    {

        [DllImport("kernel32.dll")]
        internal extern static IntPtr LoadLibrary(String path);//path 就是dll路径 返回结果为0表示失败。
        [DllImport("kernel32.dll")]
        internal extern static IntPtr GetProcAddress(IntPtr lib, String funcName);//lib是LoadLibrary返回的句柄，funcName 是函数名称 返回结果为0标识失败。
        [DllImport("kernel32.dll")]
        internal extern static bool FreeLibrary(IntPtr lib);

    }
    /// <summary>
    /// API函数封装
    /// </summary>
    public class NT_CSDK_Function
    {    
        //操作句柄
        static IntPtr hLib;
        //委托函数
        internal delegate NT_FILE_OPERATE ReadFlexConfig();
        internal delegate NT_CONN_ERR_CODE ConnectGlove();
        internal delegate NT_RET_STATE Exit();
        internal delegate NT_ERR_GETDATA GetData(NT_HAND hand, NT_TYPE type, ref NT_DATA data);
        internal delegate NT_RET_STATE GetState(NT_HAND hand, ref NT_STATE gloveState);
        internal delegate NT_RET_STATE SetVibration(NT_HAND hand, NT_VBT_MOD vbt_mod);
        /// <summary>
        /// 加载DLL
        /// </summary>
        public static void LoadLibrary()
        {
            hLib = NT_CSDK_Interop.LoadLibrary(@"Assets\NullUnity_CSDK\Plugins\NTSDK.dll");
         //   hLib = NT_CSDK_Interop.LoadLibrary(@"NullTouch_Data\Plugins\NTSDK.dll");
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        public static NT_FILE_OPERATE NT_ReadFlexConfig()
        {
            
           IntPtr apiFun = NT_CSDK_Interop.GetProcAddress(hLib, "NT_ReadFlexConfig");
            ReadFlexConfig readflexconfig = (Delegate)Marshal.GetDelegateForFunctionPointer(apiFun, typeof(ReadFlexConfig)) as ReadFlexConfig;
            CheckPointer(apiFun);
            return readflexconfig();
        }
        public static NT_RET_STATE NT_GetState(NT_HAND hand,  ref NT_STATE gloveState)
        {
            IntPtr apiFun = NT_CSDK_Interop.GetProcAddress(hLib, "NT_GetState");
            GetState getstate = (Delegate)Marshal.GetDelegateForFunctionPointer(apiFun, typeof(GetState)) as GetState;
            CheckPointer(apiFun);
            return getstate(hand,ref gloveState);
        }
        /// <summary>
        /// 连接手套设备
        /// </summary>
        /// <returns></returns>
        public static NT_CONN_ERR_CODE NT_ConnectGlove()
        {
            IntPtr apiFun = NT_CSDK_Interop.GetProcAddress(hLib, "NT_ConnectGlove");
            ConnectGlove connectglove = (Delegate)Marshal.GetDelegateForFunctionPointer(apiFun, typeof(ConnectGlove)) as ConnectGlove;
            CheckPointer(apiFun);
            return connectglove();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static NT_ERR_GETDATA NT_GetData(NT_HAND hand, NT_TYPE type, ref NT_DATA data)
        {
            IntPtr apiFun = NT_CSDK_Interop.GetProcAddress(hLib, "NT_GetData");
            GetData getdata = (Delegate)Marshal.GetDelegateForFunctionPointer(apiFun, typeof(GetData)) as GetData;
            CheckPointer(apiFun);
            return getdata(hand, type, ref data);
        }

        /// <summary>
        /// 传感器震动接口
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="vbt_mod"></param>
        /// <returns></returns>
        public static NT_RET_STATE NT_SetVibration(NT_HAND hand, NT_VBT_MOD vbt_mod)
        {
            IntPtr apiFun = NT_CSDK_Interop.GetProcAddress(hLib, "NT_SetVibration");
            SetVibration setvibration = (Delegate)Marshal.GetDelegateForFunctionPointer(apiFun, typeof(SetVibration)) as SetVibration;
            CheckPointer(apiFun);
            return setvibration(hand, vbt_mod);
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public static NT_RET_STATE NT_Exit()
        {
            IntPtr apiFun = NT_CSDK_Interop.GetProcAddress(hLib, "NT_Exit");
            Exit exit = (Delegate)Marshal.GetDelegateForFunctionPointer(apiFun, typeof(Exit)) as Exit;
            CheckPointer(apiFun);
            return exit();
        }
        /// <summary>
        /// 卸载DLL
        /// </summary>
        public static void FreeLibrary()
        {
            NT_CSDK_Interop.FreeLibrary(hLib);
        }
        public static void CheckPointer(IntPtr apiFun)
        {
            if (apiFun.ToInt32() == 0)
            {
                Debug.Log("函数没有找到");
            }
        }
    }

    /// <summary>
    /// 初始化手套                                                                                                                                                                             
    /// </summary>
    public class InitGlove
    {

        //连接状态结构体
        public static NT_STATE nt_state;
        public static bool Init()
        {
            bool initState = false;
            NT_CSDK_Function.LoadLibrary();
            NT_FILE_OPERATE nt_file_operate;
            nt_file_operate = NT_CSDK_Function.NT_ReadFlexConfig();
            switch(nt_file_operate)
            {
                case NT_FILE_OPERATE.NT_FILE_OK:
                    Debug.Log("成功读取配置文件");
                    initState = true;
                    break;
                case NT_FILE_OPERATE.NT_FILE_ERR:
                    Debug.Log("文件操作错误");
                    break;
                case NT_FILE_OPERATE.NT_NO_PERMISSION:
                    Debug.Log("没有文件操作权限");
                    break;
                case NT_FILE_OPERATE.NT_NO_CALIBREAT_VALUE:
                    Debug.Log("读取校准文件数据失败");
                    break;
                case NT_FILE_OPERATE.NT_NO_WRITE:
                    Debug.Log("没有写入");
                    break;
            }
            return initState;
        }

        /// <summary>
        /// 关闭设备串口,动态加载DLL的卸载方法
        /// </summary>
        public static void AllExit()
        {
            NT_CSDK_Function.NT_Exit();
            NT_CSDK_Function.FreeLibrary();
        }

    }
}
