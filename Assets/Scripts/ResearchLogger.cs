using System;
using System.IO;
using UnityEngine;

public static class ResearchLogger
{
    private static string filePath;

    private static void EnsureFile()
    {
        if (filePath == null)
        {
            string condition = GameManager.Instance.IsInVR ? "VR" : "Desktop";
            string fileName = $"log_{condition}_{DateTime.Now:yyyy-MM-dd_HHmm}.csv";
            filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.AppendAllText(filePath, "Timestamp,Event\n");
        }
    }

    public static void Log(string eventName)
    {
        EnsureFile();
        File.AppendAllText(filePath, $"{DateTime.Now:HH:mm:ss},{eventName}\n");
    }
}