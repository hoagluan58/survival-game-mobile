using System.Collections;
using System.Collections.Generic;
using Redcode.Extensions;
using TMPro;
using UnityEngine;

namespace SquidGame.LandScape
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject healthBarObject;
        [SerializeField] private SpriteRenderer _barSprite;
        [SerializeField] private TMP_Text _healthText;

        [SerializeField] private bool _isBot;

        Camera _mainCamera;
        int _maxHealth;
        // float _maxWidth;

        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_isBot)
            {
                _barSprite.color = Color.red;
                healthBarObject.SetActive(false);
            }

            // _maxWidth = _barSprite.size.x;
        }

        public void Init(int maxHealth)
        {
            _maxHealth = maxHealth;
            _healthText.text = $"{_maxHealth}/{_maxHealth}";
            _barSprite.size = new Vector2(1.92f, _barSprite.size.y);

        }

        private void LateUpdate()
        {
            transform.LookAt(_mainCamera.transform);
        }

        public void UpdateUI(int currentHealth, float value)
        {
            if (!healthBarObject.activeSelf)
                healthBarObject.SetActive(true);

            _healthText.text = $"{currentHealth}/{_maxHealth}";
            _barSprite.size = new Vector2(1.92f * value, _barSprite.size.y);
        }
    }
}
