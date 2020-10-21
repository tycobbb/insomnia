using U = UnityEngine;

public static class Log {
    // -- types --
    public enum Level {
        None,
        Info,
        Debug
    }

    // -- props --
    private static Level sLevel;

    // -- commands --
    public static void SetLevel(Level level) {
        #if UNITY_EDITOR
            sLevel = level;
        #endif
    }

    public static void Info(string format, params object[] args) {
        #if UNITY_EDITOR
            if (sLevel >= Level.Info) {
                U.Debug.LogFormat(format, args);
            }
        #endif
    }

    public static void Debug(string format, params object[] args) {
        #if UNITY_EDITOR
            if (sLevel >= Level.Debug) {
                U.Debug.LogFormat(format, args);
            }
        #endif
    }
}
