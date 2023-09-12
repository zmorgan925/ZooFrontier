using UnityEngine;

public class CharacterShuffle : MonoBehaviour
{
    public float speed = 1f;
    public float changeDirectionTime = 1f;
    public LayerMask tilemapLayer;
    private Vector2 currentDirection;
    private Rigidbody2D rb;
    private float elapsed = 0f;
    private Vector2 startingPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;
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
        Vector2[] directions = {
            new Vector2(1, 0.5f),
            new Vector2(1, -0.5f),
            new Vector2(-1, -0.5f),
            new Vector2(-1, 0.5f)
        };

        currentDirection = directions[Random.Range(0, directions.Length)];

        // Check if the new position will be inside the 3x3 boundary
        Vector2 predictedPosition = rb.position + currentDirection;
        if (Mathf.Abs(predictedPosition.x - startingPosition.x) > 2 || Mathf.Abs(predictedPosition.y - startingPosition.y) > 1)
        {
            currentDirection = Vector2.zero;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, 1f, tilemapLayer);
        if (hit.collider == null)
        {
            currentDirection = Vector2.zero;
        }
    }

    void Move()
    {
        rb.velocity = currentDirection * speed;
    }
}
