using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterItemHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private CharacterObjStacker _characterObjStacker;
        [SerializeField] private CharacterObjBreaker _characterObjBreaker;
        [SerializeField] private CharacterMover _characterMover;
        [SerializeField] private CharacterStatus _characterStatus;
        [SerializeField] private float _enhanceTime;
        private WaitForSeconds _enhanceForSeconds;
        private Coroutine _enhanceCoroutine;
        [SerializeField] private float _enhanceSpeedFactor;
        [SerializeField] private float _enhancePowerFactor;
        
        private enum Item
        {
            None,
            SpecialEnhancement,
            GiganticRay,
            SpecialBlock
        }
        private Item _haveItem = Item.None;
        
        private void Start()
        {
            if (!photonView.IsMine) return;
            _characterObjStacker.ChangeStack += GetItem;
            _enhanceForSeconds = new WaitForSeconds(_enhanceTime);
        }

        private void OnDestroy()
        {
            if (!photonView.IsMine) return;
            _characterObjStacker.ChangeStack -= GetItem;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            if (Input.GetMouseButtonDown(1)) UseItem();
        }

        private void GetItem(string[] stacks)
        {
            var ambrasCount = 0;
            var herosCount = 0;
            foreach (var t in stacks)
            {
                switch (t)
                {
                    case "Ambras":
                        ambrasCount++;
                        break;
                    case "Heros":
                    case "BigHeros":
                        herosCount++;
                        break;
                    case CharacterObjStacker.NullKey:
                        return;
                }
            }
            if (herosCount == stacks.Length)
                _haveItem = Item.SpecialEnhancement;
            else if (ambrasCount == stacks.Length)
                _haveItem = Item.GiganticRay;
            else
                _haveItem = Item.SpecialBlock;
        }

        private void UseItem()
        {
            switch (_haveItem)
            {
                case Item.None:
                    return;
                case Item.SpecialEnhancement:
                    if (_enhanceCoroutine != null) StopCoroutine(_enhanceCoroutine);
                    _enhanceCoroutine = StartCoroutine(EnhancementSelf());
                    break;
                case Item.GiganticRay:
                    _characterObjStacker.ChangeHasBlock("BigHeros");
                    break;
                case Item.SpecialBlock:
                    _characterObjStacker.ChangeHasBlock("ItemCBlock");
                    break;
            }
        }

        private IEnumerator EnhancementSelf()
        {
            _characterStatus.ReceiveSpecialEffects(CharacterStatus.SpecialEffects.SelfEnhancement);
            _characterObjBreaker.ChangePowerFactor(_enhancePowerFactor);
            _characterMover.ChangeSpeedFactor(_enhanceSpeedFactor);
            yield return _enhanceForSeconds;
            _characterStatus.ReceiveSpecialEffects(CharacterStatus.SpecialEffects.None);
            _characterObjBreaker.ChangePowerFactor(1.0f);
            _characterMover.ChangeSpeedFactor(1.0f);
            _enhanceCoroutine = null;
        }
        
    }
}
