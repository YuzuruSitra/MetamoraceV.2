using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCBehavior : MonoBehaviour
{
    [SerializeField] private bool _useCombo;
    private bool _used = false;
    public bool _Used => _used;
    private BlockBehaviour _hitBlock;
    public void Break4()
    {
        Debug.Log("4");
        
        Vector3[] directions =
        {
            Vector3.up,
            Vector3.down,
            Vector3.right,
            Vector3.left
        };

        foreach (var direction in directions)
        {
            BreakDirection(direction);
        }
    }

    private void BreakDirection(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        float rayLength = 1.0f;

        if (!Physics.Raycast(ray, out hit, rayLength)) return;
        _hitBlock = hit.collider.GetComponent<BlockBehaviour>();
        string _hitBlockType = _hitBlock._objName;
        if (_hitBlockType == "Ambras" ||
            _hitBlockType == "Heros" ||
            _hitBlockType == "ItemCBlock")
        {
            Destroy(hit.collider.gameObject);

            if (!hit.collider.CompareTag("ItemCBlock")) return;

            if (!_useCombo) return;
            var itemCBehavior = hit.collider.GetComponent<ItemCBehavior>();
            if (!itemCBehavior._Used)
            {
                _used = true;
                itemCBehavior.Break4();
            }
        }
    }
}