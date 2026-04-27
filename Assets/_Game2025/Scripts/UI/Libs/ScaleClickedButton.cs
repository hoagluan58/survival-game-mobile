using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SquidGame.LandScape
{
    [RequireComponent(typeof(Button))]
    public class ScaleClickedButton : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ScaleUp);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ScaleUp);
        }

        private void ScaleUp()
        {
            _container?.DOPunch();
        }
    }
}
