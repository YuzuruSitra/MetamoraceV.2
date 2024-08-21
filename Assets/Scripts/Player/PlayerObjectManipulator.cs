using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Block;
//using Block;
using Photon.Pun;

public class PlayerObjectManipulator : MonoBehaviourPunCallbacks
{
    private BlockBase _currentBlockBehavior;
    [SerializeField] private PlayerCheakAround playerCheakAround;
    [SerializeField] private PlayerBlockStack playerBlockStack;
    [SerializeField] private float _initialDestroyPower = 1.0f;
    public float InitialDestroyPower => _initialDestroyPower;
    [SerializeField] private GameObject _predictCubes;
    private float _destroyPower = 1.0f;
    private bool _hasBlock = false;
    public bool HasBlock => _hasBlock;
    private Vector3 _insPos;

    private Vector3 _insBigPos;

    // private Vector3 PlayerDirection;
    private UIHandler _uiHandler;
    [SerializeField] private PlayerItemHandler playerItemHandler;
    [SerializeField] GameObject[] _herosPrefab = new GameObject[2];
    [SerializeField] GameObject[] _BigPrefab = new GameObject[2];
    [SerializeField] GameObject[] _cPrefab = new GameObject[2];
    string nextInsBlock = "Heros";
    public bool IsStan { get; private set; } = false;
    private bool breaking = false;
    public bool Breaking => breaking;
    private bool swing = false;
    public bool Swing => swing;
    private Animator _anim;
    private float _swingAnimTime;
    private float _breakAnimTime;

    private float _breakTime;

    // Start is called before the first frame update
    private void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        //playerCheakAround = GetComponent<PlayerCheakAround>();
        //playerBlockStack = GetComponent<PlayerBlockStack>();
        //playerItemHandler = GetComponent<PlayerItemHandler>();
        _anim = GetComponent<Animator>();
        _swingAnimTime = GetAnimationClipLength(_anim.runtimeAnimatorController.animationClips,
            "swing");
        _breakAnimTime = GetAnimationClipLength(_anim.runtimeAnimatorController.animationClips,
            "break");
        //Debug.Log(_swingAnimTime);
    }

    private static float GetAnimationClipLength(IEnumerable<AnimationClip> animationClips, string clipName)
    {
        return (from animationClip in animationClips
            where animationClip.name == clipName
            select animationClip.length).FirstOrDefault();
    }

    // Update is called once per frame
    private void Update()
    {
        // Debug.Log("Breaking" + breaking);
        // Debug.Log("Swing" + swing);
        BreakBlock();
        CreateBlock();
        playerItemHandler.UseItemC();
        playerItemHandler.UseItemB();
        //Debug.Log(swing);
        if (!_hasBlock) return;
        _insPos = new Vector3((int)transform.position.x, (int)transform.position.y + 0.25f, -1.0f);
        _predictCubes.transform.position = _insPos;
    }

    //ブロック生成
    public void CreateBlock()
    {
        //ブロックを持ってれば処理を行う
        if (_hasBlock && Input.GetMouseButtonDown(0))
        {
            swing = true;
            _hasBlock = false;
            _uiHandler.ResetBlockImage();
            _predictCubes.SetActive(false);
            _insBigPos = _insPos;
            _insBigPos.y += 1.0f;
            StartCoroutine(GenerateBlock());
            StartCoroutine(GenerateAnimDelay());
        }
    }

    private IEnumerator GenerateBlock()
    {
        yield return new WaitForSeconds(0.4f);
        if (playerItemHandler.NextInsBlock != null) nextInsBlock = playerItemHandler.NextInsBlock;
        else nextInsBlock = "Heros";
        switch (nextInsBlock)
        {
            case "Heros":
                Instantiate(_herosPrefab[0], _insPos, Quaternion.identity); // _createPosを使用してオブジェクトを生成
                break;
            case "BigBlock":
                // //アイテムBを持っていたら巨大ブロック一回だけ生成
                Instantiate(_BigPrefab[0], _insBigPos, Quaternion.identity);
                playerItemHandler.ResetNextInsBlock();
                break;
            case "ItemCBlock":
                Instantiate(_cPrefab[0], _insBigPos, Quaternion.identity);
                playerItemHandler.ResetNextInsBlock();
                break;
        }

        nextInsBlock = "Heros";
    }

    private IEnumerator GenerateAnimDelay()
    {
        yield return new WaitForSeconds(_swingAnimTime);
        swing = false;
        //Debug.Log("Swing");
    }

    //オブジェクト破壊
    public void BreakBlock()
    {
        if (!_hasBlock && Input.GetMouseButton(0))
        {
            Vector3 playerDirection = transform.forward;
            playerDirection.Normalize();
            _currentBlockBehavior = playerCheakAround.CheckBlockRay(playerDirection);
            if (_currentBlockBehavior == null) return;
            Debug.Log("TOutch");
            breaking = true;
            int ItemAEffectRate = playerItemHandler.ItemAEffectRate;
            _destroyPower = ItemAEffectRate * _initialDestroyPower;
            string BreakObjName = _currentBlockBehavior.DestroyBlock(_destroyPower, this.gameObject);
            if (BreakObjName != "Ambras" && BreakObjName != "Heros" && BreakObjName != "ItemC") return;
            playerBlockStack.StackBlock(BreakObjName);
            _hasBlock = true;
                Debug.Log(BreakObjName);
                //次に生成するブロックを表示する処理
                _uiHandler.SetBlockImage("Ambras");
                //壊したブロックを表示する処理
                _uiHandler.SetStackImage(BreakObjName);
                _predictCubes.SetActive(true);
                playerItemHandler.CreateItem();
                // StartCoroutine(BreakAnimDelay());
            
        }
        else breaking = false;
    }
    // private IEnumerator BreakAnimDelay()
    // {
    //     yield return new WaitForSeconds(_breakAnimTime);
    //     breaking = false;
    //     Debug.Log("Swing");
    // }
}