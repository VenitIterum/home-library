using UnityEngine;

public class CameraMove : MonoBehaviour
{
    const float cameraMoveSpeed = .03f;
    const float cameraScrollSpeed = .5f;
    const float maxValueZoom = 15.0f;
    const float minValueZoom = 1.5f;
    private CameraInputActions input;

    private void Awake()
    {
        input = new CameraInputActions();
    }

    private void OnEnable()
    {
        input.Camera.Enable();
    }

    private void OnDisable()
    {
        input.Camera.Disable();
    }

    private void Update()
    {
        Camera cam = GetComponent<Camera>();
        float zoom = input.Camera.Zoom.ReadValue<float>();
        
        if (input.Camera.Click.IsPressed())
        {
            Vector3 delta = input.Camera.Drag.ReadValue<Vector2>();
            this.transform.transform.position -= delta * cam.orthographicSize * cameraMoveSpeed;
        }

        cam.orthographicSize -= zoom * cameraScrollSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minValueZoom, maxValueZoom);
    }
}
