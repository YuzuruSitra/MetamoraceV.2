using System.Network;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace System.Battle
{
    public class BattleLauncher : MonoBehaviourPunCallbacks
    {
        public event Action BattleLaunch;
        public bool _isLaunch;
        
        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _isLaunch = false;
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (_isLaunch) return;
            _isLaunch = LaunchCheck();
            if (_isLaunch) BattleLaunch?.Invoke();
        }

        private bool LaunchCheck()
        {
            var count = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.TryGetValue(CustomInfoHandler.ReadyKey, out var readyValue)) continue;
                if ((int)readyValue == CustomInfoHandler.InitialValue) count++;
            }
            return (count == 0);
        }
        
    }
}
