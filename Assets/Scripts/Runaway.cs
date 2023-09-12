using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    public float runSpeed = 5.0f; // Speed at which the animal runs
    public float runDistance = 2.0f; // Minimum distance to run away from the mouse pointer

    private Transform playerTransform;

    void Start()
    {
        // Find the player (assuming the mouse pointer represents the player)
        playerTransform = Camera.main.transform;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogError("PlayerTransform is not assigned. Make sure you assign it in the Inspector or find it correctly in code.");
            return;
        }

        // Calculate the distance between the animal and the mouse pointer
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if the player (mouse pointer) is too close
        if (distanceToPlayer < runDistance)
        {
            // Calculate the direction away from the mouse pointer
            Vector3 runDirection = (transform.position - playerTransform.position).normalized;

            // Move the animal in the opposite direction (run away)
            transform.Translate(runDirection * runSpeed * Time.deltaTime);
        }
    }

}
