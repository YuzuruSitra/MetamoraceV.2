using System.Battle;
using System.Network;
using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.Sound;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace System.UI
{
    public class ResultPanelHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameResultHandler _gameResultHandler;
        [SerializeField] private BlockGenerator _blockGenerator;
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private GameObject _winLoosePanel;
        [SerializeField] private GameObject _drawPanel;
        [SerializeField] private Image _winTeamImage;
        [SerializeField] private Image _loseTeamImage;
        [SerializeField] private Sprite[] _teamSprites;
        [SerializeField] private UnityEngine.UI.Text[] _sharePercent;
        [SerializeField] private UnityEngine.UI.Text[] _drawSharePercent;
        [SerializeField] private UnityEngine.UI.Text[] _teamSeems;
        [SerializeField] private UnityEngine.UI.Text[] _names;
        [SerializeField] private UnityEngine.UI.Text[] _looseNames;
        [SerializeField] private UnityEngine.UI.Text[] _drawNames;
        [SerializeField] private GameObject _exitBt;
        private SoundHandler _soundHandler;
        [SerializeField] private AudioClip _finWhistleClip;
        
        private readonly Dictionary<int, string> _memberList = new();
        
        [SerializeField] private UnityEngine.UI.Text _reasonText; // 追加: 敗因を表示するテキストフィールド
        private const string SceneName = "Master_Wait";
        private void Start()
        {
            _gameResultHandler.CalcGameResult += OpenResultPanel;
            if (PhotonNetwork.IsMasterClient)
            {
                _exitBt.SetActive(true);
                var bt = _exitBt.GetComponent<Button>();
                bt.onClick.AddListener(CallReturnRoom);
            }
            _soundHandler = SoundHandler.InstanceSoundHandler;

            InitializeMemberList();
        }

        private void OnDestroy()
        {
            _gameResultHandler.CalcGameResult -= OpenResultPanel;
        }

        private void CallReturnRoom()
        {
            photonView.RPC("LoadGameScene", RpcTarget.All);
        }
        
        [PunRPC]
        public void LoadGameScene()
        {
            // もっといい方法を模索中
            PhotonNetwork.LoadLevel(SceneName);
            // var battleExitHandler = new BattleExitHandler();
            // battleExitHandler.ReturnRoom();
        }

        private void InitializeMemberList()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.MemberIdKey, out var memberId))
                    _memberList.Add((int)memberId, player.NickName);
            }
        }

        private void OpenResultPanel(int winTeamNum, string losereason) // 変更: 敗因情報を受け取る
        {
            
            _resultPanel.SetActive(true);
            _soundHandler.PlaySe(_finWhistleClip);
            if (winTeamNum == 0)
                OpenDrawPanel();
            else
                OpenWinLoosePanel(winTeamNum , losereason);
        }

        private void OpenDrawPanel()
        {
            _drawPanel.SetActive(true);
            UpdateDrawSharePercent();
            UpdatePlayerNames(_drawNames, isDraw: true);
        }

        private void OpenWinLoosePanel(int winTeamNum,string losereason)
        {
            //敗因の表示
            _reasonText.text = losereason; // 追加: 敗因情報を表示
            _winLoosePanel.SetActive(true);
            _winTeamImage.sprite = _teamSprites[winTeamNum - 1];
            _loseTeamImage.sprite = _teamSprites[winTeamNum % 2];
            UpdateTeamSeems(winTeamNum);
            UpdateSharePercent(winTeamNum);
            UpdatePlayerNames(_names, winTeamNum);
        }

        private void UpdateTeamSeems(int winTeamNum)
        {
            if (winTeamNum == 1)
            {
                _teamSeems[0].text = "あおチーム";
                _teamSeems[1].text = "あかチーム";
            }
            else
            {
                _teamSeems[0].text = "あかチーム";
                _teamSeems[1].text = "あおチーム";
            }
        }

        private void UpdateSharePercent(int winTeamNum)
        {
            if (winTeamNum == 1)
                SetSharePercent(_blockGenerator.BlocksShareTeam1, _blockGenerator.BlocksShareTeam2);
            else
                SetSharePercent(_blockGenerator.BlocksShareTeam2, _blockGenerator.BlocksShareTeam1);
        }

        private void SetSharePercent(float team1Percent, float team2Percent)
        {
            _sharePercent[0].text = team1Percent + "%";
            _sharePercent[1].text = team2Percent + "%";
        }

        private void UpdateDrawSharePercent()
        {
            SetSharePercent(_blockGenerator.BlocksShareTeam1, _blockGenerator.BlocksShareTeam2);
        }

        private void UpdatePlayerNames(UnityEngine.UI.Text[] nameFields, int winTeamNum = 0, bool isDraw = false)
        {
            var count = isDraw ? 0 : (winTeamNum - 1) * 2;
            for (var i = 0; i < nameFields.Length; i++)
            {
                if (_memberList.TryGetValue(count, out var playerName))
                    nameFields[i].text = playerName;

                count++;
                if (count >= ConnectionHandler.MaxPlayer) count = 0;
            }
        }
    }
}
