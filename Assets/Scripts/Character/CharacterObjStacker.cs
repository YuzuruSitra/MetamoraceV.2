using System;
using System.UI;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterObjStacker : MonoBehaviourPunCallbacks
    {
        private StackUIHandler _stackUIHandler;
        public string HasBlock { get; private set; }
        private event Action<string> GetBlockEvent;
        private readonly string[] _blockStack = new string[3];
        public event Action<string[]> ChangeStackEvent;
        public const string NullKey = "Null";
        private void Start()
        {
            if (!photonView.IsMine) return;
            HasBlock = NullKey;
            for (var i = 0; i < _blockStack.Length; i++) _blockStack[i] = NullKey;
            ResetStack();
            _stackUIHandler = GameObject.FindWithTag("StackUIHandler").GetComponent<StackUIHandler>();
            GetBlockEvent += _stackUIHandler.ChangeInsBlockImage;
            ChangeStackEvent += _stackUIHandler.ChangeStackImage;
        }
        
        private void OnDestroy()
        {
            if (!photonView.IsMine) return;
            GetBlockEvent -= _stackUIHandler.ChangeInsBlockImage;
            ChangeStackEvent -= _stackUIHandler.ChangeStackImage;
        }
        

        public void BreakBlock(GameObject target)
        {
            var objKind = target.tag;
            for (var i = 0; i < _blockStack.Length; i++)
            {
                if (_blockStack[i] != NullKey) continue;
                _blockStack[i] = objKind;
                break;
            }
            ChangeStackEvent?.Invoke(_blockStack);
            ChangeHasBlock("Heros");
        }

        public void InsBlock()
        {
            ChangeHasBlock(NullKey);
        }

        public void ChangeHasBlock(string newBlock)
        {
            if (newBlock == HasBlock) return;
            HasBlock = newBlock;
            GetBlockEvent?.Invoke(newBlock);
        }

        public void ResetStack()
        {
            for (var i = 0; i < _blockStack.Length; i++)
                _blockStack[i] = NullKey;
            ChangeStackEvent?.Invoke(_blockStack);
        }
    }
}
