using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheakAround : MonoBehaviour
{
    private BlockBehaviour _currentBlock;
     private Vector3 _upPadding = new Vector3(0f,0.5f,0f);
     [SerializeField] 
    private float _playerReach = 1.0f;
    [SerializeField]
    private float _jumprayrength = 0.15f;
    public BlockBehaviour CheckBlockRay(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + _upPadding, direction);
        Debug.DrawRay(transform.position + _upPadding, direction, Color.red, 1.0f);
        if (!Physics.Raycast(ray, out RaycastHit hit, _playerReach)) return null;
        _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
        return _currentBlock;
    }

    public bool CheakGroundRay()
    {
        bool _onGround;
        float raypos = 0.45f;
        float rayheight = 0.1f;
        _onGround = CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, raypos), Vector3.down)) ||
                    CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, -raypos), Vector3.down)) ||
                    CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, -raypos), Vector3.down)) ||
                    CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, raypos), Vector3.down));
        return _onGround;
    }
    // 地上にいるか判定
    private bool CheckAndJump(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * _jumprayrength, Color.red, 0.1f);
        return Physics.Raycast(ray, out hit, _jumprayrength);
    }
}
