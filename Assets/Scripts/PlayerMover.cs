using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
     [SerializeField]
    private Rigidbody _rb;
     [SerializeField]
    private float _initialSpeed = 5.0f;
    public float InitialSpeed => _initialSpeed;
    private float _playerSpeed;
    [SerializeField]
    private float _frontRayRength = 0.51f;
    [SerializeField]
    private float _initialjumpPower = 30.0f;
    private float _JumpPower;
    [SerializeField]
    private float _jumprayrength = 0.15f;
    private bool _isMoving;
    public bool IsMoving => _isMoving;
    private bool _onGround = true;
    public bool OnGround => _onGround;
    // Start is called before the first frame update
    void Start()
    {
         _JumpPower = _initialjumpPower;
         _playerSpeed = _initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCtrl();
        Jump();
    }

   void PlayerCtrl()
    {   
        
        //if (_playerObjectManipulator.AnimBreak && _onGround) return;
        float inputX = 0.0f;
        Debug.Log(inputX);
        //チーム1とチーム2で操作反転
        // if (_playerDataReceiver.MineTeamID == 0)
        // {
            if (Input.GetKey("d")) inputX = 1.0f;
            if (Input.GetKey("a")) inputX = -1.0f;
        //}
        // else
        // {
        //     if (Input.GetKey("d")) inputX = -1.0f;
        //     if (Input.GetKey("a")) inputX = 1.0f;
        // }
        if (Input.GetKey("a") && Input.GetKey("d")) inputX = 0.0f;
        _isMoving = true;

        if (inputX == 0)
        {
            _isMoving = false;
            return;
        }
        
        Vector3 movement = new Vector3(inputX, 0, 0);
        // プレイヤーの向きを移動ベクトルに向ける
        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.left);

       transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1280.0f * Time.deltaTime);
       // if (CheckFront(new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward * _frontRayRength))) return;
        // カメラの方向を考慮して移動ベクトルを作成
       
        _rb.MovePosition(transform.position + movement * _playerSpeed * Time.deltaTime);
    }

    void Jump()
    {
         float raypos = 0.45f;
        float rayheight = 0.1f;
         _onGround = CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, -raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, -raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, raypos), Vector3.down));
        // if(_animSwing) return;
        if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        {
            Debug.Log("Jump");
            //ジャンプSE鳴らす
            //_playerSoundHandler.PlayJumpSE();
            _rb.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
        }
    }
    // 地上にいるか判定
    private bool CheckAndJump(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * _jumprayrength, Color.red, 0.1f);
        if (Physics.Raycast(ray, out hit, _jumprayrength)) return true; 
        else return false;
    }
}
