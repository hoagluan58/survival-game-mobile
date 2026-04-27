using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape
{
    public class DoorSymbol : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        [Button]
        public void LoadDoor()
        {
            _image = GetComponent<Image>();
        }
    }
}
