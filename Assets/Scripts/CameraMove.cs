using UnityEngine;

public class CameraMove : MonoBehaviour
{
    const float cameraMoveSpeed = .1f;
    const float cameraScrollSpeed = .5f;
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
        if (input.Camera.Click.IsPressed())
        {
            Vector3 delta = input.Camera.Drag.ReadValue<Vector2>();
            this.transform.transform.position -= (delta * cameraMoveSpeed);
        }

        float zoom = input.Camera.Zoom.ReadValue<float>();
        this.GetComponent<Camera>().orthographicSize -= zoom * cameraScrollSpeed;
        // Debug.Log(zoom);
    }
}
