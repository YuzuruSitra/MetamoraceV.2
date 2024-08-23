using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterObjStacker : MonoBehaviourPunCallbacks
    {
        public string HasBlock { get; private set; }
        private readonly string[] _blockStack = new string[3];
        public const string NullKey = "Null";

        private void Start()
        {
            if (!photonView.IsMine) return;
            ResetStack();
        }

        public void BreakBlock(GameObject target)
        {
            var objKind = target.tag;
            HasBlock = objKind;
            for (var i = 0; i < _blockStack.Length; i++)
                if (_blockStack[i] == NullKey) _blockStack[i] = objKind;
        }

        public void InsBlock()
        {
            HasBlock = NullKey;
        }

        private void ResetStack()
        {
            for (var i = 0; i < _blockStack.Length; i++)
                _blockStack[i] = NullKey;
        }
    }
}
