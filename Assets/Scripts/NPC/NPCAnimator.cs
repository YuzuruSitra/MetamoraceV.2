using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPCAnimator : MonoBehaviour
    {
        private Animator _anim;
        [SerializeField] private NPCStateManagement _npcStateManagement;
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
            //if (!photonView.IsMine) return;
            _anim = GetComponent<Animator>();
            _npcStateManagement.ChangeStateEvent += ChangeAnim;
        }

        void Update()
        {

        }

        private void ChangeAnim(NPCStateManagement.NPCState state)
        {
            //Debug.Log(condition);
            switch (state)
            {
                case NPCStateManagement.NPCState.Idle:
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
                case NPCStateManagement.NPCState.Walk:
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
                case NPCStateManagement.NPCState.Avoid:
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
                case NPCStateManagement.NPCState.Run:
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
                case NPCStateManagement.NPCState.Jump:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, true);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
                case NPCStateManagement.NPCState.VDeath:
                    _anim.SetBool(IsVDeath, true);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
                case NPCStateManagement.NPCState.HDeath:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, true);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    break;
                case NPCStateManagement.NPCState.Break:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, false);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, true);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    //Debug.Log("Breaking");
                    break;
                case NPCStateManagement.NPCState.Generate:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, false);
                    _anim.SetBool(IsSwing, true);
                    _anim.SetBool(IsJump, false);
                    _anim.SetBool(IsBreak, false);
                    _anim.SetBool(IsIdole, false);
                    _anim.SetBool(IsWalk, false);
                    //Debug.Log("Swing");
                    break;
                case NPCStateManagement.NPCState.Stan:
                    _anim.SetBool(IsVDeath, false);
                    _anim.SetBool(IsHDeath, false);
                    _anim.SetBool(IsStan, true);
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

