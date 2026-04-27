using NFramework;
using System;
using UnityEngine;

namespace SquidGame.LandScape.MinigameMarblesVer2
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected Transform HeadTf;
        [SerializeField][ReadOnly] protected int MarbleCount;

        public event Action OnEndTurn;
        public event Action OnStartTurn;

        protected Marblesver2Controller Controller;
        public Transform Head() => HeadTf;
        public int TotalScore { get; set; }
        public int CurrentTurnScore { get; set; }
        public virtual void Init(Marblesver2Controller controller)
        {
            Controller = controller;
        }
        public abstract void StartTurn();
        public abstract void EndTurn();
        public abstract void OnWin();
        public abstract void OnLose();
        public void InvokeStartTurn() => OnStartTurn?.Invoke();
        public void InvokeEndTurn() => OnEndTurn?.Invoke();
        public void SetMarble(int max) => MarbleCount = max;
        public void RemoveMarble() => MarbleCount = Mathf.Max(MarbleCount - 1, 0);
        public bool IsCompleted() => MarbleCount == 0;
    }
}
