using UnityEngine;
using UnityEngine.UI;

public class ToggleTextController : MonoBehaviour {
    public void SwitchText(bool enabled) {
        if (enabled) GetComponent<Text>().text = "Enabled";
        else GetComponent<Text>().text = "Disabled";
    }
}
