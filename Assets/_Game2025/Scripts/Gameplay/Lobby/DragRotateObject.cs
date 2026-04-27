using UnityEngine;

public class DragRotateObject : MonoBehaviour
{
    public float rotationSpeed = 1f;
    private float targetRotationX, targetRotationY;
    private float currentRotationX, currentRotationY;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            targetRotationX = 0;
            targetRotationY = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            targetRotationX += deltaMousePosition.x * rotationSpeed * Time.deltaTime;
            targetRotationY += deltaMousePosition.y * rotationSpeed * Time.deltaTime;

            targetRotationX = Mathf.Clamp(targetRotationX, -50f, 50f);
            targetRotationY = Mathf.Clamp(targetRotationY, -50f, 50f);
        }

        currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, Time.deltaTime * rotationSpeed);
        currentRotationY = Mathf.Lerp(currentRotationY, targetRotationY, Time.deltaTime * rotationSpeed);

        transform.rotation = Quaternion.Euler(currentRotationY, currentRotationX, transform.rotation.eulerAngles.z);
    }
}