using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Image[] _StackImage = new Image[3];
    [SerializeField] private Sprite _herosSprite, _ambrasSprite, _itemCSprite;
    [SerializeField] private Image _BlockImage;
    [SerializeField] private Sprite ItemA, ItemB, ItemC;
    [SerializeField] private Image _itemImage;

    private EffectHandler _effectHandler;

    private void Start()
    {
        _effectHandler = GameObject.FindWithTag("EffectHandler").GetComponent<EffectHandler>();
        foreach (var image in _StackImage)
        {
            image.sprite = null;
            image.enabled = false;
        }
        _itemImage.sprite = null;
        _itemImage.enabled = false;
        _BlockImage.sprite = null;
        _BlockImage.enabled = false;
    }

    // Update is called once per frame
    //保持しているブロック画像表示
    public void SetBlockImage(string _spriteName)
    {
        //表示される画像　アンブラス　bigアンブラス アイテムCブロックy

         if (_spriteName == "Ambras") _BlockImage.sprite = _ambrasSprite;
         else if(_spriteName == "ItemC")   _BlockImage.sprite = _itemCSprite;
          
        //     _BlockImage.color = Color.white;
        // }
    }
    //保持しているブロック画像null
    public void ResetBlockImage()
    {
        _BlockImage.sprite = null;
        //_BlockImage.color = toumei;   
        //_BlockImage.transform.localScale = initialScale;
        //_BlockFrameImage.transform.localScale = frameinitialScale;
    }
    //アンブラスとヘイロスのスプライトを格納
    public void SetStackImage(string _objName)
    {
        Debug.Log("Called");
        for (int i = 0; i < _StackImage.Length; i++)
        {
            if (_StackImage[i].sprite == null)
            {
                if (_objName == "Ambras")
                {
                    _StackImage[i].sprite = _ambrasSprite;
                    _StackImage[i].enabled = true;
                    
                }
                else if (_objName == "Heros")
                {
                    _StackImage[i].sprite = _herosSprite;
                    _StackImage[i].enabled = true;
                }
                else Debug.Log("Error");
                break;
            }
        }
    }
    //アンブラスとヘイロスのスプライトをリセット
    public void ResetStackImage()
    {
        for (int i = 0; i < _StackImage.Length; i++)
        {
            _StackImage[i].sprite = null;
            _StackImage[i].enabled = false;
           // _StackImage[i].color = toumei;
        }
    }

    public void SetItemImage(string ItemType)
    {
        Debug.Log("Called");
        _itemImage.enabled = true;
        _effectHandler.LoadCreateItemEffect();
        switch (ItemType)
        {
            case "ItemA":
                _itemImage.sprite = ItemA;
                break;
            case "ItemB":
                _itemImage.sprite = ItemB;
                break;
            case "ItemC":
                _itemImage.sprite = ItemC;
                break;
            default :
                Debug.Log("Error");
                break;
        }
        
    }

    public void ResetItemImage()
    {
        _itemImage.enabled = false;
        _itemImage.sprite = null;
        //_itemImage.color = transparent;
    }
}
