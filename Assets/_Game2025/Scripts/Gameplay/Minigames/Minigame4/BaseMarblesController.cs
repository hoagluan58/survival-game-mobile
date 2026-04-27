using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SquidGame.LandScape.Minigame4
{
    public class BaseMarblesController : MonoBehaviour
    {
        [Header("Configs Parent")]
        [SerializeField] protected int _marblesCount;
        [SerializeField] protected int _marblesScored;
        [SerializeField] protected bool _isInHole;
        [SerializeField] protected Transform _head;

        public Transform Head => _head;
        public UnityAction<bool> OnEndTurn;
        public bool IsCompleted()
        {
            return _marblesCount == 0;
        }

        public virtual void StartTurn()
        {
            _isInHole = false;
        }

        public virtual void EndTurn()
        {
            OnEndTurn?.Invoke(_isInHole);
        }


        public virtual void ResetMarbles(int marblesCount = 3)
        {
            _marblesCount = marblesCount;
            _marblesScored = 0;
        }


        public virtual void ScoredMarble()
        {
            _marblesScored++;
        }

        public int GetScore()
        {
            return _marblesScored;
        }


        public virtual void Dance()
        {
            Debug.Log("Dancing");
        }


        public virtual void Dead()
        {
            Debug.Log("Dead");
        }
    }
}
