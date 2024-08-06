using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    private PlayerBlockStack playerBlockStack;
    private UIHandler _uiHandler;

    public void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        playerBlockStack = GetComponent<PlayerBlockStack>();
    }
    public void CreateItem()
    {
        if (!playerBlockStack.FullStack) return;
        Debug.Log(playerBlockStack.FullStack);
        string ItemType = playerBlockStack.CreateItemType();
       
            //アイテムUI表示
            _uiHandler.SetItemImage(ItemType);
            //アイテムフラグオン
            
            // //ブロックすたっくUIリセット
            // playerBlockStack.ResetBlock();
            // //ブロックスタックリセット
            // _uiHandler.ResetStackImage();
    }

    

    // void UseItem()
    // {
    //     
    // }
    
}
