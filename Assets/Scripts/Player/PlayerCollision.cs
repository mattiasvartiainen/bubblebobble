
namespace Assets.Scripts.Player
{
    using UnityEngine;

    public class PlayerCollision : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnTriggerEnter2D(Collider2D target)
        {
            Debug.Log($"Bubblun OnTriggerEnter2D {target.gameObject.tag} {target.gameObject.name}");

            if (target.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Bubblun is dead!");
            }

            if (target.gameObject.CompareTag("BubbledEnemy"))
            {
                Debug.Log("Check if enemy is killed");
            }

            if (target.gameObject.name.StartsWith("PickupObject"))
            {
                Debug.Log($"Got {target.gameObject.GetComponent<PickupObject>().Score}");
                Destroy(target.gameObject);
            }
        }
    }
}
