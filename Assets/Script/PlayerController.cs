using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walk_speed;
    [SerializeField]
    private float run_speed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float JumpForce;

    // 상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applycrouchPosY;
    // 땅 착지 여부
    private CapsuleCollider capsulecollider;

    // 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 한계 제한
    [SerializeField]
    private float cameraRotaitonLimit;
    private float currentCameraRoationX = 0f;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;

    private Rigidbody rigid;
    void Start()
    {
        capsulecollider = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        applySpeed = walk_speed;
        // 왜 localPostion인가
        // 월드 좌표계 기준  y = 2
        // 플레이어 기준 플레이어 보다 y가 1 이상 있다.
        // 상대적인 변수
        originPosY = theCamera.transform.localPosition.y;
        applycrouchPosY = originPosY; // 기본 서 있는 상태
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
    // 앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    // 실제로 앉기 동작
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
        // y만 변경

        StartCoroutine(CrouchCoroutine());
    }
    // 부드러운 앉기 동작 실행
    // 코루틴 이용 Single Thread -> 여러개 사용
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
                break; // 선형보간 단점 해결
            }
            yield return null; // 1프레임 쉰다.
        }
        theCamera.transform.localPosition = new Vector3(0, applycrouchPosY, 0);
    }
    // 지면 확인하기 점프시 
    private void is_Ground()
    {   // 대각선 or 계단 오차를 제거해주기 위해 bounds.extents.y에 0.1f를 더해준다.
        isGround = Physics.Raycast(transform.position, Vector3.down, capsulecollider.bounds.extents.y + 0.1f);
    }
    // 점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    //실제로 점프
    private void Jump()
    {
        // velocity -> 현재 rigid가 움직이고 있는 방향
        rigid.velocity = transform.up * JumpForce; // transform.up -> (0,1,0)

        // 앉은 상태에서 점프시 앉은 상태 해제
        if (isCrouch)
        {
            Crouch();
        }
    }

    // 달리기 시도
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
    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walk_speed;
    }
    // 실제로 달리는 동작
    private void Running()
    {
        if (isCrouch)
        {
            Crouch();
        }
        isRun = true;
        applySpeed = run_speed;
    }

    // 좌우 캐릭터 회전
    private void PlayerRotation()
    {
        
        float _yRotation = Input.GetAxisRaw("Mouse X");
        // y값 수정
        Vector3 _playerRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_playerRotationY));
    }

    // 상하 카메라 회전
    private void CameraRotation()
    {     
        // x값 수정
        float x_rotation = Input.GetAxisRaw("Mouse Y");
        float Camera_rotation_x = x_rotation * lookSensitivity;
        currentCameraRoationX -= Camera_rotation_x;
        currentCameraRoationX = Mathf.Clamp(currentCameraRoationX, -cameraRotaitonLimit, cameraRotaitonLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRoationX, 0, 0);
    }
    // 움직임 실행
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
