using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControlWithSorting : MonoBehaviour
{
    public float speed = 3.0f; // Movement speed
    public LayerMask tilemapLayer; // Layer of the tilemap

    private Vector3 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Dynamic sorting based on Y-position
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100f);

        // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }

        // If target position is set and character isn't there yet, move towards it
        if (isMoving)
        {
            MoveCharacter();
        }
    }

    void SetTargetPosition()
    {
        // Cast a ray from the camera to the clicked point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, tilemapLayer);

        // If it hits the tilemap, set the target position
        if (hit.collider != null)
        {
            targetPosition = new Vector3(hit.point.x, hit.point.y, transform.position.z);
            isMoving = true;
        }
    }

    void MoveCharacter()
    {
        Vector3 direction = targetPosition - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;

        // Check if the character is close to the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }
}
