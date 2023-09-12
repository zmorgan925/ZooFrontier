using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float runAwaySpeed = 4.0f;
    public float detectionRadius = 2.0f;
    public Sprite clickedSprite; // Assign your "clicked" sprite here in the Inspector.
    public AudioClip clickSound; // Assign your click sound here in the Inspector.
    public int characterID; //ID  to make sure changing correct animal

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool runningAway = false;
    private bool isClicked = false;

    private Rigidbody2D rb2D;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    //asd
    void Start()
    {
        initialPosition = transform.position;
        targetPosition = GetRandomPositionInGrid();
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clickSound; // Assign the click sound in the Inspector.
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite; // Store the original sprite.
    }

    void Update()
    {
        if (!runningAway)
        {
            MoveRandomlyInGrid();
            CheckForMouse();
        }
        else
        {
            RunAwayFromMouse();
        }

        // Handle click to change the sprite and play the sound
        if (Input.GetMouseButtonDown(0))
        {
            ChangeSprite();
            PlayClickSound();
        }
    }

    void MoveRandomlyInGrid()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPositionInGrid();
        }

        Vector2 moveDirection = (targetPosition - transform.position).normalized;
        rb2D.velocity = moveDirection * moveSpeed;
    }

    Vector3 GetRandomPositionInGrid()
    {
        float randomX = Random.Range(initialPosition.x - 1.0f, initialPosition.x + 1.0f);
        float randomY = Random.Range(initialPosition.y - 1.0f, initialPosition.y + 1.0f);

        return new Vector3(randomX, randomY, transform.position.z);
    }

    void CheckForMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Match z-coordinates.

        float distanceToMouse = Vector3.Distance(transform.position, mousePosition);

        if (distanceToMouse < detectionRadius)
        {
            runningAway = true;
        }
    }

    void RunAwayFromMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Match z-coordinates.

        Vector2 runDirection = (transform.position - mousePosition).normalized;
        rb2D.velocity = runDirection * runAwaySpeed;

        // Check if the creature is far enough from the mouse to stop running away
        if (Vector3.Distance(transform.position, mousePosition) > detectionRadius)
        {
            runningAway = false;
            targetPosition = GetRandomPositionInGrid();
            rb2D.velocity = Vector2.zero;
        }
    }

    void ChangeSprite()
    {
        if (characterID == 1)
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

    void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
