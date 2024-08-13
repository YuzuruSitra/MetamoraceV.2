using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace System.Network
{
    public class CustomInfoHandler
    {
        private static CustomInfoHandler _instance;
        public static CustomInfoHandler Instance => _instance ??= new CustomInfoHandler();
        private readonly Hashtable _properties;
        public const string TeamIdKey = "TeamID";
        public const string BattleIdKey = "BattleID";
        public const string ReadyKey = "Ready";
        
        private CustomInfoHandler()
        {
            _properties = new Hashtable
            {
                { TeamIdKey, 0 },
                { BattleIdKey, 0 },
                { ReadyKey, 0 } // 1-Ready
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(_properties);
        }

        public void ChangeValue(string key, int value, Player player)
        {
            _properties[key] = value;
            player.SetCustomProperties(_properties);
        }
    }
}
