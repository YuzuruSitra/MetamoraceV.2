using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    private PlayerBlockStack playerBlockStack;
    private UIHandler _uiHandler;
    private bool hasItemA, hasItemB, hasItemC;
    private float _itemAEffectTime = 6.0f;
    public int ItemAEffectRate { get; private set; } = 1;

    public void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        playerBlockStack = GetComponent<PlayerBlockStack>();
    }
    public void CreateItem()
    {
        if (!playerBlockStack.FullStack) return;
        string ItemType = playerBlockStack.CreateItemType();
            //アイテムUI表示
            _uiHandler.SetItemImage(ItemType);
            hasItemA = false;
            hasItemB = false;
            hasItemC = false;
            //アイテムフラグオン
            switch (ItemType)
            {
                case "ItemA":
                     hasItemA = true;
                    break;
                case "ItemB":
                     hasItemB = true;
                    break;
                case "ItemC":
                     hasItemC = true;
                    break;
                default :
                    Debug.Log("Error");
                    break;
            }    
       // Debug.Log($"hasItemA: {hasItemA}, hasItemB: {hasItemB}, hasItemC: {hasItemC}");

    }

    public void UseItemA()
    {
        if (!Input.GetMouseButton(1) || !hasItemA) return;
            //アイテムの効果ーージャンプと破壊2倍
            ItemAEffectRate = 2;
            // //ブロックすたっくUIリセット
            playerBlockStack.ResetBlock();
            //ブロックスタックリセット
            _uiHandler.ResetStackImage();
            _uiHandler.ResetItemImage();
            hasItemA = false;
            StartCoroutine(FinishItemA());
    }
    IEnumerator FinishItemA()
    {
        yield return new WaitForSeconds(_itemAEffectTime);        //もとに戻す
        Debug.Log("効果終了");
        ItemAEffectRate = 1;
    }
    //ブロックを持っている状態でクリック
    void UseItemB()
    {
        if (!Input.GetMouseButton(0) &&hasItemB) return; 
        // //ブロックすたっくUIリセット
        playerBlockStack.ResetBlock();
        //ブロックスタックリセット
        _uiHandler.ResetStackImage();
        _uiHandler.ResetItemImage();
        hasItemB = false;
    }
    //ブロックを持っている状態でクリック
    void UseItemC()
    {
        if (!Input.GetMouseButton(0) &&hasItemC) return; 
        // //ブロックすたっくUIリセット
        playerBlockStack.ResetBlock();
        //ブロックスタックリセット
        _uiHandler.ResetStackImage();
        _uiHandler.ResetItemImage();
        hasItemC = false;
    }
    
}
