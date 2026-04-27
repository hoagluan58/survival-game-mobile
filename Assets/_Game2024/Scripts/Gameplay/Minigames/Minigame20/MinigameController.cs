using SquidGame.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.Minigame20
{
    public class MinigameController : BaseMinigameController
    {
        [SerializeField] private MoneyManager _moneyManager;
        [SerializeField] private CarsManager _carsManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private List<BotController> _bots;
        [SerializeField] private List<UnitBase> _bases;

        public override void OnLoadMinigame()
        {
            base.OnLoadMinigame();
            _moneyManager.OnLoadMinigame();
            _carsManager.OnLoadMinigame();
        }

        public override void OnStart()
        {
            base.OnStart();
            _bots.ForEach(b => b.OnStartMinigame());
        }
    }
}
    