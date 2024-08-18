using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerEffectHandler _playerEffectHandler;
    private PlayerBlockStack playerBlockStack;
    private UIHandler _uiHandler;
    private bool hasItemA, hasItemB, hasItemC;
    private float _itemAEffectTime = 6.0f;
    public int ItemAEffectRate { get; private set; } = 1;
    private Coroutine _itemACoroutine;
    private PlayerObjectManipulator playerObjectManipulator;
    [SerializeField] private GameObject SaiyaEffect;
    public string NextInsBlock { get; private set; } = null;

    public void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        playerBlockStack = GetComponent<PlayerBlockStack>();
        playerObjectManipulator = GetComponent<PlayerObjectManipulator>();
        _playerEffectHandler = GetComponent<PlayerEffectHandler>();
    }

    public void Update()
    {
      // Debug.Log(playerBlockStack.FullStack);
    }
    
    public void CreateItem()
    {
        if (!playerBlockStack.FullStack) return;
        if (hasItemA || hasItemB || hasItemC) return;
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
            _playerEffectHandler.ChangeSaiya(true);
            hasItemA = false;
            if (_itemACoroutine != null)
            {
                StopCoroutine(_itemACoroutine);
            }
            _itemACoroutine = StartCoroutine(FinishItemA());
    }

    private IEnumerator FinishItemA()
    {
        yield return new WaitForSeconds(_itemAEffectTime);        //もとに戻す
        ItemAEffectRate = 1;
        _playerEffectHandler.ChangeSaiya(false);
        _itemACoroutine = null;
    }
    //ブロックを持っている状態でクリック
    public void UseItemB()
    {
        if (Input.GetMouseButton(1) &&hasItemB && playerObjectManipulator.HasBlock)
        {
            // //ブロックすたっくUIリセット
            playerBlockStack.ResetBlock();
            //ブロックスタックリセット
            _uiHandler.ResetStackImage();
            _uiHandler.ResetItemImage();
            //ブロック巨大化エフェクト
            
            
            hasItemB = false;
            NextInsBlock =  "BigBlock";
        }
    }
    //ブロックを持っている状態でクリック
    public void UseItemC()
    {
        if (Input.GetMouseButton(1) &&hasItemC && playerObjectManipulator.HasBlock)
        {
            // //ブロックすたっくUIリセット
            playerBlockStack.ResetBlock();
            //ブロックスタックリセット
            _uiHandler.ResetStackImage();
            _uiHandler.ResetItemImage();
            hasItemC = false;
            NextInsBlock =  "ItemCBlock";
        }
    }

    public void ResetNextInsBlock()
    {
        NextInsBlock = null;
    }
    
}
