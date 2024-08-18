using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private PlayerItemHandler playerItemHandler;
    private PlayerObjectManipulator playerObjectManipulator;
    //private ItemCBehavior itemCBehavior;
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
    
    private bool _isMoving;
    public bool IsMoving => _isMoving;
    private bool _onGround = true;
    public bool OnGround => _onGround;

    private PlayerCheakAround playerCheakAround;
    [SerializeField] private float stanTime = 2.0f;
    private PlayerStatus _playerStatus;

    // Start is called before the first frame update
    private void Start()
    {
      
         _JumpPower = _initialjumpPower;
         _playerSpeed = _initialSpeed;
         playerItemHandler = GetComponent<PlayerItemHandler>();
         playerCheakAround = GetComponent<PlayerCheakAround>();
        //itemCBehavior = GetComponent<ItemCBehavior>();
         playerObjectManipulator = GetComponent<PlayerObjectManipulator>();
         _playerStatus = GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerCtrl();
        Jump();
        playerItemHandler.UseItemA();
    }

    private void PlayerCtrl()
    {   
        
        //if (_playerObjectManipulator.AnimBreak && _onGround) return;
        float inputX = 0.0f;
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
        int ItemAEffectRate = playerItemHandler.ItemAEffectRate;
        _rb.MovePosition(transform.position + movement * _playerSpeed * Time.deltaTime * ItemAEffectRate );
    }

    public void StanTest()
    {
        if (_playerStatus.CurrentCondition != PlayerStatus.Condition.Stan) return;

        _playerSpeed = 0;
        StartCoroutine(FinishStan());
    }

    private IEnumerator FinishStan()
    {
        yield return new WaitForSeconds(stanTime); //もとに戻す
        _playerSpeed = 1;
    }

    private void Jump()
    {
        _onGround = playerCheakAround.CheakGroundRay();
        if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        {
            //ジャンプSE鳴らす
            //_playerSoundHandler.PlayJumpSE();
            _rb.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
        }
    }
   
}
