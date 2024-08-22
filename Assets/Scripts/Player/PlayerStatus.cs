using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public enum Condition
    {
        Idle,
        Walk,
        Run,
        Jump,
        Generate,
        Break,
        VDeath,
        HDeath,
        Stan,
        Pause
    }

    private Condition _currentCondition;
    public Condition CurrentCondition => _currentCondition;
    public event Action<Condition> ChangeConditionEvent;

    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerObjectManipulator _playerObjectManipulator;
    [SerializeField] private PlayerCheakAround _playerCheakAround;
    [SerializeField] private PlayerEffectHandler _playerEffectHandler;
    [SerializeField] private const float StanTime = 2.0f;
    private float StunElapsedTime;

    private readonly HashSet<Condition> _nonMovingConditions = new()
    {
        Condition.Pause,
        Condition.Break,
        Condition.Stan,
        Condition.VDeath,
        Condition.HDeath
    };
    private void Start()
    {
        if (!photonView.IsMine) return;
        _currentCondition = Condition.Idle;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        JudgmentCondition();
    }

    public void StartStan()
    {
        _playerEffectHandler.ChangeStan(true);
        ChangeCondition(Condition.Stan);
        StartCoroutine(FinishStan());
    }
    public IEnumerator FinishStan()
    {
        yield return new WaitForSeconds(StanTime); //もとに戻す
        ChangeCondition(Condition.Idle);
        _playerEffectHandler.ChangeStan(false);
    }
    
    private void JudgmentCondition()
    {
        if (_currentCondition is Condition.Stan or Condition.Pause) return;
        if (_playerCheakAround.VerticalDeath)
        {
            ChangeCondition(Condition.VDeath);
        }
        else if (_playerCheakAround.HorizontalDeath)
        {
            ChangeCondition(Condition.HDeath);
        }
        // else if (_playerMover.CurrentMoveSpeed == 0)
        // {
        //     ChangeCondition(Condition.Idole);
        // }
        
        
        else if (!_playerMover.IsGrounded)
        {
            ChangeCondition(Condition.Jump);
        }
        else if (_playerObjectManipulator.Swing)
        {
            ChangeCondition(Condition.Generate);
        }
        else if (_playerObjectManipulator.Breaking)
        {
            ChangeCondition(Condition.Break);
        }
        else if (_playerMover.CurrentMoveSpeed <= _playerMover.WalkSpeed)
        {
            ChangeCondition(Condition.Walk);
        }
        
        // else if (_playerMover.CurrentMoveSpeed <= _playerMover.RunSpeed)
        // {
        //     ChangeCondition(Condition.Run);
        // }
    }

    // External factors.
    public void ReceiveChangeState(Condition condition)
    {
        ChangeCondition(condition);
    }

    private void ChangeCondition(Condition newCondition)
    {
        if (_currentCondition == newCondition) return;
        ChangeConditionEvent?.Invoke(newCondition);
        _currentCondition = newCondition;
        ChangeMoveBool(newCondition);
    }

    private void ChangeMoveBool(Condition newCondition)
    {
        var isMove = !_nonMovingConditions.Contains(newCondition);
        _playerMover.SetMoveBool(isMove);
    }
}