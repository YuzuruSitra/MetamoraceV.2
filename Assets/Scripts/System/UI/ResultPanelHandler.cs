using System.Battle;
using System.Network;
using Photon.Pun;
using UnityEngine;

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
        [SerializeField] private UnityEngine.UI.Text[] _winNames;
        [SerializeField] private UnityEngine.UI.Text[] _looseNames;
        [SerializeField] private UnityEngine.UI.Text[] _drawNames;
        [SerializeField] private UnityEngine.UI.Button _exitBt;

        private void Start()
        {
            _gameResultHandler.CalcGameResult += OpenResultPanel;
            var exitHandler = new BattleExitHandler();
            _exitBt.onClick.AddListener(exitHandler.ReturnRoom);
        }

        private void OnDestroy()
        {
            _gameResultHandler.CalcGameResult -= OpenResultPanel;
        }

        private void OpenResultPanel(int winTeamNum)
        {
            _resultPanel.SetActive(true);

            if (winTeamNum == 0)
            {
                _drawPanel.SetActive(true);
                UpdateDrawSharePercent();
                // Show Players.
                UpdateDrawPlayerNames(_drawNames);
            }
            else
            {
                _winLoosePanel.SetActive(true);
                UpdateSharePercent(winTeamNum);
                // Show Winning Teams.
                UpdatePlayerNames(winTeamNum, _winNames);
                // Show losing teams
                UpdatePlayerNames(3 - winTeamNum, _looseNames);
            }
        }

        private void UpdateSharePercent(int winTeamNum)
        {
            if (winTeamNum == 1)
            {
                _sharePercent[0].text = _blockGenerator.BlocksShareTeam1 + "%";
                _sharePercent[1].text = _blockGenerator.BlocksShareTeam2 + "%";
            }
            else
            {
                _sharePercent[0].text = _blockGenerator.BlocksShareTeam2 + "%";
                _sharePercent[1].text = _blockGenerator.BlocksShareTeam1 + "%";
            }
        }
        
        private void UpdateDrawSharePercent()
        {
            _drawSharePercent[0].text = _blockGenerator.BlocksShareTeam1 + "%";
            _drawSharePercent[1].text = _blockGenerator.BlocksShareTeam2 + "%";
        }

        private void UpdatePlayerNames(int teamNum, UnityEngine.UI.Text[] nameFields)
        {
            var index = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) continue;
                var teamValue = (int)teamId;
                if (teamValue != teamNum) continue;
                if (index >= nameFields.Length) continue;
                nameFields[index].text = player.NickName;
                index++;
            }
        }

        private void UpdateDrawPlayerNames(UnityEngine.UI.Text[] nameFields)
        {
            int[] indices = { 0, 2 };
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) continue;
                var teamValue = (int)teamId;
                if (teamValue != 1 && teamValue != 2) continue;
                nameFields[indices[teamValue - 1]].text = player.NickName;
                indices[teamValue - 1]++;
            }
        }
        
    }
}
