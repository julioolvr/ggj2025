using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;              // Speed of the bullet

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if we hit a bubble
        Bubble bubble = other.GetComponent<Bubble>();
        if (bubble != null)
        {
            bubble.PopBubble(false); // Call the bubble's pop effect

            Destroy(gameObject); // Destroy the bullet
        }
    }
}
