using System.Network;
using Photon.Pun;
using UnityEngine;
using System.Collections; 

namespace Character
{
    public class CharacterEffectHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private CharacterStatus _characterStatus;
        [SerializeField] private GameObject _selfEnhancementEffect;
        [SerializeField] private GameObject _stanEffect;
        [SerializeField] private GameObject _dieEffect;
        [SerializeField] private GameObject _walkEffect;
        [SerializeField] private GameObject _jumpEffect;

        private GameObject[] _effects;
        private const float EffectPosZ = 0.4f;
        private readonly Vector3[] _stanOffSets = {
            new (0f, 0.8f, -EffectPosZ),
            new (0f, 0.8f, EffectPosZ)
        };
        private readonly Vector3[] _selfEnhancementOffSets = {
            new(0, 1.2f, -EffectPosZ),
            new(0, 1.2f, EffectPosZ)
        };
        private readonly Vector3[] _dieOffSets = {
            new(0, 1.2f, -EffectPosZ),
            new(0, 1.2f, EffectPosZ)
        };
        private readonly Vector3 _walkOffSet = 
            new (0f, 0.4f, 0f)
        ;
    
        bool PlayWalkEffect ,PlayJumpEffect= false;
        private void Start()
        {
            if (!photonView.IsMine) return;
            _dieEffect.SetActive(false);
            _characterStatus.ChangeConditionEvent += ReceiveEffect;
            _characterStatus.ChangeSpecialEffectsEvent += ReceiveEnhancement;
             _effects = new[] { _stanEffect, _dieEffect,_selfEnhancementEffect};
             _walkEffect.transform.parent = null;
             _jumpEffect.transform.parent = null;
            var num = _characterStatus.LocalPlayerTeam - 1;
            // _stanEffect.transform.position = _stanOffSets[num];
            // _selfEnhancementEffect.transform.position = _selfEnhancementOffSets[num];
            // _dieEffect.transform.position = _dieOffSets[num];
        }

        private void OnDestroy()
        {
            if (!photonView.IsMine) return;
            _characterStatus.ChangeConditionEvent -= ReceiveEffect;
            _characterStatus.ChangeSpecialEffectsEvent -= ReceiveEnhancement;
        }
        
        private void ReceiveEnhancement(CharacterStatus.SpecialEffects special)
        {
            _selfEnhancementEffect.SetActive(special == CharacterStatus.SpecialEffects.SelfEnhancement);
            photonView.RPC(nameof(ApplyEnhancement), RpcTarget.All,
                special == CharacterStatus.SpecialEffects.SelfEnhancement);
        }
        
        [PunRPC]
        public void ApplyEnhancement(bool isActive)
        {
            _selfEnhancementEffect.SetActive(isActive);
        }

        private void ReceiveEffect(CharacterStatus.Condition condition)
        {
            switch (condition)
            {
                case CharacterStatus.Condition.Stan:
                    photonView.RPC(nameof(ApplyEffect), RpcTarget.All, 0);
                    break;
                case CharacterStatus.Condition.VDeath:
                case CharacterStatus.Condition.HDeath:
                    photonView.RPC(nameof(ApplyEffect), RpcTarget.All, 1);
                    break;
                default:
                    photonView.RPC(nameof(DisableEffects), RpcTarget.All);
                    break;
            }
        }

        public IEnumerator RecieveWalkEffect()
        {
            if(PlayWalkEffect) yield break;
             _walkEffect.transform.position = this.gameObject.transform.position + _walkOffSet;
            _walkEffect.SetActive(true);
            PlayWalkEffect = true;
           yield return new WaitForSeconds (0.6f);
             PlayWalkEffect = false;
        }
       
          public IEnumerator ApplyJumpEffect()
        {
             _jumpEffect.transform.position = this.gameObject.transform.position;
            _jumpEffect.SetActive(true);
           yield return new WaitForSeconds (0.4f);
           _jumpEffect.SetActive(false);

        }
        [PunRPC]
        public void ApplyEffect(int target)
        {
            // var other = 1 - target;
            // _effects[other].SetActive(false);
            _effects[target].SetActive(true);
        }
        
        [PunRPC]
        public void DisableEffects()
        {
            foreach (var t in _effects)
                t.SetActive(false);
        }
    }
}
