using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame
{
    public static class AnimUtils
    {
        public static void DOMoveScreenEdge(this RectTransform rect, float duration = 0.5f, bool isMoveX = false, Action onComplete = null)
        {
            rect?.DOKill();
            if (isMoveX)
            {
                rect.DOAnchorPosX(-Screen.width, duration).From().OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
            }
            else
            {
                rect.DOAnchorPosY(Screen.height, duration).From().OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
            }
        }

        public static void DOPunchScalePopup(this RectTransform rect, float punch = 0.15f, float duration = 0.15f, Action onComplete = null)
        {
            rect?.DOKill();
            rect.localScale = Vector3.one;
            rect.DOPunchScale(Vector3.one * punch, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public static void DOPunch(this GameObject gameObject, float punch = 0.15f, float duration = 0.15f, Action onComplete = null)
        {
            gameObject.transform.DOKill();
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.DOPunchScale(Vector3.one * punch, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }


        public static void DOScaleLoop(this GameObject gameObject, Vector3 from, Vector3 to, float duration)
        {
            gameObject.transform.DOKill();
            gameObject.transform.localScale = from;
            gameObject.transform.DOScale(to, duration).SetLoops(-1, LoopType.Yoyo);
        }

        public static void DOScaleShow(this GameObject gameObject, Action onCompleted = null)
        {
            if (gameObject.activeSelf) return;

            gameObject.SetActive(true);
            gameObject.transform.DOKill();
            gameObject.transform.DOScale(Vector3.zero, 0.2f).From().SetEase(Ease.OutBack).OnComplete(() => onCompleted?.Invoke());
        }

        public static bool IsAgentReachDestination(this NavMeshAgent agent)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
