using System.Battle;
using System.Network;
using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

namespace System.UI
{
    public class ResultPanelHandler : MonoBehaviour
    {
        [SerializeField] private GameResultHandler _gameResultHandler;
        [SerializeField] private BlockGenerator _blockGenerator;
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private GameObject _winLoosePanel;
        [SerializeField] private GameObject _drawPanel;
        [SerializeField] private UnityEngine.UI.Text[] _sharePercent;
        [SerializeField] private UnityEngine.UI.Text[] _drawSharePercent;
        [SerializeField] private UnityEngine.UI.Text[] _teamSeems;
        [SerializeField] private UnityEngine.UI.Text[] _names;
        [SerializeField] private UnityEngine.UI.Text[] _looseNames;
        [SerializeField] private UnityEngine.UI.Text[] _drawNames;
        [SerializeField] private UnityEngine.UI.Button _exitBt;

        private Dictionary<int, string> _memberList = new Dictionary<int, string>();

        private void Start()
        {
            _gameResultHandler.CalcGameResult += OpenResultPanel;
            _exitBt.onClick.AddListener(() => new BattleExitHandler().ReturnRoom());

            InitializeMemberList();
        }

        private void OnDestroy()
        {
            _gameResultHandler.CalcGameResult -= OpenResultPanel;
        }

        // メンバーリストの初期化処理
        private void InitializeMemberList()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.MemberIdKey, out var memberId))
                    _memberList.Add((int)memberId, player.NickName);
            }
        }

        // 結果パネルの開閉処理
        private void OpenResultPanel(int winTeamNum)
        {
            _resultPanel.SetActive(true);

            if (winTeamNum == 0)
                OpenDrawPanel();
            else
                OpenWinLoosePanel(winTeamNum);
        }

        // 引き分けパネルの表示
        private void OpenDrawPanel()
        {
            _drawPanel.SetActive(true);
            UpdateDrawSharePercent();
            UpdatePlayerNames(_drawNames, isDraw: true);
        }

        // 勝敗パネルの表示
        private void OpenWinLoosePanel(int winTeamNum)
        {
            _winLoosePanel.SetActive(true);
            UpdateTeamSeems(winTeamNum);
            UpdateSharePercent(winTeamNum);
            UpdatePlayerNames(_names, winTeamNum);
        }

        // チーム表示の更新
        private void UpdateTeamSeems(int winTeamNum)
        {
            _teamSeems[0].text = "Team" + winTeamNum;
            _teamSeems[1].text = "Team" + (3 - winTeamNum);
        }

        // シェアパーセントの更新
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

        // プレイヤー名の表示更新
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
