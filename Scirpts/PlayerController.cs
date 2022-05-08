using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float lookSensitivity = 1.0f;

    [SerializeField]
    private float camRotLimit;
    private float curCamRotX;
    private float curCamRotY;

    [SerializeField]
    private Camera cam;
    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        Manager.Input.keyAction -= OnKeyBoard;
        Manager.Input.keyAction += OnKeyBoard;
    }


    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        rigid.MovePosition(transform.position + _velocity * Time.deltaTime);

        //TODO : 캐릭터 이동 애니메이션 추가
        //TODO : 캐릭터 움직임 소리 

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
}
