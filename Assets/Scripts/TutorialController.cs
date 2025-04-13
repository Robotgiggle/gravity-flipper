using UnityEngine;

public class TutorialController : MonoBehaviour {
    public GameObject[] m_popups = new GameObject[4];
    
    GameManager m_gameManager;
    
    void Start() {
        m_gameManager = GameManager.TheInstance;
    }

    public void ShowPopup(int index) {
        // TODO: fade in
        m_gameManager.m_inputsLocked = true;
        m_popups[index].SetActive(true);
    }

    public void HidePopup(int index) {
        // TODO: fade out
        m_gameManager.m_inputsLocked = false;
        m_popups[index].SetActive(false);
    }
}
