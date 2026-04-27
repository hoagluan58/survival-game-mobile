using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape
{
    public class BotAvatar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Image _avatarImage;
        [SerializeField] private GameObject _highlight;

        public void SetInfo(int nameId, Sprite avatarSprite)
        {
            _nameText.text = nameId.ToString("D3");
            _avatarImage.sprite = avatarSprite;
        }

        public void Disable()
        {
            _avatarImage.gameObject.SetActive(false);
            // _deadIcon.SetActive(true);
        }

        public void HightlightPlayerAvatar()
        {
            _avatarImage.gameObject.SetActive(true);
            _highlight.SetActive(true);
        }
    }
}
