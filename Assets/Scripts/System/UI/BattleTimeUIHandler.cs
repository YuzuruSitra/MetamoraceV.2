using System.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class BattleTimeUIHandler : MonoBehaviour
    {
        [SerializeField] private TimeHandler _timeHandler;
        [SerializeField] private Image _countDownImage;
        [SerializeField] private Sprite[] _countDowns;
        [SerializeField] private UnityEngine.UI.Text _timeText;

        private void Start()
        {
            _countDownImage.enabled = true;
            _countDownImage.sprite = _countDowns[2];
            UpdateTimeText();
        }
        
        private void Update()
        {
            if (_countDownImage.enabled)
            {
                var count = (int)Math.Ceiling(_timeHandler.CountTime) - 1;
                if (count >= _countDowns.Length - 1) count = _countDowns.Length - 1;
                if (count < 0)
                {
                    _countDownImage.enabled = false;
                    return;
                }
                _countDownImage.sprite = _countDowns[count];
            }

            UpdateTimeText();
        }

        private void UpdateTimeText()
        {
            var minute = (int)Math.Floor(_timeHandler.BattleTime / 60.0f);
            var second = (int)Math.Floor(_timeHandler.BattleTime) % 60;
            _timeText.text = minute + ":" + second.ToString("D2");
        }
    }
}