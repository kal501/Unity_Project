using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerVer2 : MonoBehaviour
{
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    private MovementController movement;
    [SerializeField] // �ܺο��� �� ���� �����Ű�� ����
    private CameraController cameraController;

    // Start is called before the first frame update
    private void Awake()
    {
        movement = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        movement.MoveTo(new Vector3(x, 0, z));

        if (Input.GetKeyDown(jumpKeyCode))
        {
            movement.JumpTo();
        }

        float mouseX = Input.GetAxisRaw("Mouse X"); // ���콺�� ��/�� ������
        float mouseY = Input.GetAxisRaw("Mouse Y"); // ���콺�� ��/�Ʒ� ������

        cameraController.RotateTo(mouseX, mouseY);
        
    }
}
