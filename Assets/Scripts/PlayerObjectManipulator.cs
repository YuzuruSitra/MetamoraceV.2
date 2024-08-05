using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectManipulator : MonoBehaviour
{
     [SerializeField]
    private float _initialDestroyPower = 1.0f;
    public float InitialDestroyPower => _initialDestroyPower;

    private float _destroyPower;
     private bool _hasBlock = false;
    public bool HasBlock => _hasBlock;
     private Vector3 _insPos;
    private Vector3 _insBigPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         BreakBlock();
        CreateBlock();
    }
     //ブロック生成
    public void CreateBlock()
    {
        //ブロックを持ってれば処理を行う
        if (!_hasBlock) return;
        //if(_animSwing) return;
        if (!Input.GetMouseButtonDown(0)) return;
        //swingAnim再生
       // _animSwing = true;
        _hasBlock = false;
        //_predictCubes.SetActive(false);
        _insBigPos = _insPos;
        _insBigPos.y += 1.0f;
      
        //StartCoroutine(GenerateBlock());
    }
    //  private IEnumerator GenerateBlock()
    // {
    //     yield return new WaitForSeconds(0.4f);
       
    //     //アイテムBを持っていたら巨大ブロック一回だけ生成
    //     if (_itemHandler._HasItemB && _playerItemHandler.UseItemState == 0)
    //     {
    //         //アイテムB微調整
    //         _playerItemHandler.UsedItem();
    //         _itemHandler.ItemEffectB();
    //     }
    //     //ItemCBlock生成
    //     else if (_itemHandler._HasItemC && _playerItemHandler.UseItemState == 1)
    //     {
    //         _playerItemHandler.UsedItem();
    //     }
    //     else
    //     {
           
    //     }
        
    // }
    //オブジェクト破壊
    public void BreakBlock()
    {
        if (_hasBlock || !Input.GetMouseButton(0)) return;
       
        Vector3 direction = transform.forward;
        direction.Normalize();
        
         //_currentBlock.DecreceGage();

        // int objID = _currentBlock.DestroyBlock(_destroyPower);

        // if (objID == UIHandler._ambrassID || objID == UIHandler._herosID)
        // {
        //     //UIに保持しているブロックを表示する処理
        //     _uiHandler.BlockImage(objID);
        //     if (hit.collider.CompareTag("ItemCBlock"))
        //     {
        //         SetItemC(hit.collider);
        //         ProcessItemCBlockEffect();
        //     }
        //     //ブロック破壊SE;
        //     _predictCubes.SetActive(true);
        //     _hasBlock = true;
        //     if (!_itemHandler._HasItemA || !_itemHandler._HasItemB || !_itemHandler._HasItemC) 
        //     {
        //         _itemHandler.StackBlock(objID);
        //         _itemHandler.CreateItem();
        //     }
        //}
    }

}
