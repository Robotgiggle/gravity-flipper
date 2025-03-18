using UnityEngine;

public class BonusController : MonoBehaviour {
    public int index;
    
    GameObject resourceManager;

    void Start() {
        if (ScoreTracker.bonuses[index]) {
            // TODO: render a greyed-out version instead
            gameObject.SetActive(false);
        }
    }
}
