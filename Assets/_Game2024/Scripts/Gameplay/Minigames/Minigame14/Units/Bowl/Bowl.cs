using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace SquidGame.Minigame14
{
    public class Bowl : MonoBehaviour
    {
        public event Action<bool> Clicked;

        [SerializeField] private Transform _bowlModel;

        private bool _isCorrectBowl;
        private bool _isClickable;
        private Transform _marbles;

        public void SetAsCorrectBowl()
        {
            _isCorrectBowl = true;
        }

        public void SetClickable(bool isClickable)
        {
            _isClickable = isClickable;
        }

        public void AddMarbles(Transform marbles)
        {
            _marbles = marbles;
            _marbles.SetParent(this.transform);
            _marbles.DOLocalMove(new Vector3(0, 0, 0.15f), 1f);
        }

        public void OnPlayerClickBowl()
        {
            Clicked?.Invoke(_isCorrectBowl);
            _bowlModel.DOKill();
            _bowlModel.DOLocalRotate(Vector3.right * 180, 0.5f, RotateMode.LocalAxisAdd);
            var point1 = new Vector3(0, 0.25f, -0.2f);
            var point2 = new Vector3(0, 0.5f, 0.15f);
            _bowlModel.DOLocalPath(new[] { point1, point2 }, 0.5f, PathType.CatmullRom);
        }

        private void Update()
        {
            if (_isClickable)
            {
#if UNITY_EDITOR
                // For Unity Editor, check for mouse input
                if (Input.GetMouseButtonDown(0))
                {
                    ProcessClick();
                }
#elif UNITY_ANDROID || UNITY_IOS
    // For Android and iOS, check for touch input
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    {
        ProcessClick();
    }
#endif
            }
        }

        private void ProcessClick()
        {
            // Create a ray from the camera to the mouse/touch position
            Vector3 inputPosition =
#if UNITY_EDITOR
                (Vector3)Input.mousePosition; // Mouse position in Unity Editor
#else
        Input.GetTouch(0).position;    // Touch position on mobile
#endif

            Ray ray = Camera.main.ScreenPointToRay(inputPosition);

            // Perform the raycast and get all hit results
            RaycastHit[] hits = Physics.RaycastAll(ray);

            // Check if any objects were hit
            if (hits.Length <= 0) return;

            // If this object's transform was hit, call the appropriate function
            if (hits.Any(hit => hit.transform == this.transform))
            {
                OnPlayerClickBowl();
            }
        }

        public void UpsideDown()
        {
            _bowlModel.DOLocalRotate(Vector3.zero, 0.5f);

            var point1 = new Vector3(0, 0.25f, -0.2f);
            var point2 = new Vector3(0, 0, -0.1f);
            _bowlModel.DOLocalPath(new[] { point1, point2 }, 0.5f, PathType.CatmullRom);
        }
    }
}