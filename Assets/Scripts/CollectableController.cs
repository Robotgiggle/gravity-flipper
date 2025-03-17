using UnityEngine;

public class CollectableController : MonoBehaviour
{

    GameObject resourceManager;

    void Start()
    {
        resourceManager = GameObject.Find("ResourceManager");
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == ("Player")){
            resourceManager.GetComponent<ResourceManager>().current_level_collectables += 1;
            Destroy(this.gameObject);
        }
    }
}
