using System.Battle;
using System.Collections;
using System.Collections.Generic;
using System.Sound;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterObjGenerator : MonoBehaviourPunCallbacks
    {
        private int _teamID;
        private bool _isGenerate = true;
        [SerializeField] private CharacterObjStacker _characterObjStacker;
        [SerializeField] private CharacterPhotonStatus _characterPhotonStatus;
        [SerializeField] private GameObject[] _herosPrefab;
        [SerializeField] private GameObject[] _bigHerosPrefab;
        [SerializeField] private GameObject[] _blockCPrefab;
        private Dictionary<string, (Vector3 offset, GameObject prefab, Vector3 size)> _blockInfoDict;
        [SerializeField] private GameObject _predictCubes;
        [SerializeField] private AnimationClip _generateAnim;
        [SerializeField] private float _motionDelay;
        public bool IsGenerate { get; private set; }
        private Vector3 _insBlockOffset = new(0f, 0.5f, -1f);
        private Vector3 _insBigBlockOffset = new(0f, 1.0f, -2.0f);
        private WaitForSeconds _forSeconds;
        private Coroutine _insCoroutine;
        private BlockGenerator _blockGenerator;
        private BigBlockInsClamper _bigBlockInsClamper;
        [SerializeField] private AudioClip _createBlockClip;
        private SoundHandler _soundHandler;
        private TimeHandler _timeHandler;
        
        private void Start()
        {
            if (!photonView.IsMine) return;
            _soundHandler = SoundHandler.InstanceSoundHandler;
            InitializeVariables();
            InitializeBlockInfoDict();
            _timeHandler = GameObject.FindWithTag("TimeHandler").GetComponent<TimeHandler>();
        }

        private void InitializeVariables()
        {
            _bigBlockInsClamper = new BigBlockInsClamper();
            var animationLength = _generateAnim.length;
            _forSeconds = new WaitForSeconds((animationLength + _motionDelay) * 0.5f);
            _predictCubes = Instantiate(_predictCubes);
            _blockGenerator = GameObject.FindWithTag("BlockGenerator").GetComponent<BlockGenerator>();

            _teamID = _characterPhotonStatus.LocalPlayerTeamID - 1;
            if (_teamID == 1) return;
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
            if (!_timeHandler.IsCountDown) return;
            if (Input.GetMouseButtonDown(0)) CreateBlock();
            MovePredict();
        }

        private void CreateBlock()
        {
            if (!_isGenerate) return;
            if (_characterObjStacker.HasBlock == CharacterObjStacker.NullKey || _insCoroutine != null) return;
            _insCoroutine = StartCoroutine(GenerateBlock());
        }

        private IEnumerator GenerateBlock()
        {
            IsGenerate = true;
            yield return _forSeconds;
            if (_blockInfoDict.TryGetValue(_characterObjStacker.HasBlock, out var values))
            {
                var insPos = RoundPos(_characterObjStacker.HasBlock, transform.position);
                insPos += values.offset;
                _blockGenerator.OtherGenerateObj(1 - _teamID, values.prefab.name, insPos);
                _characterObjStacker.InsBlock();
                _soundHandler.PlaySe(_createBlockClip);
            }
            yield return _forSeconds;
            IsGenerate = false;
            _insCoroutine = null;
        }

        private void MovePredict()
        {
            if (_blockInfoDict.TryGetValue(_characterObjStacker.HasBlock, out var values))
            {
                _predictCubes.SetActive(true);
                var insPos = RoundPos(_characterObjStacker.HasBlock, transform.position);
                insPos += values.offset;
                _predictCubes.transform.position = insPos;
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

        private Vector3 RoundPos(string blockType, Vector3 pos)
        {
            if (blockType == CharacterObjStacker.BlockBigHeros)
            {
                pos.x = _bigBlockInsClamper.ClampValueX(pos.x);
                pos.y = _bigBlockInsClamper.ClampValueY(pos.y);
            }
            else
            {
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
            }
            return pos;
        }
    }
}
