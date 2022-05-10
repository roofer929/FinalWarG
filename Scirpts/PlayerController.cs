using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus
{
    Move,
    Run,
    Jump,
    Idle,
    CrouchIdle,
    CrouchMove,
    CrawlIdle,
    CrawlMove,
    Die,

}


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f; // *캐릭터의 이동속도
    [SerializeField]
    private float lookSensitivity = 1.0f; // *캐릭터의 회전 민감도

    [SerializeField]
    private float camRotLimit; //* 카메라 최대 제한 각도
    private float curCamRotX; //* 현재 카메라의 X각도    

    [SerializeField]
    private Camera cam;
    private Rigidbody rigid;

    private PlayerStatus myStatus = PlayerStatus.Idle;
    private Animator myAnimator;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();

        Manager.Input.keyAction -= OnKeyBoard;
        Manager.Input.keyAction += OnKeyBoard;
        Manager.Input.nullKeyAction -= OffKeyBoard;
        Manager.Input.nullKeyAction += OffKeyBoard;
    }

    private void Update()
    {
        AnimationController();
    }

    ///<Summary>
    /// 플레이어의 기본 움직임을 제어합니다.
    ///</Summary>
    private void Move()
    {
        Debug.Log("Move Move");
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        if (_moveDirX == 0 && _moveDirZ == 0)
        {
            Debug.Log("멈춰있는 상태입니다.");
            myStatus = PlayerStatus.Idle;
            return;
        }

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        float speedValue = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //달리기
            //TODO : 속도 증가 , 상태 변경 
            speedValue *= 1.5f;
            myStatus = PlayerStatus.Run;

        }
        else
        {
            myStatus = PlayerStatus.Move;
        }

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speedValue;

        rigid.MovePosition(transform.position + _velocity * Time.deltaTime);


        //TODO : 캐릭터 이동 애니메이션 추가
        //TODO : 캐릭터 움직임 소리 

    }

    ///<Summary>
    /// 플레이어의 애니메이션을 관리 및 재생합니다.
    ///</Summary>
    private void AnimationController()
    {
        switch (myStatus)
        {
            case PlayerStatus.Idle:
            myAnimator.SetBool("Move", false);
                myAnimator.SetBool("Idle", true);

                break;
            case PlayerStatus.Move:
                myAnimator.SetBool("Idle", false);
                if (myAnimator.GetBool("Move"))
                {
                    return;
                }
                myAnimator.SetBool("Move", true);
                break;
            case PlayerStatus.Run:
                myAnimator.SetBool("Idle", false);
                myAnimator.SetTrigger("Move");
                myAnimator.SetTrigger("Run");
                break;
            case PlayerStatus.CrouchIdle:
                break;
            case PlayerStatus.CrouchMove:
                break;
            case PlayerStatus.CrawlIdle:
                break;
            case PlayerStatus.CrawlMove:
                break;
            case PlayerStatus.Die:
                break;

        }
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensitivity;

        curCamRotX -= _cameraRotationX;

        curCamRotX = Mathf.Clamp(curCamRotX, -camRotLimit, camRotLimit);

        cam.transform.localEulerAngles = new Vector3(curCamRotX, 0, 0f);
    }

    private void CharacterRotation()  // 좌우 캐릭터 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_characterRotationY)); // 쿼터니언 * 쿼터니언
                                                                                    // Debug.Log(myRigid.rotation);  // 쿼터니언
                                                                                    // Debug.Log(myRigid.rotation.eulerAngles); // 벡터
    }

    public void OnKeyBoard()
    {
        Move();
        CameraRotation();
        CharacterRotation();
    }

    public void OffKeyBoard()
    {
        myStatus = PlayerStatus.Idle;
    }
}
