using UnityEngine;
using UnityEngine.EventSystems;

public class InfoHoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject info;

    public void OnPointerEnter(PointerEventData eventData) {
        if (info != null) info.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (info != null) info.SetActive(false);
    }
}

