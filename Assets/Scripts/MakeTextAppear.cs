using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MakeTextAppear : MonoBehaviour
{
    public Image[] uiImages;  // Array to hold references to different images
    private Dictionary<string, Image> imageDictionary;

    void Start()
    {
        imageDictionary = new Dictionary<string, Image>();
        imageDictionary.Add("TextCollision1", uiImages[0]);
        imageDictionary.Add("TextCollision2", uiImages[1]);
        imageDictionary.Add("TextCollision3", uiImages[2]);

        foreach (Image image in uiImages)
        {
            image.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (imageDictionary.ContainsKey(other.name))
        {
            Debug.Log(other.name);
            imageDictionary[other.name].enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (Image image in uiImages)
        {
            image.enabled = false;
        }
    }
}