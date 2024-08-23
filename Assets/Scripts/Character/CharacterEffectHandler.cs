using System.Network;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterEffectHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private CharacterStatus _characterStatus;
        [SerializeField] private GameObject _selfEnhancementEffect;
        [SerializeField] private GameObject _stanEffect;
        [SerializeField] private GameObject _dieEffect;
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

        private void Start()
        {
            if (!photonView.IsMine) return;
            _dieEffect.SetActive(false);
            _characterStatus.ChangeConditionEvent += ReceiveEffect;
            _characterStatus.ChangeSpecialEffectsEvent += ReceiveEnhancement;
            _effects = new[] { _stanEffect, _dieEffect };
            if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
            _stanEffect.transform.position = _stanOffSets[(int)teamId];
            _selfEnhancementEffect.transform.position = _selfEnhancementOffSets[(int)teamId];
            _dieEffect.transform.position = _dieOffSets[(int)teamId];
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
        
        [PunRPC]
        public void ApplyEffect(int target)
        {
            var other = 1 - target;
            _effects[other].SetActive(false);
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
