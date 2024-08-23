using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Block;
using UnityEngine;

namespace NPC
{
    public class NPCObjectmanipulater : MonoBehaviour
    {
        [SerializeField] private bool DebugMode;
        private BlockBase _currentBlockBehavior;
        private BlockBehaviour _currentBlockBehavior1;
        [SerializeField] private NPCCheackAround _npcCheackAround;
        [SerializeField] private NPCBlockStack _npcBlockStack;
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
        [SerializeField] private NPCItemHandler _npcItemHandler;
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
            //if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
            //_uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
            _anim = GetComponent<Animator>();
            _swingAnimTime = GetAnimationClipLength(_anim.runtimeAnimatorController.animationClips,
                "swing");
            _breakAnimTime = GetAnimationClipLength(_anim.runtimeAnimatorController.animationClips,
                "break");
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
            if (!_hasBlock) return;
            _insPos = new Vector3((int)transform.position.x, (int)transform.position.y + 0.25f, -1.0f);
            _predictCubes.transform.position = _insPos;
        }

        //ブロック生成
        public void CreateBlock()
        {
            //ブロックを持ってれば処理を行う

            swing = true;
            _hasBlock = false;
            //_uiHandler.ResetBlockImage();
            _predictCubes.SetActive(false);
            _insBigPos = _insPos;
            _insBigPos.y += 1.0f;
            StartCoroutine(GenerateBlock());
            StartCoroutine(GenerateAnimDelay());
        }

        private IEnumerator GenerateBlock()
        {
            yield return new WaitForSeconds(0.4f);
            if (_npcItemHandler.NextInsBlock != null) nextInsBlock = _npcItemHandler.NextInsBlock;
            else nextInsBlock = "Heros";
            switch (nextInsBlock)
            {
                case "Heros":
                    Instantiate(_herosPrefab[0], _insPos, Quaternion.identity); // _createPosを使用してオブジェクトを生成
                    break;
                case "BigBlock":
                    // //アイテムBを持っていたら巨大ブロック一回だけ生成
                    Instantiate(_BigPrefab[0], _insBigPos, Quaternion.identity);
                    _npcItemHandler.ResetNextInsBlock();
                    break;
                case "ItemCBlock":
                    Instantiate(_cPrefab[0], _insBigPos, Quaternion.identity);
                    _npcItemHandler.ResetNextInsBlock();
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
            if (!_hasBlock)
            {
                if (DebugMode)
                {
                    _currentBlockBehavior1 = _npcCheackAround.CheckBlockRay1();
                    if (_currentBlockBehavior1 == null) return;
                    breaking = true;
                    //int ItemAEffectRate = _npcItemHandler.ItemAEffectRate;
                    _destroyPower = _initialDestroyPower;
                    string BreakObjName = _currentBlockBehavior1.DestroyBlock(_destroyPower);
                    if (BreakObjName != "Ambras" && BreakObjName != "Heros" && BreakObjName != "ItemCBlock") return;
                    _npcBlockStack.StackBlock(BreakObjName);
                    _hasBlock = true;
                    //次に生成するブロックを表示する処理
                    //_uiHandler.SetBlockImage("Ambras");
                    //壊したブロックを表示する処理
                    //_uiHandler.SetStackImage(BreakObjName);
                    _predictCubes.SetActive(true);
                    //_npcItemHandler.CreateItem();
                    // StartCoroutine(BreakAnimDelay());
                }
                else
                {
                    _currentBlockBehavior = _npcCheackAround.CheckBlockRay();
                    if (_currentBlockBehavior == null) return;
                    breaking = true;
                    //int ItemAEffectRate = _npcItemHandler.ItemAEffectRate;
                    _destroyPower = _initialDestroyPower;
                    string BreakObjName = _currentBlockBehavior.DestroyBlock(_destroyPower, this.gameObject);
                    if (BreakObjName != "Ambras" && BreakObjName != "Heros" && BreakObjName != "ItemCBlock") return;
                    _npcBlockStack.StackBlock(BreakObjName);
                    _hasBlock = true;
                    //次に生成するブロックを表示する処理
                    //_uiHandler.SetBlockImage("Ambras");
                    //壊したブロックを表示する処理
                    //_uiHandler.SetStackImage(BreakObjName);
                    _predictCubes.SetActive(true);
                    //_npcItemHandler.CreateItem();
                    // StartCoroutine(BreakAnimDelay());
                }
            }
            else breaking = false;
        }
    }
}