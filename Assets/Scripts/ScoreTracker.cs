using UnityEngine;

public static class ScoreTracker {
    public static int deaths;
    public static float timer;
    public static bool[] bonuses = new bool[10];
    public static int holdingBonus;

    public static void Reset() {
        deaths = 0;
        timer = 0;
    }

    public static void TrySaveBonus() {
        if (holdingBonus >= 0) bonuses[holdingBonus] = true;
    }
}
