using UnityEngine;

public static class ScoreTracker {
    public static int deaths;
    public static float timer;

    public static void Reset() {
        deaths = 0;
        timer = 0;
    }
}
