using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class NPCItemHandler : MonoBehaviour
    {
        [SerializeField] private NPCEffectHandler _npcEffectHandler;
        private NPCBlockStack _npcBlockStack;
        private UIHandler _uiHandler;
        private bool hasItemA, hasItemB, hasItemC;
        private float _itemAEffectTime = 6.0f;
        public int ItemAEffectRate { get; private set; } = 1;
        private Coroutine _itemACoroutine;
        private NPCObjectmanipulater _npcObjectmanipulater;
        [SerializeField] private GameObject SaiyaEffect;
        public string NextInsBlock { get; private set; } = null;

        public void Start()
        {
            //_uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
            _npcBlockStack = GetComponent<NPCBlockStack>();
            _npcObjectmanipulater = GetComponent<NPCObjectmanipulater>();
            _npcEffectHandler = GetComponent<NPCEffectHandler>();
        }

        public void Update()
        {
            CreateItem();
            UseItemA();
            UseItemB();
            UseItemC();
        }

        public void CreateItem()
        {
            if (!_npcBlockStack.FullStack) return;
            if (hasItemA || hasItemB || hasItemC) return;
            string ItemType = _npcBlockStack.CreateItemType();
            //アイテムUI表示
            //_uiHandler.SetItemImage(ItemType);
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
                default:
                    Debug.Log("Error");
                    break;
            }
            // Debug.Log($"hasItemA: {hasItemA}, hasItemB: {hasItemB}, hasItemC: {hasItemC}");

        }

        public void UseItemA()
        {
            if (!hasItemA) return;
            //アイテムの効果ーージャンプと破壊2倍
            ItemAEffectRate = 2;
            // //ブロックすたっくUIリセット
            _npcBlockStack.ResetBlock();
            //ブロックスタックリセット
            // _uiHandler.ResetStackImage();
            // _uiHandler.ResetItemImage();
            _npcEffectHandler.ChangeSaiya(true);
            hasItemA = false;
            if (_itemACoroutine != null)
            {
                StopCoroutine(_itemACoroutine);
            }

            _itemACoroutine = StartCoroutine(FinishItemA());
        }

        private IEnumerator FinishItemA()
        {
            yield return new WaitForSeconds(_itemAEffectTime); //もとに戻す
            ItemAEffectRate = 1;
            _npcEffectHandler.ChangeSaiya(false);
            _itemACoroutine = null;
        }

        //ブロックを持っている状態でクリック
        public void UseItemB()
        {
            if ( hasItemB && _npcObjectmanipulater.HasBlock)
            {
                // //ブロックすたっくUIリセット
                _npcBlockStack.ResetBlock();
                //ブロックスタックリセット
                // _uiHandler.ResetStackImage();
                // _uiHandler.ResetItemImage();
                //ブロック巨大化エフェクト

                //NextブロックUI差し替え
                //_uiHandler.SetBlockImage("BigAmbras");

                hasItemB = false;
                NextInsBlock = "BigBlock";
            }
        }

        //ブロックを持っている状態でクリック
        public void UseItemC()
        {
            if ( hasItemC && _npcObjectmanipulater.HasBlock)
            {
                // //ブロックすたっくUIリセット
                _npcBlockStack.ResetBlock();
                //NextブロックUI差し替え
                //_uiHandler.SetBlockImage("ItemC");
                //ブロックスタックリセット
                // _uiHandler.ResetStackImage();
                // _uiHandler.ResetItemImage();
                hasItemC = false;
                NextInsBlock = "ItemCBlock";
            }
        }

        public void ResetNextInsBlock()
        {
            NextInsBlock = null;
        }
    }
}
