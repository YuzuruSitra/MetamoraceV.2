using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Character
{
    public class CharacterEffectHandler : MonoBehaviourPunCallbacks
    {
        public enum Effects
        {
            Stan,
            Die,
            Run,
            Jump
        }

        [SerializeField] private CharacterStatus _characterStatus;
        [SerializeField] private GameObject _selfEnhancementEffect;
        [SerializeField] private GameObject _stanEffect;
        [SerializeField] private GameObject _dieEffect;
        [SerializeField] private GameObject _runEffect;
        [SerializeField] private GameObject _jumpEffect;

        private Dictionary<Effects, GameObject> _effects;
        private Vector3 _runOffset = new Vector3(0f, 0.4f, 0f);
        private WaitForSeconds _runWaitSeconds = new WaitForSeconds(0.6f);
        private Coroutine _runCoroutine;
        [SerializeField] private CharacterMover _characterMover;
        private const float JumpPadding = 0.02f;

        private void Start()
        {
            InitializeEffects();
            if (!photonView.IsMine) return;

            _characterStatus.ChangeConditionEvent += OnConditionChanged;
            _characterStatus.ChangeSpecialEffectsEvent += OnSpecialEffectChanged;
        }

        private void OnDestroy()
        {
            if (!photonView.IsMine) return;

            _characterStatus.ChangeConditionEvent -= OnConditionChanged;
            _characterStatus.ChangeSpecialEffectsEvent -= OnSpecialEffectChanged;
        }

        private void InitializeEffects()
        {
            _effects = new Dictionary<Effects, GameObject>
            {
                { Effects.Stan, _stanEffect },
                { Effects.Die, _dieEffect },
                { Effects.Run, _runEffect },
                { Effects.Jump, _jumpEffect }
            };

            _runEffect.transform.parent = null;
            _jumpEffect.transform.parent = null;
        }

        private void OnSpecialEffectChanged(CharacterStatus.SpecialEffects special)
        {
            bool isActive = special == CharacterStatus.SpecialEffects.SelfEnhancement;
            SetEnhancementEffect(isActive);
            photonView.RPC(nameof(SetEnhancementEffect), RpcTarget.Others, isActive);
        }

        [PunRPC]
        public void SetEnhancementEffect(bool isActive)
        {
            _selfEnhancementEffect.SetActive(isActive);
        }

        private void OnConditionChanged(CharacterStatus.Condition condition)
        {
            switch (condition)
            {
                case CharacterStatus.Condition.Stan:
                case CharacterStatus.Condition.VDeath:
                case CharacterStatus.Condition.HDeath:
                    ApplyEffect(condition == CharacterStatus.Condition.Stan ? Effects.Stan : Effects.Die);
                    break;
                case CharacterStatus.Condition.Run:
                    if (_runCoroutine == null)
                        _runCoroutine = StartCoroutine(RunEffectCoroutine());
                    break;
                case CharacterStatus.Condition.Jump:
                    ApplyJumpEffect();
                    break;
                default:
                    StopEffectCoroutine();
                    DisableAllEffects();
                    break;
            }
        }

        private IEnumerator RunEffectCoroutine()
        {
            while (true)
            {
                if (_characterMover.IsGrounded)
                {
                    Vector3 position = transform.position + _runOffset;
                    ApplyEffect(Effects.Run, position);
                }
                yield return _runWaitSeconds;
            }
        }

        private void ApplyJumpEffect()
        {
            if (_jumpEffect.activeSelf) _jumpEffect.SetActive(false);
            var position = transform.position;
            position.y += JumpPadding;
            ApplyEffect(Effects.Jump, position);
        }

        private void ApplyEffect(Effects effect, Vector3? position = null)
        {
            _effects[effect].SetActive(true);
            photonView.RPC(nameof(RpcApplyEffect), RpcTarget.Others, (int)effect, position ?? Vector3.zero);

            if (position != null) MoveEffect(effect, (Vector3)position);
        }

        [PunRPC]
        public void RpcApplyEffect(int effectIndex, Vector3 position)
        {
            var effect = (Effects)effectIndex;
            _effects[effect].SetActive(true);
            if (position != Vector3.zero) MoveEffect(effect, position);
        }

        private void MoveEffect(Effects effect, Vector3 position)
        {
            _effects[effect].transform.position = position;
        }

        private void StopEffectCoroutine()
        {
            if (_runCoroutine != null)
            {
                StopCoroutine(_runCoroutine);
                _runCoroutine = null;
            }
        }

        private void DisableAllEffects()
        {
            foreach (var effect in _effects.Values)
                effect.SetActive(false);
        }
    }
}
