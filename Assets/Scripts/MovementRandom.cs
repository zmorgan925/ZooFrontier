using UnityEngine;

public class IsometricGridWander : MonoBehaviour
{
    public float speed = 3f;
    public float changeDirectionTime = 2f;
    private float elapsed = 0f;
    private Vector2 currentDirection;
    private Rigidbody2D rb;
    private Vector2 startingPosition;
    public float gridLimit = 1.5f;  // The distance from the starting point for a 3x3 grid
    private const int MAX_ATTEMPTS = 10;  // Maximum number of direction choosing attempts

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position; // Capture the starting position
        ChooseDirection();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= changeDirectionTime)
        {
            elapsed = 0;
            ChooseDirection();
        }

        Move();
    }

    void ChooseDirection()
    {
        int attempts = 0;
        bool validDirectionChosen = false;

        while (attempts < MAX_ATTEMPTS && !validDirectionChosen)
        {
            int choice = Random.Range(0, 4);

            switch (choice)
            {
                case 0:
                    currentDirection = new Vector2(1, 0.5f).normalized; // north in isometric
                    break;
                case 1:
                    currentDirection = new Vector2(1, -0.5f).normalized; // east in isometric
                    break;
                case 2:
                    currentDirection = new Vector2(-1, -0.5f).normalized; // south in isometric
                    break;
                case 3:
                    currentDirection = new Vector2(-1, 0.5f).normalized; // west in isometric
                    break;
            }

            // Check if the new position is within our 3x3 grid boundary
            Vector2 nextPosition = (Vector2)transform.position + currentDirection;
            if (Mathf.Abs(nextPosition.x - startingPosition.x) <= gridLimit && Mathf.Abs(nextPosition.y - startingPosition.y) <= gridLimit)
            {
                validDirectionChosen = true;
            }

            attempts++;
        }

        if (!validDirectionChosen)
        {
            currentDirection = Vector2.zero;  // Stop if a valid direction can't be found after max attempts
        }
    }

    void Move()
    {
        rb.velocity = currentDirection * speed;
    }
}
