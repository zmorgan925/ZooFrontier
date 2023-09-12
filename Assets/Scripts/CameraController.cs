using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float zoomSpeed = 1000000; // Increase this value for faster zooming
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f; // Original maxZoom value

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Handle zoom using mouse scroll
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scrollInput);
#else
        // Check if a touch has started
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchStartPos = Input.GetTouch(0).position;
        }

        // Check if the touch has moved
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchEndPos = Input.GetTouch(0).position;
            Vector2 touchDelta = touchEndPos - touchStartPos;

            // Convert touch movement to world movement
            Vector3 moveDirection = new Vector3(-touchDelta.x, -touchDelta.y, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(moveDirection);

            // Update the start position for the next frame
            touchStartPos = Input.GetTouch(0).position;
        }

        // Handle zoom using pinch-to-zoom gesture
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float zoomInput = prevTouchDeltaMag - touchDeltaMag;
            ZoomCamera(zoomInput);
        }
#endif
    }

    void ZoomCamera(float zoomInput)
    {
        Camera mainCamera = Camera.main;
        float zoomAmount = zoomInput * Time.deltaTime;

        // Calculate the new orthographic size and clamp it within minZoom and maxZoom
        float newOrthoSize = Mathf.Clamp(mainCamera.orthographicSize - zoomAmount, minZoom, maxZoom);

        // Set the new orthographic size directly
        mainCamera.orthographicSize = newOrthoSize;
    }
}
