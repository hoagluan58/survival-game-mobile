using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SquidGame.LandScape.Minigame6
{
    public class MinigameIntro : MonoBehaviour
    {
        [SerializeField] private CinemachineDollyCart _dollyCart;
        [SerializeField] private CinemachinePath _path;
        [SerializeField] private float _speed;

        public async UniTask PlayIntro()
        {
            _dollyCart.gameObject.SetActive(true);
            _dollyCart.m_Speed = _speed;
            await UniTask.WaitUntil(() => IsReachEndOfPath());
            _dollyCart.gameObject.SetActive(false);
        }

        public bool IsReachEndOfPath()
        {
            return _path.PathLength == _dollyCart.m_Position;
        }
    }
}
