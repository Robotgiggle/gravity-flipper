using UnityEngine;

public class PopupTriggerController : MonoBehaviour {
    public TutorialController m_tutorialController;
    public int index;

    void OnTriggerEnter2D() {
        m_tutorialController.ShowPopup(index);
    }
}
