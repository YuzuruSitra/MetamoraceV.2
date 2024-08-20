using System;
//using Photon.Pun;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum Condition
    {
        Idole,
        Walk,
        Run,
        Jump,
        Stan,
        Swing,
        Break,
        VDeath,
        HDeath
    }

    private Condition _currentCondition;
    public Condition CurrentCondition => _currentCondition;
    public event Action<Condition> ChangeConditionEvent;

    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerObjectManipulator _playerObjectManipulator;
    [SerializeField] private PlayerCheakAround _playerCheakAround;

    private void Start()
    {
        //if (!photonView.IsMine) return;
        _currentCondition = Condition.Idole;
    }

    private void Update()
    {
        //if (!photonView.IsMine) return;
        JudgmentCondition();
    }

    private void JudgmentCondition()
    {
        if (!_playerMover.OnGround)
        {
            ChangeCondition(Condition.Jump);
        }
        else if (_playerObjectManipulator.IsStan)
        {
            ChangeCondition(Condition.Stan);
        }
        else if (_playerMover.CurrentMoveSpeed == 0)
        {
            ChangeCondition(Condition.Idole);
        }
        else if (_playerMover.CurrentMoveSpeed <= _playerMover.WalkSpeed)
        {
            ChangeCondition(Condition.Walk);
        }
        else if (_playerCheakAround.VerticalDeath)
        {
            ChangeCondition(Condition.VDeath);
        }
        else if (_playerCheakAround.HorizontalDeath)
        {
            ChangeCondition(Condition.HDeath);
        }
        
        // else if (_playerMover.CurrentMoveSpeed <= _playerMover.RunSpeed)
        // {
        //     ChangeCondition(Condition.Run);
        // }
    }

    private void ChangeCondition(Condition newCondition)
    {
        if (_currentCondition == newCondition) return;
        ChangeConditionEvent?.Invoke(newCondition);
        _currentCondition = newCondition;
    }
}