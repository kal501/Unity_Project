using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5.0f;   // �̵� �ӵ�
    private Vector3 moveDirection;    // �̵� ����
    private float gravity = -9.81f; // �߷� ���

    [SerializeField]
    private float jumpForce = 3.0f; // ���� ��

    [SerializeField]
    private Transform cameraTransform; // ī�޶� transform ������Ʈ
    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
                  
    void Update()
    {
        if (characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime; // �߷��� �޴� �� 
            // y���� ������ �����ϰ� �ǰ�
            // moveDirection.y == 0�� ������ ������ �� �ְ����� �ȴ�.
        }

        characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        // moveDirection = direction;
        // moveDirection = new Vector3(direction.x, moveDirection.y, direction.z); // y���� �߷��� �޾� �Ʒ��� �������� ������
        // x��� z���� �̵����� �߷��� ���� �ʾƾ� �մϴ�.

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

