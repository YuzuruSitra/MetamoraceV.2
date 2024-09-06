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
        public const string MemberIdKey = "MemberID";
        public const string TeamIdKey = "TeamID";
        public const string ReadyKey = "Ready";
        public const int InitialValue = -1;
        
        private CustomInfoHandler()
        {
            _properties = new Hashtable
            {
                { MemberIdKey, InitialValue },
                { TeamIdKey, InitialValue },
                { ReadyKey, InitialValue } // 1-Ready
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
