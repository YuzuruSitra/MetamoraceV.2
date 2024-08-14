using Photon.Pun;
using UnityEngine;

namespace System.Battle
{
    public class BlockGenerator : MonoBehaviour
    {
        [Header("Index-TeamNum")]
        [SerializeField] private GameObject[] _blockTeam;
        [SerializeField] private Vector3[] _insPos;
        [SerializeField] private Transform[] _parentObj;
        [SerializeField] private float _insInterVal;
        private const int MinPosX = -7;
        private const int MaxPosX = 8;
        private const int FieldSize = 120;
        private const float RayDistance = 1.0f;
        private float _currentTime;
        private const int MaxGenerate = 3;
        public int BlocksShareTeam1 => CalcBlocksShare(_parentObj[0]);
        public int BlocksShareTeam2 => CalcBlocksShare(_parentObj[1]);
        
        [SerializeField] private TimeHandler _timeHandler;

        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (_timeHandler.CountTime > 0) return;
            if (_timeHandler.BattleTime <= 0) return;
            _currentTime += Time.deltaTime;
            if (_currentTime <= _insInterVal) return;
            InsBlock();
            _currentTime = 0;
        }

        private void InsBlock()
        {
            var insCount = UnityEngine.Random.Range(1,  MaxGenerate + 1);
            for (var i = 0; i < _blockTeam.Length; i++)
            {
                for (var j = 0; j < insCount; j++)
                {
                    int calcPosX;
                    do
                    {
                        calcPosX = UnityEngine.Random.Range(MinPosX, MaxPosX);
                    } while (Mathf.Approximately(calcPosX, _insPos[i].x) || ObjectExistsInRay(_insPos[i], calcPosX));

                    _insPos[i].x = calcPosX;
                    var obj = PhotonNetwork.Instantiate(_blockTeam[i].name, _insPos[i], Quaternion.identity);
                    obj.transform.SetParent(_parentObj[i]);
                }
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
    }
}
