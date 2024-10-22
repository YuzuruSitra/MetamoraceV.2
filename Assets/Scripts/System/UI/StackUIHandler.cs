using Character;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class StackUIHandler : MonoBehaviour
    {
        [SerializeField] private Image[] _stackImage;
        [SerializeField] private Sprite _ambrasSprite, _herosSprite, _itemCSprite;
        [SerializeField] private Image _insBlockImage;
        [SerializeField] private Image _insBlockFrame;
        [SerializeField] private Sprite[] _itemSprites;
        [SerializeField] private Image _itemImage;
        
        [SerializeField] private GameObject _getItemEffect;
        [SerializeField] private GameObject _changeBlockEffect;
        
        private Vector2 _initialBlockFrameScale;
        private Vector2 _initialBlockFramePos;
        [SerializeField] private Vector2 _bigScale;
        [SerializeField] private Vector2 _bigPos;
        private bool _currentIsFrame;
        
        private void Start()
        {
            foreach (var t in _stackImage) t.enabled = false;
            _insBlockImage.enabled = false;
            _itemImage.enabled = false;
            _getItemEffect.SetActive(false);
            _changeBlockEffect.SetActive(false);
            _initialBlockFrameScale = _insBlockFrame.transform.localScale;
            _initialBlockFramePos = _insBlockFrame.rectTransform.anchoredPosition;
        }

        public void ChangeInsBlockImage(string block)
        {
            switch (block)
            {
                case "Heros":
                    _insBlockImage.enabled = true;
                    _insBlockImage.sprite = _herosSprite;
                    ChangeFrameSize(false);
                    break;
                case "BigHeros":
                    _changeBlockEffect.SetActive(true);
                    _insBlockImage.enabled = true;
                    _insBlockImage.sprite = _herosSprite;
                    ChangeFrameSize(true);
                    break;
                case "ItemCBlock":
                    _changeBlockEffect.SetActive(true);
                    _insBlockImage.enabled = true;
                    _insBlockImage.sprite = _itemCSprite;
                    ChangeFrameSize(false);
                    break;
                case CharacterObjStacker.NullKey:
                    _insBlockImage.enabled = false;
                    ChangeFrameSize(false);
                    return;
            }
        }

        private void ChangeFrameSize(bool isBig)
        {
            if (_currentIsFrame == isBig) return;
            if (isBig)
            {
                _insBlockFrame.transform.localScale = _bigScale;
                _insBlockFrame.rectTransform.anchoredPosition = _bigPos;
            }
            else
            {
                _insBlockFrame.transform.localScale = _initialBlockFrameScale;
                _insBlockFrame.rectTransform.anchoredPosition = _initialBlockFramePos;
            }
            _currentIsFrame = isBig;
        }

        public void ChangeStackImage(string[] stacks)
        {
            for (var i = 0; i < stacks.Length; i++)
            {
                var t = stacks[i];
                switch (t)
                {
                    case "Ambras":
                        _stackImage[i].enabled = true;
                        _stackImage[i].sprite = _ambrasSprite;
                        break;
                    case "Heros":
                    case "BigHeros":
                    case "ItemCBlock":
                        _stackImage[i].enabled = true;
                        _stackImage[i].sprite = _herosSprite;
                        break;
                    case CharacterObjStacker.NullKey:
                        _stackImage[i].enabled = false;
                        _stackImage[i].sprite = null;
                        break;
                        default:
                        return;
                }
            }
        }
        
        public void ChangeItemImage(CharacterItemHandler.Item item)
        {
            var itemNum = (int)item;
            var hasItem = itemNum != 0;

            _getItemEffect.SetActive(hasItem);
            _itemImage.enabled = hasItem;

            if (hasItem) _itemImage.sprite = _itemSprites[itemNum];
        }
    }
}
