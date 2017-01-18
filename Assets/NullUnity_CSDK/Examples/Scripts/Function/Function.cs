using UnityEngine;
using System.Collections;

public static class Function {
    public static readonly bool IS_DEBUG_Log = true;
    public static readonly bool IS_DEBUG_Error = true;

    public static void DebugLog(object msg) {
        if (IS_DEBUG_Log) {
            Debug.Log(msg);
        }
    }

    public static void DebugError(object msg)
    {
        if (IS_DEBUG_Error)
        {
            Debug.LogError(msg);
        }
    }
}
