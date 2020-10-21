using U = UnityEngine;

public static class Log {
    // -- types --
    public enum Level {
        None,
        Info,
        Debug
    }

    // -- props --
    private static Level sLevel = Level.Info;

    // -- commands --
    public static void SetLevel(Level level) {
        sLevel = level;
    }

    public static void Info(string format, params object[] args) {
        if (sLevel >= Level.Info) {
            U.Debug.LogFormat(format, args);
        }
    }

    public static void Debug(string format, params object[] args) {
        if (sLevel >= Level.Debug) {
            U.Debug.LogFormat(format, args);
        }
    }
}
