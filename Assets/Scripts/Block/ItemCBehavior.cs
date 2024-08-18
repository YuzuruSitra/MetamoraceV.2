using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemCBehavior : MonoBehaviour
{
    [SerializeField] private bool _useCombo;
    private BlockBehaviour _hitBlock;
    [SerializeField] private float stanTime = 2.0f;
    public bool StanFlag { get; private set; } = false;
    public int StanMagnification { get; private set; } = 1;

    public enum ItemCType
    {
       Stan,
        Break4,
       wall
    }

    private ItemCType itemCType;

    public void LotteryItemC()
    {
        //enum型の要素数を取得
        int maxCount = System.Enum.GetNames(typeof(ItemCType)).Length;
        //ランダムな整数を取得
        int number = Random.Range(0, maxCount);
        //int型からenum型へ変換
        itemCType = (ItemCType)System.Enum.ToObject(typeof(ItemCType), number);
        switch (itemCType)
        {
            case ItemCType.Stan:
                StanFlag = true;
                break;
            case ItemCType.Break4:
                Break4();
                break;
            // case ItemCType.wall:
            //     StainWall();
            //     break;
        }
    }
    
    public void PlayerStan()
    {
        StanMagnification = 0;
        StartCoroutine(FinishStan());
    }

    private IEnumerator FinishStan()
    {
        yield return new WaitForSeconds(stanTime); //もとに戻す
        StanMagnification = 1;
    }

    public void StainWall()
    {
        //Playerのxz座標の位置に汚れ
    }

    public void Break4()
    {
        Debug.Log("1");

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
        Debug.Log("2");
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        float rayLength = 1.0f;

        if (!Physics.Raycast(ray, out hit, rayLength)) return;
        _hitBlock = hit.collider.GetComponent<BlockBehaviour>();
        if (_hitBlock == null) return;
        string _hitBlockType = _hitBlock._objName;
        if (_hitBlockType != "Ambras" && _hitBlockType != "Heros") return;

        Destroy(hit.collider.gameObject);
        ItemCBehavior itemCBehavior = hit.collider.GetComponent<ItemCBehavior>();
        if (itemCBehavior == null || !_useCombo) return;
        var values = System.Enum.GetValues(typeof(ItemCType));
        itemCType = (ItemCType)values.GetValue(Random.Range(0, values.Length));
        if (itemCType != ItemCType.Break4) return;
        itemCBehavior.Break4();
    }
}