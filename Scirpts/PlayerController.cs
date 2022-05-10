using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ActionStatus
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
    private ActionStatus myStatus = ActionStatus.Idle; //* 플레이어 행동 상태
    private Animator myAnimator;
    private event Action OnStatusChanged; //* 플레이어 행동 상태가 변경되었을 때 호출되는 이벤트


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();

        Manager.Input.keyAction -= OnKeyBoard;
        Manager.Input.keyAction += OnKeyBoard;
        Manager.Input.nullKeyAction -= OffKeyBoard;
        Manager.Input.nullKeyAction += OffKeyBoard;

        OnStatusChanged += AnimationController;
    }

    ///<Summary>
    /// 플레이어의 기본 움직임을 제어합니다.
    ///</Summary>
    private void Move()
    {        
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        if (_moveDirX == 0 && _moveDirZ == 0)
        {
            Debug.Log("멈춰있는 상태입니다.");
            ChangePlayerStatus(ActionStatus.Idle);
            return;
        }

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        float speedValue = speed;

        //* 달리기
        if (Input.GetKey(KeyCode.LeftShift))
        {                     
            speedValue *= 2f;            
            ChangePlayerStatus(ActionStatus.Run);
        }
        //* 일반 이동
        else
        {
            ChangePlayerStatus(ActionStatus.Move);
        }

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speedValue;

        rigid.MovePosition(transform.position + _velocity * Time.deltaTime);


        //TODO : 캐릭터 이동 애니메이션 추가
        //TODO : 캐릭터 움직임 소리 

    }


    ///<Summary>
    /// 플레이어의 동적 상태를 변경합니다.
    ///</Summary>
    public void ChangePlayerStatus(ActionStatus status)
    {
        myStatus = status;
        OnStatusChanged();
    }

    ///<Summary>
    /// 플레이어의 애니메이션을 관리 및 재생합니다.
    ///</Summary>
    private void AnimationController()
    {
        switch (myStatus)
        {
            case ActionStatus.Idle:
                myAnimator.SetBool("Move", false);
                myAnimator.SetBool("Idle", true);
                myAnimator.SetBool("Run", false);

                break;
            case ActionStatus.Move:
                myAnimator.SetBool("Idle", false);                
                myAnimator.SetBool("Move", true);
                myAnimator.SetBool("Run", false);
                break;
            case ActionStatus.Run:
                myAnimator.SetBool("Idle", false);
                myAnimator.SetBool("Move", false);
                myAnimator.SetBool("Run", true);
                break;
            case ActionStatus.CrouchIdle:
                break;
            case ActionStatus.CrouchMove:
                break;
            case ActionStatus.CrawlIdle:
                break;
            case ActionStatus.CrawlMove:
                break;
            case ActionStatus.Die:
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
    }

    public void OnKeyBoard()
    {
        Move();
        CameraRotation();
        CharacterRotation();
    }

    public void OffKeyBoard()
    {
        Debug.Log("키보드 입력 없음");
        myStatus = ActionStatus.Idle;
    }
}
