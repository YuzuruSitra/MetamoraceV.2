using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterAnimator : MonoBehaviourPunCallbacks
    {
        private Animator _anim;
        [SerializeField] private CharacterStatus _characterStatus;
        private static readonly int IsVDeath = Animator.StringToHash("_isVDeath");
        private static readonly int IsHDeath = Animator.StringToHash("_isHDeath");
        private static readonly int IsStan = Animator.StringToHash("_isStan");
        private static readonly int IsSwing = Animator.StringToHash("_isSwing");
        private static readonly int IsJump = Animator.StringToHash("_isJump");
        private static readonly int IsBreak = Animator.StringToHash("_isBreak");
        private static readonly int IsIdole = Animator.StringToHash("_isIdole");
        private static readonly int IsWalk = Animator.StringToHash("_isWalk");
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        private void Start()
        {
            if (!photonView.IsMine) return;
            _anim = GetComponent<Animator>();
            _characterStatus.ChangeConditionEvent += ChangeAnim;
        }

        private void ChangeAnim(CharacterStatus.Condition condition)
        {
            switch (condition)
            {
                case CharacterStatus.Condition.Idole:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, true);
                    _anim.SetBool(IsWalk, false);
                    _anim.SetFloat(MoveSpeed, 0.0f);
                    break;
                case CharacterStatus.Condition.Walk:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, true);
                    _anim.SetFloat(MoveSpeed, 0.5f);
                    break;
                case CharacterStatus.Condition.Run:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, true);
                    _anim.SetFloat(MoveSpeed, 1.0f);
                    break;
                case CharacterStatus.Condition.Jump:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, true);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
                case CharacterStatus.Condition.Generate:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, true);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
                case CharacterStatus.Condition.Break:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, true);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
                case CharacterStatus.Condition.VDeath:
                    _anim.SetBool(IsVDeath, true);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
                case CharacterStatus.Condition.HDeath:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, true);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
            }
        }
       
    }
}
