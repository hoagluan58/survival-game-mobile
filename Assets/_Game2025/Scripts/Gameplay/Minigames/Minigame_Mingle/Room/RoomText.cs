using DG.Tweening;
using Redcode.Extensions;
using SquidGame.LandScape.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.MinigameMingle
{
    public enum TextColor
    {
        Red,
        Green,
        Yellow
    }

    public class RoomText : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Color _redColor;
        [SerializeField] private Color _greenColor;
        [SerializeField] private Color _yellowColor;
        [SerializeField] private GameObject _spinGo;
        [SerializeField] private RectTransform _roller;//210 - 0
        [SerializeField] private TextMeshProUGUI _targetText;
        [SerializeField] private TextMeshProUGUI _firstText;
        [SerializeField] private List<TextMeshProUGUI> _bodyTexts;

        public RoomText SetTextAnimation(int target,float duration = 3,UnityAction onComplted = null)
        {
            SetColor(TextColor.Green);
            _text.text = target.ToString();
            _text.gameObject.SetActive(false);

            _spinGo.SetActive(true);
            _firstText.SetText("?");
            _targetText.SetText(target.ToString());
            _bodyTexts.ForEach(text => text.SetText(UnityEngine.Random.Range(1, 6).ToString()));
            SetColor(TextColor.Green);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG03_RANDOM);

            _roller.SetAnchoredPositionY(210);
            _roller.DOAnchorPosY(0,duration).OnComplete(() => {
                
                onComplted?.Invoke();
                _text.gameObject.SetActive(true);
                _spinGo.SetActive(false);
            });
            return this;
        }

        public RoomText SetText(string text)
        {
            _text.text = text;
            return this;
        }

        public RoomText SetColor(TextColor textColor)
        {
            switch (textColor)
            {
                case TextColor.Red:
                    _text.color = _redColor;
                    break;
                case TextColor.Green:
                    _text.color = _greenColor;
                    break;
                case TextColor.Yellow:
                    _text.color = _yellowColor;
                    break;
            }

            return this;
        }
    }
}
