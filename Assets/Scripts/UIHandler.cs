using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Image[] _StackImage = new Image[3];
    [SerializeField] Sprite _herosSprite, _ambrasSprite, _itemCSprite;
    [SerializeField] Image _BlockImage;
    [SerializeField]  Sprite ItemA, ItemB, ItemC;
    [SerializeField] Image _itemImage;
    private Color transparent = new Color(0f, 0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //保持しているブロック画像表示
    public void BlockImage(string objName)
    {
        if (objName == "Ambras")
        {
            _BlockImage.sprite = _ambrasSprite;
            _BlockImage.color = Color.white;
        }
        else
        {
            _BlockImage.sprite = _herosSprite;
            _BlockImage.color = Color.white;
        }
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
        for (int i = 0; i < _StackImage.Length; i++)
        {
            if (_StackImage[i].sprite == null)
            {
                if (_objName == "Ambras")
                {
                    _StackImage[i].sprite = _ambrasSprite;
                    _StackImage[i].color = Color.white;
                }
                else if (_objName == "Heros")
                {
                    _StackImage[i].sprite = _herosSprite;
                    _StackImage[i].color = Color.white;
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
           // _StackImage[i].color = toumei;
        }
    }

    public void SetItemImage(string ItemType)
    {
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
        _itemImage.sprite = null;
        //_itemImage.color = transparent;
    }
}
