using System.Collections.Generic;
using System.Linq;
using SquidGame.Minigame19;
using UnityEngine;
using UnityEngine.Pool;

namespace SquidGame.Minigame19
{
    public class CannonManager : MonoBehaviour
    {
        [SerializeField] private List<CannonController> _cannonList;
        [SerializeField] private CannonBullet _canonBullet;

        private ObjectPool<CannonBullet> _bulletPool;

        private bool _isActive;
        private float _aliveTimer;
        private int _difficulty;

        public void Init()
        {
            // Init data
            _bulletPool = new ObjectPool<CannonBullet>(
                () => Instantiate(_canonBullet, transform),
                bullet => bullet.gameObject.SetActive(true),
                bullet => bullet.gameObject.SetActive(false),
                Destroy
            );
            _cannonList.ForEach(c => c.Init(_bulletPool));
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            if (_isActive)
            {
                _aliveTimer = 0;
            }
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;

            _aliveTimer += Time.fixedDeltaTime;
            if (_aliveTimer >= 2f)
            {
                _aliveTimer = 0;


                if (_difficulty < 3)
                {
                    SetActiveRandomCannon(1);
                }
                else if (_difficulty >= 3 && _difficulty < 7)
                {
                    SetActiveRandomCannon(2);
                }
                else if (_difficulty >= 7 && _difficulty < 10)
                {
                    SetActiveRandomCannon(3);
                }
                else if (_difficulty >= 10)
                {
                    SetActiveRandomCannon(4);
                }

                _difficulty++;
            }
        }

        private void SetActiveRandomCannon(int amount)
        {
            var inactiveCannons = _cannonList.Where(c => !c.gameObject.activeSelf).ToList();
            int count = Mathf.Min(amount, inactiveCannons.Count);

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, inactiveCannons.Count);
                var cannon = inactiveCannons[randomIndex];
                cannon.gameObject.SetActive(true);
                inactiveCannons.RemoveAt(randomIndex);
            }
        }
    }
}