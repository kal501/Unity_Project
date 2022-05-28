using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ���ǵ� ���� ����
    [SerializeField]
    private float walk_speed;
    [SerializeField]
    private float run_speed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float JumpForce;

    // ���� ����
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applycrouchPosY;
    // �� ���� ����
    private CapsuleCollider capsulecollider;

    // �ΰ���
    [SerializeField]
    private float lookSensitivity;

    // ī�޶� �Ѱ� ����
    [SerializeField]
    private float cameraRotaitonLimit;
    private float currentCameraRoationX = 0f;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;

    private Rigidbody rigid;
    void Start()
    {
        capsulecollider = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        applySpeed = walk_speed;
        // �� localPostion�ΰ�
        // ���� ��ǥ�� ����  y = 2
        // �÷��̾� ���� �÷��̾� ���� y�� 1 �̻� �ִ�.
        // ������� ����
        originPosY = theCamera.transform.localPosition.y;
        applycrouchPosY = originPosY; // �⺻ �� �ִ� ����
    }

    
    
    void Update()
    {
        is_Ground();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        PlayerRotation();
    }
    // �ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    // ������ �ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;
        // if(isCrouch) isCrouch = false;
        // else isCrouch = true;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applycrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walk_speed;
            applycrouchPosY = originPosY;
        }
        // theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applycrouchPosY, theCamera.transform.localPosition.z);
        // y�� ����

        StartCoroutine(CrouchCoroutine());
    }
    // �ε巯�� �ɱ� ���� ����
    // �ڷ�ƾ �̿� Single Thread -> ������ ���
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(_posY != applycrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applycrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 15)
            {
                break; // �������� ���� �ذ�
            }
            yield return null; // 1������ ����.
        }
        theCamera.transform.localPosition = new Vector3(0, applycrouchPosY, 0);
    }
    // ���� Ȯ���ϱ� ������ 
    private void is_Ground()
    {   // �밢�� or ��� ������ �������ֱ� ���� bounds.extents.y�� 0.1f�� �����ش�.
        isGround = Physics.Raycast(transform.position, Vector3.down, capsulecollider.bounds.extents.y + 0.1f);
    }
    // ���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    //������ ����
    private void Jump()
    {
        // velocity -> ���� rigid�� �����̰� �ִ� ����
        rigid.velocity = transform.up * JumpForce; // transform.up -> (0,1,0)

        // ���� ���¿��� ������ ���� ���� ����
        if (isCrouch)
        {
            Crouch();
        }
    }

    // �޸��� �õ�
    private void TryRun()
    {
        // Shift key -> Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    // �޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walk_speed;
    }
    // ������ �޸��� ����
    private void Running()
    {
        if (isCrouch)
        {
            Crouch();
        }
        isRun = true;
        applySpeed = run_speed;
    }

    // �¿� ĳ���� ȸ��
    private void PlayerRotation()
    {
        
        float _yRotation = Input.GetAxisRaw("Mouse X");
        // y�� ����
        Vector3 _playerRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_playerRotationY));
    }

    // ���� ī�޶� ȸ��
    private void CameraRotation()
    {     
        // x�� ����
        float x_rotation = Input.GetAxisRaw("Mouse Y");
        float Camera_rotation_x = x_rotation * lookSensitivity;
        currentCameraRoationX -= Camera_rotation_x;
        currentCameraRoationX = Mathf.Clamp(currentCameraRoationX, -cameraRotaitonLimit, cameraRotaitonLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRoationX, 0, 0);
    }
    // ������ ����
    private void Move()
    {
        float move_x = Input.GetAxisRaw("Horizontal");
        float move_z = Input.GetAxisRaw("Vertical");

        Vector3 move_Horizontal = transform.right * move_x; // (1,0,0) 
        Vector3 move_Vertical = transform.forward * move_z; // (0,0,1)

        Vector3 player_velocity = (move_Horizontal + move_Vertical).normalized * applySpeed; //  

        rigid.MovePosition(transform.position + player_velocity * Time.deltaTime);
    }
}
