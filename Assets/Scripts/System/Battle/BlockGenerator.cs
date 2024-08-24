using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace System.Battle
{
    public class BlockGenerator : MonoBehaviourPunCallbacks
    {
        private const int MaxGenerate = 3;
        private const int AllGenerate = MaxGenerate * 2;
        [Header("Index-TeamNum")]
        [SerializeField] private GameObject[] _blockTeam;
        private const float BlockInsY = 11.0f;
        public const float Team1PosZ = -2.42f;
        public const float Team2PosZ = 2.42f;
        [SerializeField] private Vector3[] _generatePos = new Vector3[AllGenerate];
        [SerializeField] private Vector3[] _insRot;
        [SerializeField] private Transform[] _parentObj;
        [SerializeField] private float _insInterVal;
        private const int MinPosX = -7;
        private const int MaxPosX = 8;
        private const int FieldSize = 120;
        private const float RayDistance = 1.0f;
        private float _currentTime;
        public int BlocksShareTeam1 => CalcBlocksShare(_parentObj[0]);
        public int BlocksShareTeam2 => CalcBlocksShare(_parentObj[1]);
        [SerializeField] private TimeHandler _timeHandler;
        
        [SerializeField] private GameObject _predictPrefab;
        private readonly GameObject[] _predictObjs = new GameObject[AllGenerate];
        private readonly Vector3[] _predictPos = new Vector3[AllGenerate];
        private const float PredictPosY = 5.8f;
        [SerializeField] private int _toggleCount;
        [SerializeField] private float _toggleTime;
        private WaitForSeconds _toggleWait;
        
        private void Start()
        {
            _toggleWait = new WaitForSeconds(_toggleTime);
            for (var i = 0; i < AllGenerate; i++)
            {
                _predictObjs[i] = Instantiate(_predictPrefab, Vector3.zero, Quaternion.identity);
                _predictPos[i].y = PredictPosY;
            }
            if (!PhotonNetwork.IsMasterClient) return;
            for (var i = 0; i < _generatePos.Length; i++)
                _generatePos[i].y = BlockInsY;
        }

        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (!_timeHandler.IsCountDown) return;
            _currentTime += Time.deltaTime;
            if (_currentTime <= _insInterVal) return;
            SetPositionInfo();
            _currentTime = 0;
        }

        private void SetPositionInfo()
        {
            var teamInsCount = UnityEngine.Random.Range(1,  MaxGenerate + 1);
            var allInsCount = teamInsCount * 2;
            for (var i = 0; i < allInsCount; i++)
            {
                int calcPosX;
                do
                {
                    calcPosX = UnityEngine.Random.Range(MinPosX, MaxPosX);
                } while (Mathf.Approximately(calcPosX, _generatePos[i].x) || ObjectExistsInRay(_generatePos[i], calcPosX));
                _generatePos[i].x = calcPosX;
                _generatePos[i].z = i < teamInsCount ? Team1PosZ : Team2PosZ;
                _predictPos[i].x = _generatePos[i].x;
                _predictPos[i].z = _generatePos[i].z;
            }
            photonView.RPC(nameof(CallGenerate), RpcTarget.All, _predictPos, _generatePos, allInsCount);
        }
        
        [PunRPC]
        private void CallGenerate(Vector3[] predictPos, Vector3[] generatePos, int insCount)
        {
            StartCoroutine(GenerateBlocks(predictPos, generatePos, insCount));
        }

        private IEnumerator GenerateBlocks(Vector3[] predictPos, Vector3[] generatePos, int insCount)
        {
            yield return ToggleActiveObjects(predictPos, insCount);
            
            if (!PhotonNetwork.IsMasterClient) yield break;
            InstantiateBlocks(insCount, generatePos);
        }
        
        private IEnumerator ToggleActiveObjects(Vector3[] predictPos, int insCount)
        {
            for (var i = 0; i < insCount; i++)
                _predictObjs[i].transform.position = predictPos[i];
            
            for (var i = 0; i < _toggleCount; i++)
            {
                var setActive = i % 2 == 0;
                for (var j = 0; j < insCount; j++)
                {
                    _predictObjs[j].SetActive(setActive);
                }
                yield return _toggleWait;
            }
        }
        
        private void InstantiateBlocks(int insCount, Vector3[] generatePos)
        {
            for (var i = 0; i < insCount; i++)
            {
                var teamNum = i < insCount / 2 ? 0 : 1;
                var obj = PhotonNetwork.Instantiate(_blockTeam[teamNum].name, generatePos[i], Quaternion.Euler(_insRot[teamNum]));
                obj.transform.SetParent(_parentObj[teamNum]);
            }
        }
        
        private bool ObjectExistsInRay(Vector3 startPos, int targetPosX)
        {
            startPos.x = targetPosX;
            Debug.DrawRay(startPos, Vector3.down * RayDistance, Color.red, 1f);

            if (!Physics.Raycast(startPos, Vector3.down, out var hit, RayDistance)) return false;
            return hit.collider.CompareTag("Ambras") || hit.collider.CompareTag("Heros");
        }
        
        private int CalcBlocksShare(Transform cubeParent)
        {
            var share = (cubeParent.childCount * 100) / FieldSize;
            return share;
        }

        public void OtherGenerateObj(int teamNum, string target, Vector3 pos)
        {
            photonView.RPC(nameof(ReceiveGenerate), RpcTarget.MasterClient, teamNum, target, pos);
        }
        
        [PunRPC]
        public void ReceiveGenerate(int teamNum, string target, Vector3 pos)
        {
            var obj = PhotonNetwork.Instantiate(target, pos, Quaternion.Euler(_insRot[teamNum]));
            obj.transform.SetParent(_parentObj[teamNum]);
        }
        
    }
}
