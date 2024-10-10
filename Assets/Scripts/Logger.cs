using UnityEngine;

public static class Logger {
    public static void Log(string message) {
        Debug.Log($"[Turret Defense] {message}");
    }

    public static void LogError(string message) {
        Debug.LogError($"[Turret Defense] ERROR: {message}");
    }

    public static void LogWarning(string message) {
        Debug.LogWarning($"[Turret Defense] WARNING: {message}");
    }
}
