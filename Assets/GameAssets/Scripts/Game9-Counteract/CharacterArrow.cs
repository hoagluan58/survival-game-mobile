using Game9;
using SquidGame.Gameplay;
using UnityEngine;

namespace SquidGame.Minigame09
{
    public class CharacterArrow : MonoBehaviour
    {
        private CharacterControl _character;
        private Game9Control _controller;
        private EnemyBot _curTarget;

        public void Init(Game9Control controller, CharacterControl character)
        {
            _controller = controller;
            _character = character;
            FollowCharacter();
        }

        private void Update()
        {
            if (GameManager.I.CurGameState == EGameState.Playing)
            {
                _curTarget = GetNearestEnemyBot();
                FollowCharacter();
                if (_curTarget != null)
                {
                    RotateArrow(_curTarget.transform.position);
                }
            }
        }

        private void FollowCharacter() => transform.position = _character.transform.position;

        private void RotateArrow(Vector3 targetPosition)
        {
            var direction = targetPosition - transform.position;
            var angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, angleY, 0);
        }

        private EnemyBot GetNearestEnemyBot()
        {
            EnemyBot nearestEnemy = null;
            var shotestDistance = Mathf.Infinity;

            foreach (var enemy in _controller.Enemies)
            {
                if (!enemy.IsActive) continue;
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < shotestDistance)
                {
                    shotestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            return nearestEnemy;
        }

    }
}
