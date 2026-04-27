using NFramework;
using UnityEngine;

namespace SquidGame.LandScape.Survival
{
    public class ConfigController : SingletonMono<ConfigController>
    {
        [SerializeField] private BotSpawnSO _botSpawnSO;
        [SerializeField] private WeaponAmountSO _weaponAmoutSO;
        [SerializeField] private WeaponStatsSO _weaponStatsSO;

        public BotSpawnSO BotSpawnSO => _botSpawnSO;
        public WeaponAmountSO WeaponAmountSO => _weaponAmoutSO;
        public WeaponStatsSO WeaponStatsSO => _weaponStatsSO;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Init()
        {
            _botSpawnSO?.Init();
            _weaponAmoutSO?.Init();
            _weaponStatsSO?.Init();
        }
    }
}
