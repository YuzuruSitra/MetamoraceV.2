using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheakAround : MonoBehaviour
{
    private BlockBehaviour _currentBlock;
     private Vector3 _upPadding = new Vector3(0f,0.5f,0f);
     [SerializeField] 
    private float _playerReach = 1.0f;
    public BlockBehaviour CheckBlockRay(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + _upPadding, direction);
        Debug.DrawRay(transform.position + _upPadding, direction, Color.red, 1.0f);
        if (!Physics.Raycast(ray, out RaycastHit hit, _playerReach)) return null;
        _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
        return _currentBlock;
       
    }
}
