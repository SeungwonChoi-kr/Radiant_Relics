using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;

    // 위아래 시선
    private float xRotation = 0f;

    // 부드러운 회전을 위한 변수
    private float currentYRotation;
    private float yRotationVelocity;

    void Start()
    {
        if (playerBody != null)
        {
            currentYRotation = playerBody.eulerAngles.y;
        }
    }

    void Update()
    {
        // GameManager의 isMouseOn이 켜져있으면 카메라 회전을 멈춤
        if (GameManager.Instance.isMouseOn)
        {
            return;
        }

        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * GameManager.Instance.mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * GameManager.Instance.mouseSensitivity;

        // 위아래 시선
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 시선 각도 제한

        // 카메라의 X축 회전 적용
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 좌우 시선
        if (playerBody != null)
        {
            currentYRotation += mouseX;
            float smoothYRotation = Mathf.SmoothDampAngle(playerBody.eulerAngles.y, currentYRotation, ref yRotationVelocity, GameManager.Instance.rotationSmoothTime);  // 부드러운 화면 전환
            playerBody.rotation = Quaternion.Euler(0f, smoothYRotation, 0f);
        }
    }
}
