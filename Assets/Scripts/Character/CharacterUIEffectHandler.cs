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
        private RectTransform _deathEffectPos, _selfEnhancementEffectPos, _stanEffectPos;
        
        private readonly Vector2 _stanOffsets = new(0f, 0.8f);
        private readonly Vector2 _selfEnhancementOffsets = new(0f, 1.2f);
        private readonly Vector2 _dieOffsets = new(0f, 1.2f);
        [SerializeField] private Transform target;

        private Camera _mainCamera;
        private bool _effectsActive = false;

        private void Start()
        {
            _mainCamera = Camera.main;
            _deathEffectPos = _dieEffect.GetComponent<RectTransform>();
            _selfEnhancementEffectPos = _selfEnhancementEffect.GetComponent<RectTransform>();
            _stanEffectPos = _stanEffect.GetComponent<RectTransform>();

            _dieEffect.SetActive(false);
            _characterStatus.ChangeConditionEvent += ReceiveEffect;
            _characterStatus.ChangeSpecialEffectsEvent += ReceiveEnhancement;
            _effects = new[] { _stanEffect, _dieEffect, _selfEnhancementEffect };

            DisableEffects(); // 全てのエフェクトを最初に無効化
        }

        void Update()
        {
            // エフェクトが有効な場合のみ座標計算を行う
            if (_effectsActive)
            {
                Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(_mainCamera, target.position);
                _deathEffectPos.position = screenPos + (Vector3)_dieOffsets;
                _selfEnhancementEffectPos.position = screenPos + (Vector3)_selfEnhancementOffsets;
                _stanEffectPos.position = screenPos + (Vector3)_stanOffsets;
            }
        }

        private void OnDestroy()
        {
            _characterStatus.ChangeConditionEvent -= ReceiveEffect;
            _characterStatus.ChangeSpecialEffectsEvent -= ReceiveEnhancement;
        }

        private void ReceiveEnhancement(CharacterStatus.SpecialEffects special)
        {
            bool isActive = special == CharacterStatus.SpecialEffects.SelfEnhancement;
            if (_selfEnhancementEffect.activeSelf != isActive) // 状態が変わった場合のみ処理
            {
                _selfEnhancementEffect.SetActive(isActive);
                photonView.RPC(nameof(ApplyEnhancement), RpcTarget.All, isActive);
            }
        }

        [PunRPC]
        public void ApplyEnhancement(bool isActive)
        {
            _selfEnhancementEffect.SetActive(isActive);
        }

        private void ReceiveEffect(CharacterStatus.Condition condition)
        {
            int effectIndex = -1;

            switch (condition)
            {
                case CharacterStatus.Condition.Stan:
                    effectIndex = 0;
                    break;
                case CharacterStatus.Condition.VDeath:
                case CharacterStatus.Condition.HDeath:
                    effectIndex = 1;
                    break;
                default:
                    DisableEffects();
                    return;
            }

            // 状態が変わった場合のみRPCを送信
            if (!_effects[effectIndex].activeSelf)
            {
                photonView.RPC(nameof(ApplyEffect), RpcTarget.All, effectIndex);
            }
        }

        [PunRPC]
        public void ApplyEffect(int targetIndex)
        {
            for (int i = 0; i < _effects.Length; i++)
            {
                _effects[i].SetActive(i == targetIndex);
            }
            _effectsActive = true;
        }

        [PunRPC]
        public void DisableEffects()
        {
            foreach (var effect in _effects)
            {
                effect.SetActive(false);
            }
            _effectsActive = false; // エフェクトが無効な状態
        }
    }
}
