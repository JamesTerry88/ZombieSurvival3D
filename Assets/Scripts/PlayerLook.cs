using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 50f;
    public Transform playerBody;

    float xRotation = 0f;
    bool isCursorLocked = true;

    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = !isCursorLocked;
            LockCursor(isCursorLocked);
        }

        if (!isCursorLocked) return;

        float rawX = Mathf.Clamp(Input.GetAxisRaw("Mouse X"), -1f, 1f);
        float rawY = Mathf.Clamp(Input.GetAxisRaw("Mouse Y"), -1f, 1f);

        float mouseX = rawX * mouseSensitivity * Time.deltaTime;
        float mouseY = rawY * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void LockCursor(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
