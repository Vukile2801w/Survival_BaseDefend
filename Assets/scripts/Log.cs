using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Log
{
    bool isDebugEnabled = true;
    string logPrefix = "";

    public Log(bool isDebugEnabled = true, string logPrefix = "")
    {
        this.isDebugEnabled = isDebugEnabled;
        this.logPrefix = logPrefix;
    }

    public void log(string message, UnityEngine.Object context = null)
    {
        if (!isDebugEnabled) return;

        Debug.Log(logPrefix + message, context);
    }

    public void warn(string message, UnityEngine.Object context = null)
    {
        if (!isDebugEnabled) return;
        Debug.LogWarning(logPrefix + message, context);
    }

    public void error(string message, UnityEngine.Object context = null)
    {
        if (!isDebugEnabled) return;
        Debug.LogError(logPrefix + message, context);
    }
}
