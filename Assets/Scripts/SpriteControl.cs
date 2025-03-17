using UnityEngine;

public class FitSpriteToObject : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Get the object's size in world units
        Vector2 objectSize = GetComponent<BoxCollider2D>().size;

        // Get the sprite's size in world units
        Vector2 spriteSize = sr.sprite.bounds.size;

        // Calculate the scale factors
        float scaleX = objectSize.x / spriteSize.x;
        float scaleY = objectSize.y / spriteSize.y;

        // Apply scale
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}