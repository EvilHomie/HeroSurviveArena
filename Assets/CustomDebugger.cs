using System;
using UnityEngine;

public class CustomDebugger
{
    public static bool Enabled;
    public static void Log(string logText)
    {
        if (Enabled)
        {
            Debug.Log(logText);
        }
    }

    public static void Exeption(string logText)
    {
        if (Enabled)
        {
            throw new Exception(logText);
        }
    }
}
