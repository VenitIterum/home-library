using UnityEngine;
using UnityEngine.InputSystem;

public class BookClickRaycast : MonoBehaviour
{
    private BookInputActions input;
    private Camera cam;

    void Awake()
    {
        input = new BookInputActions();
        cam = Camera.main;
    }

    void OnEnable()
    {
        input.Book.Enable();
        input.Book.Click.performed += OnClick;
    }

    void OnDisable()
    {
        input.Book.Click.performed -= OnClick;
        input.Book.Disable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider == null)
            return;

        OpenBookWindow book = hit.collider.GetComponent<OpenBookWindow>();
        if (book != null)
        {
            book.OnBookClicked();
        }
    }
}
