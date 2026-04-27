using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game6
{
    public class PanelInGame : PanelBase
    {
        [SerializeField] private UIHPBar _playerHPBar;
        [SerializeField] private UIHPBar _enemyHPBar;
        [SerializeField] private TextMeshProUGUI _tmpEnemyName;
        [SerializeField] private GameObject _warning;
        [SerializeField] private GameObject _tutFight;

        public UIHPBar PlayerHPBar { get { return _playerHPBar; } }
        public UIHPBar EnemyHPBar { get { return _enemyHPBar; } }

        private bool _isForceHideWarning;

        private void Start()
        {
            _tmpEnemyName.text = "No." + Random.Range(0, 500);
        }

        public void UpdateUIPlayerHpBar(float val)
        {
            if (_isForceHideWarning == false)
            {
                if (val <= 0.25f)
                    SetActiveWarning(true);
                else SetActiveWarning(false);
            }
            _playerHPBar.UpdateUI(val);
        }

        public void SetActiveWarning(bool b, bool isForceHide = false)
        {
            _isForceHideWarning = isForceHide;
            _warning.SetActive(b);
        }

        public void ShowTutFight()
        {
            _tutFight.SetActive(true);
        }
    }
}