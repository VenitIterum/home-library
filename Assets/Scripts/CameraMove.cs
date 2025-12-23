using System;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private const float cameraMoveSpeed = .03f;
    private const float cameraScrollSpeed = .5f;
    private const float maxValueZoom = 15.0f;
    private const float minValueZoom = 1.5f;
    private const float maxValueMoveX = 25.0f;
    private const float maxValueMoveY = 7.0f;
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
        Vector3 pos = this.transform.position;
        float zoom = input.Camera.Zoom.ReadValue<float>();

        if (input.Camera.Click.IsPressed())
        {
            Vector3 delta = input.Camera.Drag.ReadValue<Vector2>();
            pos -= delta * cam.orthographicSize * cameraMoveSpeed;
            
            pos.x = Math.Clamp(pos.x, -maxValueMoveX, maxValueMoveX);
            pos.y = Math.Clamp(pos.y, -maxValueMoveY, maxValueMoveY);
            this.transform.position = pos;
        }
        
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoom * cameraScrollSpeed, minValueZoom, maxValueZoom);
    }
}
