using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class MakeTextAppear : MonoBehaviour
{
    public Image[] uiImages;  // Array to hold references to different images
    public TextMeshProUGUI[] uiTexts;    // Array to hold references to the text components
    private Dictionary<string, Image> imageDictionary;
    private Dictionary<string, TextMeshProUGUI> textDictionary;

    void Start()
    {
        imageDictionary = new Dictionary<string, Image>();
        textDictionary = new Dictionary<string, TextMeshProUGUI>();


        // Initialize the dictionaries
        for (int i = 0; i < uiImages.Length; i++)
        {
            imageDictionary.Add("TextCollision" + (i + 1), uiImages[i]);
            textDictionary.Add("Text" + (i + 1), uiTexts[i]);
        }

        // Disable both images and text initially
        foreach (Image image in uiImages)
        {
            image.enabled = false;
        }

        foreach (TextMeshProUGUI text in uiTexts)
        {
            text.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (imageDictionary.ContainsKey(other.name))
        {
            Debug.Log(other.name);
            // Enable the image and text for the corresponding collision
            imageDictionary[other.name].enabled = true;
            if (other.name == "TextCollision1")
            {
                textDictionary["Text1"].enabled = true;
            }
            else if (other.name == "TextCollision2")
            {
                textDictionary["Text2"].enabled = true;
            }
            else
            {
                textDictionary["Text3"].enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (Image image in uiImages)
        {
            image.enabled = false;
        }

        foreach (TextMeshProUGUI text in uiTexts)
        {
            text.enabled = false;
        }
    }
}