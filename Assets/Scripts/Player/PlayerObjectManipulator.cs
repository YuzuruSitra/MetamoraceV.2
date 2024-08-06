using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectManipulator : MonoBehaviour
{
    private BlockBehaviour _currentBlock;
    PlayerCheakAround playerCheakAround;
    PlayerBlockStack playerBlockStack;
     [SerializeField]
    private float _initialDestroyPower = 1.0f;
    public float InitialDestroyPower => _initialDestroyPower;
    [SerializeField]
    private GameObject _predictCubes;
    private float _destroyPower = 1;
     private bool _hasBlock = false;
    public bool HasBlock => _hasBlock;
     private Vector3 _insPos;
    private Vector3 _insBigPos;
   // private Vector3 PlayerDirection;
    private UIHandler _uiHandler;
    private PlayerItemHandler playerItemHandler;
    [SerializeField]
    GameObject[] _herosPrefab = new GameObject[2];

    private Vector3 _createPos;
    // Start is called before the first frame update
    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        playerCheakAround = GetComponent<PlayerCheakAround>();
        playerBlockStack = GetComponent<PlayerBlockStack>();
        playerItemHandler = GetComponent<PlayerItemHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        BreakBlock();
        CreateBlock();
        //playerItemHandler.CreateItem();
        if (_hasBlock) 
        {
            _insPos = new Vector3((int)transform.position.x, (int)transform.position.y + 0.25f, -1.0f);
             _createPos = _insPos;

            _predictCubes.transform.position = _insPos;
        }
    }
     //ブロック生成
    public void CreateBlock()
    {
        //ブロックを持ってれば処理を行う
        if (!_hasBlock) return;
        if (!Input.GetMouseButtonDown(0)) return;
        _hasBlock = false;
        _uiHandler.ResetBlockImage();
        _predictCubes.SetActive(false);
        _insBigPos = _insPos;
        _insBigPos.y += 1.0f;
      
        StartCoroutine(GenerateBlock());
    }
     private IEnumerator GenerateBlock()
    {
        yield return new WaitForSeconds(0.4f);
        Instantiate(_herosPrefab[0], _createPos, Quaternion.identity); // _createPosを使用してオブジェクトを生成        // //アイテムBを持っていたら巨大ブロック一回だけ生成
        // if (_itemHandler._HasItemB && _playerItemHandler.UseItemState == 0)
        // {
        //     //アイテムB微調整
        //     _playerItemHandler.UsedItem();
        //     _itemHandler.ItemEffectB();
        // }
        // //ItemCBlock生成
        // else if (_itemHandler._HasItemC && _playerItemHandler.UseItemState == 1)
        // {
        //     _playerItemHandler.UsedItem();
        // }
        // else
        // {
        //    
        // }

    }
    //オブジェクト破壊
    public void BreakBlock()
    {
        if (_hasBlock || !Input.GetMouseButton(0)) return; 
        Vector3 playerDirection = transform.forward;
        playerDirection.Normalize();
        _currentBlock = playerCheakAround.CheckBlockRay(playerDirection);
        if(_currentBlock == null)return;
        string BreakObjName = _currentBlock.DestroyBlock(_destroyPower);
        if (BreakObjName == "Ambras" || BreakObjName == "Heros")
        {
            playerBlockStack.StackBlock(BreakObjName);
            _hasBlock = true;
            //次に生成するブロックを表示する処理
            _uiHandler.BlockImage(BreakObjName);
            //壊したブロックを表示する処理
            _uiHandler.SetStackImage(BreakObjName);
            _predictCubes.SetActive(true);
        }
    }

}
