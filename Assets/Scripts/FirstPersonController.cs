using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float Walkspeed = 5f;
    public float SprintMultiplier = 2f;
    public float Jumpforce = 5;
    public float GroundCheckDistance = 1.5f;
    public float LookSensitivityX = 1f;
    public float LookSensitivityY = 1f;
    public float MinYLookAngle = -90f;
    public float MaxYLookAngle = 90f;
    public Transform PlayerCamera;
    public float Gravity = -9.81f;
    private Vector3 velocity;
    private float verticleRotation = 0f;
    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        moveDirection.Normalize();

        float speed = Walkspeed;
        if(Input.GetAxis("Sprint") > 0)
        {
            speed *= SprintMultiplier;
        }

        characterController.Move(moveDirection * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = Jumpforce;
        }
        else
        {
            velocity.y += Gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);

        if(PlayerCamera != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY;

            verticleRotation -= mouseY;
            verticleRotation = Mathf.Clamp(verticleRotation, MinYLookAngle, MaxYLookAngle);

            PlayerCamera.localRotation = Quaternion.Euler(verticleRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, GroundCheckDistance))
        {
            return true;
        }
        return false;
    }

}
