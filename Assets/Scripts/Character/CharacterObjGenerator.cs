using System.Battle;
using System.Collections;
using System.Collections.Generic;
using System.Network;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterObjGenerator : MonoBehaviourPunCallbacks
    {
        private int _teamID;
        private bool _isGenerate = true;
        [SerializeField] private CharacterObjStacker _characterObjStacker;
        [SerializeField] private GameObject[] _herosPrefab;
        [SerializeField] private GameObject[] _bigHerosPrefab;
        [SerializeField] private GameObject[] _blockCPrefab;
        private Dictionary<string, (Vector3 offset, GameObject prefab, Vector3 size)> _blockInfoDict;
        [SerializeField] private GameObject _predictCubes;
        [SerializeField] private AnimationClip _generateAnim;
        [SerializeField] private float _motionDelay;
        public bool IsGenerate { get; private set; }
        private Vector3 _insBlockOffset = new(0f, 0.25f, -1f);
        private Vector3 _insBigBlockOffset = new(0f, 1.25f, -1.5f);
        private WaitForSeconds _forSeconds;
        private Coroutine _insCoroutine;
        private BlockGenerator _blockGenerator;
        
        private void Start()
        {
            if (!photonView.IsMine) return;
            InitializeVariables();
            InitializeBlockInfoDict();
        }

        private void InitializeVariables()
        {
            var animationLength = _generateAnim.length;
            _forSeconds = new WaitForSeconds(animationLength + _motionDelay);
            _predictCubes = Instantiate(_predictCubes);
            _blockGenerator = GameObject.FindWithTag("BlockGenerator").GetComponent<BlockGenerator>();
            if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
            _teamID = (int)teamId;
            if (_teamID != 1) return;
            _insBlockOffset.z *= -1;
            _insBigBlockOffset.z *= -1;
        }

        private void InitializeBlockInfoDict()
        {
            _blockInfoDict = new Dictionary<string, (Vector3 offset, GameObject prefab, Vector3 size)>
            {
                { "Heros", (_insBlockOffset, _herosPrefab[_teamID], _herosPrefab[_teamID].transform.localScale) },
                { "BigHeros", (_insBigBlockOffset, _bigHerosPrefab[_teamID], _bigHerosPrefab[_teamID].transform.localScale) },
                { "ItemCBlock", (_insBlockOffset, _blockCPrefab[_teamID], _blockCPrefab[_teamID].transform.localScale) }
            };
        }

        private void Update()
        {
            if (!photonView.IsMine) return; 
            if (Input.GetMouseButtonDown(0)) CreateBlock();
            MovePredict();
        }

        private void CreateBlock()
        {
            if (!_isGenerate) return;
            if (_characterObjStacker.HasBlock == CharacterObjStacker.NullKey || _insCoroutine != null) return;
            _insCoroutine = StartCoroutine(GenerateBlock());
            _characterObjStacker.InsBlock();
        }

        private IEnumerator GenerateBlock()
        {
            IsGenerate = true;
            yield return _forSeconds;
            if (_blockInfoDict.TryGetValue(_characterObjStacker.HasBlock, out var values))
            {
                var insPos = transform.position + values.offset;
                _blockGenerator.OtherGenerateObj(_teamID,values.prefab.name, insPos);
            }
            IsGenerate = false;
            _insCoroutine = null;
        }

        private void MovePredict()
        {
            if (_blockInfoDict.TryGetValue(_characterObjStacker.HasBlock, out var values))
            {
                _predictCubes.SetActive(true);
                _predictCubes.transform.position = transform.position + values.offset;
                _predictCubes.transform.localScale = values.size;
            }
            else
            {
                _predictCubes.SetActive(false);
            }
        }
        
        public void SetGenerateBool(bool isGenerate)
        {
            _isGenerate = isGenerate;
        }
    }
}
