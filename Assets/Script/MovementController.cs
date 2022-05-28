using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5.0f;   // 이동 속도
    private Vector3 moveDirection;    // 이동 방향
    private float gravity = -9.81f; // 중력 계수

    [SerializeField]
    private float jumpForce = 3.0f; // 점프 힘

    [SerializeField]
    private Transform cameraTransform; // 카메라 transform 컴포넌트
    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
                  
    void Update()
    {
        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime; // 중력을 받는 식 
            // y값이 서서히 감소하게 되고
            // moveDirection.y == 0인 지점이 점프할 때 최고점이 된다.
        }

        characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        // moveDirection = direction;
        // moveDirection = new Vector3(direction.x, moveDirection.y, direction.z); // y축은 중력을 받아 아래로 내려가고 있지만
        // x축과 z축의 이동에서 중력은 받지 않아야 합니다.

        Vector3 movedis = cameraTransform.rotation * direction;
        moveDirection = new Vector3(movedis.x, moveDirection.y, movedis.z);
    }

    public void JumpTo()
    {
        if (characterController.isGrounded == true)
        {
            moveDirection.y = jumpForce;
        }
    }
}

