using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OutputLog : MonoBehaviour
{
    private static DateTime date = DateTime.Now;
    private static int num;
    private static string path;

    private void Start()
    {
        num = UnityEngine.Random.Range(0, 100);
        path = "C:\\Users\\Owner\\Documents\\UnoCardGameLogs\\log-" + date.ToString("yyyy-MM-dd-" + num) + ".log";
    }

    public static void WriteToOutput(object output)
    {
        Debug.Log(output);
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine("[" + date.ToString("hh:mm:ss") + "] " + output);
            sw.Close();
        }
    }
}
