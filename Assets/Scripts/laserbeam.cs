using UnityEngine;
using System.Collections;

public class LaserTrap : MonoBehaviour
{
    public Transform endpointA;       
    public Transform endpointB;       
    public GameObject hitboxPrefab;   
    public float hitboxSpacing = 1f;  
    public float spawnInterval = 2f;  
    public float hitboxLifetime = 1f; 
    public Vector3 hitboxScale = new Vector3(1f, 1f, 1f); 

    void Start()
    {
        StartCoroutine(SpawnLaser());
    }

    IEnumerator SpawnLaser()
    {
        while (true)
        {
            float distance = Vector2.Distance(endpointA.position, endpointB.position);
            int numHitboxes = Mathf.FloorToInt(distance / hitboxSpacing);
            
            // Calculate the normalized direction from A to B
            Vector2 direction = (endpointB.position - endpointA.position).normalized;

            // Spawn hitboxes 
            for (int i = 0; i <= numHitboxes; i++)
            {
                Vector2 spawnPosition = (Vector2)endpointA.position + direction * hitboxSpacing * i;
                GameObject hitbox = Instantiate(hitboxPrefab, spawnPosition, Quaternion.identity);
                hitbox.transform.localScale = hitboxScale;
                Destroy(hitbox, hitboxLifetime);
            }
            
            // Wait before calling coroutine again
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
