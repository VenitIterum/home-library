using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    private const float cameraMoveSpeed     = .03f;
    private const float cameraScrollSpeed   = .5f;
    private const float maxValueZoom        = 15.0f;
    private const float minValueZoom        = 1.5f;
    private const float baseMaxValueMoveX   = 5.0f;
    private const float baseMaxValueMoveY   = 3.0f;
    // private const float boundsMultiplier    = 20.5f;

    private Camera cam;
    private CameraInputActions input;
    private Mouse mouse;

    private float currentMaxMoveX;
    private float currentMaxMoveY;

    private void Awake()
    {
        input = new CameraInputActions();
        cam = GetComponent<Camera>();
        mouse = Mouse.current;
    }

    private void OnEnable()
    {
        input.Camera.Enable();
        UpdateMovementBounds();
    }

    private void OnDisable()
    {
        input.Camera.Disable();
    }

    private void Update()
    {
        Vector3 pos = this.transform.position;
        float zoom = input.Camera.Zoom.ReadValue<float>();

        if(input.Camera.Click.IsPressed())
        {
            Vector3 delta = input.Camera.Drag.ReadValue<Vector2>();
            pos -= delta * cam.orthographicSize * cameraMoveSpeed;
            
            this.transform.position = MaxLimitsCameraMovement(pos);
        }
        
        if(zoom != 0)
        {
            float oldZoom = cam.orthographicSize;

            Vector2 mousePos = mouse.position.ReadValue();
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoom * cameraScrollSpeed, minValueZoom, maxValueZoom);

            Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
            Vector3 offset = mouseWorldBefore - mouseWorldAfter;
            offset.z = 0;
            pos = transform.position + offset;

            this.transform.position = MaxLimitsCameraMovement(pos);

            if(Math.Abs(oldZoom - cam.orthographicSize) > 0.01f) UpdateMovementBounds();
        }
    }

    private Vector3 MaxLimitsCameraMovement(Vector3 pos)
    {
        pos.x = Math.Clamp(pos.x, -currentMaxMoveX, currentMaxMoveX);
        pos.y = Math.Clamp(pos.y, -currentMaxMoveY, currentMaxMoveY);

        return pos;
    }

    private void UpdateMovementBounds()
    {
        float zoomNormalized = (cam.orthographicSize - minValueZoom) / (maxValueZoom - minValueZoom);
    
        float maxMultiplier = 5f;
        float minMultiplier = 1f;
        
        float multiplier = Mathf.Lerp(maxMultiplier, minMultiplier, zoomNormalized);
        
        currentMaxMoveX = baseMaxValueMoveX * multiplier;
        currentMaxMoveY = baseMaxValueMoveY * multiplier;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(0, 0, transform.position.z);
        Vector3 size = new Vector3(currentMaxMoveX * 2, currentMaxMoveY * 2, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}
