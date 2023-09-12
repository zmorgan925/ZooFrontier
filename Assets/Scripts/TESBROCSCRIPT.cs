using UnityEngine;

public class CharacterShuffle : MonoBehaviour
{
    public float speed = 1f;
    public float changeDirectionTime = 1f;
    public LayerMask tilemapLayer;
    public Sprite clickedSprite; //The holder for the changed to sprite
    public int characterID; //ID  to make sure changing correct animal


    private Vector2 currentDirection;
    private Rigidbody2D rb;
    private float elapsed = 0f;
    private Vector2 startingPosition;




    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite; //Holds the original sprite before clicked.

    //Bools for isClicked
    private bool isClicked = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;
        ChooseDirection();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite; // Store the original sprite.
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= changeDirectionTime)
        {
            elapsed = 0;
            ChooseDirection();
        }

        //Check for mouse click. IF clicked, change sprite
        if(Input.GetMouseButtonDown(0))
        {
            ChangeSprite();
        }

        Move();
    }

    void ChangeSprite()
    {
        if (characterID == 2)
        {
            if (isClicked)
            {
                spriteRenderer.sprite = originalSprite; // Switch back to the original sprite.
                isClicked = false;
            }
            else
            {
                spriteRenderer.sprite = clickedSprite;
                transform.localScale = new Vector3(0.5f, 0.5f, -2f);
                isClicked = true;
            }
        }
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
