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
    [SerializeField] private Image _blockFrame;
    private float BigAmbrasScale = 1.5f;
    private Vector3 initBlockFrameScale;
    private Vector3 initBlockScale;
    [SerializeField] private GameObject CreateBigBlockEffect;
    [SerializeField] private GameObject CreateCBlockEffect;

    [SerializeField] private GameObject CreateItemEffect;
    private void Start()
    {
        foreach (var image in _StackImage)
        {
            image.sprite = null;
            image.enabled = false;
        }
        _itemImage.sprite = null;
        _itemImage.enabled = false;
        _BlockImage.sprite = null;
        _BlockImage.enabled = false;
        initBlockFrameScale = _blockFrame.transform.localScale;
        initBlockScale = _BlockImage.transform.localScale;
        CreateBigBlockEffect.SetActive(false);
        CreateCBlockEffect.SetActive(false);

    }

    // Update is called once per frame
    //保持しているブロック画像表示
    public void SetBlockImage(string _spriteName)
    {
        //表示される画像　アンブラス　bigアンブラス アイテムCブロックy
        _BlockImage.enabled = true;
         if (_spriteName == "Ambras") _BlockImage.sprite = _ambrasSprite;
         else if (_spriteName == "ItemC")
         {
             _BlockImage.sprite = _itemCSprite;
             CreateCBlockEffect.SetActive(true);
         }
         
         else if (_spriteName == "BigAmbras")
         {
             //くものエフェクト
             
             //大きくする
             CreateBigBlockEffect.SetActive(true);
             _BlockImage.sprite = _ambrasSprite;
             _blockFrame.transform.localScale = initBlockFrameScale * BigAmbrasScale;
             _BlockImage.transform.localScale =initBlockScale * BigAmbrasScale;
         }
         else Debug.LogError("NUllBLockName");
        //     _BlockImage.color = Color.white;
        // }
    }
    //保持しているブロック画像null
    public void ResetBlockImage()
    {
        _BlockImage.enabled = false;
        _blockFrame.transform.localScale = initBlockFrameScale;
        _BlockImage.transform.localScale =initBlockScale;
        _BlockImage.sprite = null;
    }
    //アンブラスとヘイロスのスプライトを格納
    public void SetStackImage(string _objName)
    {
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
        _itemImage.enabled = true;
        CreateItemEffect.SetActive(true);
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
    }
}
