using System.Network;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterUIEffectHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private CharacterStatus _characterStatus;
        [SerializeField] private GameObject _selfEnhancementEffect;
        [SerializeField] private GameObject _stanEffect;
        [SerializeField] private GameObject _dieEffect;
        private GameObject[] _effects;
        private const float EffectPosZ = 0.4f;
         private RectTransform _deathEffectPos;

        private readonly Vector2 _stanOffSets = 
            new (0f, 0.8f);
        private readonly Vector2 _selfEnhancementOffSets = 
            new(0, 1.2f)
        ;
        private readonly Vector2 _dieOffSets = 
            new(0, 1.2f);
        [SerializeField] private Transform target;


        private void Start()
        {
            if (!photonView.IsMine) return;
            _deathEffectPos = _dieEffect.GetComponent<RectTransform>();

            _dieEffect.SetActive(false);
            _characterStatus.ChangeConditionEvent += ReceiveEffect;
            _characterStatus.ChangeSpecialEffectsEvent += ReceiveEnhancement;
            _effects = new[] { _stanEffect, _dieEffect };

            var num = _characterStatus.LocalPlayerTeam - 1;
            _stanEffect.transform.position = _stanOffSets;
            _selfEnhancementEffect.transform.position = _selfEnhancementOffSets;
            _dieEffect.transform.position = _dieOffSets;
        }

        void Update()
        {
            // メインカメラが存在するかチェック
            if (Camera.main == null)
            {
                Debug.LogError("Main camera not found!");
                return;
            }

            // ターゲットのワールド座標をスクリーン座標に変換し、オフセットを適用
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
            _deathEffectPos.position = screenPos + (Vector3)_dieOffSets;  // Vector3にキャストして加算
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
