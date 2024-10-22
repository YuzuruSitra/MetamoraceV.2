using System;
using System.Battle;
using System.Collections;
using System.UI;
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
        private StackUIHandler _stackUIHandler;
        [SerializeField] private float _enhanceTime;
        private WaitForSeconds _enhanceForSeconds;
        private Coroutine _enhanceCoroutine;
        [SerializeField] private float _enhanceSpeedFactor;
        [SerializeField] private float _enhancePowerFactor;
        private TimeHandler _timeHandler;
        
        public enum Item
        {
            None,
            SpecialEnhancement,
            GiganticRay,
            SpecialBlock
        }
        private Item _haveItem = Item.None;
        private event Action<Item> ChangeItemEvent; 
        
        private void Start()
        {
            if (!photonView.IsMine) return;
            _enhanceForSeconds = new WaitForSeconds(_enhanceTime);
            _characterObjStacker.ChangeStackEvent += GetItem;
            _stackUIHandler = GameObject.FindWithTag("StackUIHandler").GetComponent<StackUIHandler>();
            _timeHandler = GameObject.FindWithTag("TimeHandler").GetComponent<TimeHandler>();
            ChangeItemEvent += _stackUIHandler.ChangeItemImage;
        }

        private void OnDestroy()
        {
            if (!photonView.IsMine) return;
            _characterObjStacker.ChangeStackEvent -= GetItem;
            ChangeItemEvent -= _stackUIHandler.ChangeItemImage;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            if (!_timeHandler.IsCountDown) return;
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
                    case CharacterObjStacker.BlockAmbras:
                        ambrasCount++;
                        break;
                    case CharacterObjStacker.BlockHeros:
                    case CharacterObjStacker.BlockBigHeros:
                    case CharacterObjStacker.BlockItemC:
                        herosCount++;
                        break;
                    case CharacterObjStacker.NullKey:
                        return;
                }
            }
            if (herosCount == stacks.Length)
                ChangeItem(Item.SpecialEnhancement);
            else if (ambrasCount == stacks.Length) 
                ChangeItem(Item.GiganticRay);
            else
                ChangeItem(Item.SpecialBlock);
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
                    if (_characterObjStacker.HasBlock == CharacterObjStacker.NullKey) return;
                    _characterObjStacker.ChangeHasBlock(CharacterObjStacker.BlockBigHeros);
                    break;
                case Item.SpecialBlock:
                    if (_characterObjStacker.HasBlock == CharacterObjStacker.NullKey) return;
                    _characterObjStacker.ChangeHasBlock(CharacterObjStacker.BlockItemC);
                    break;
            }
            ChangeItem(Item.None);
            _characterObjStacker.ResetStack();
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

        private void ChangeItem(Item newItem)
        {
            if (_haveItem == newItem) return;
            ChangeItemEvent?.Invoke(newItem);
            _haveItem = newItem;
        }
        
    }
}
