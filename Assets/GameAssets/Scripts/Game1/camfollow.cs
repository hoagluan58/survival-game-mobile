using UnityEngine;

namespace Game1
{
    public class camfollow : MonoBehaviour
    {
        GameObject playercContainer;
        Vector3 offset;
        public float speed;
        public Transform winfollow;
        PlayerController playercont;

        private bool _isLookAtTarget;

        void Start()
        {
            playercont = FindObjectOfType<PlayerController>();
            if (playercont != null)
            {
                playercContainer = playercont.gameObject;
                offset = transform.position - playercContainer.transform.position;
            }
        }

        void Update()
        {
            if (playercContainer != null)
            {
                transform.position = Vector3.Lerp(transform.position, playercContainer.transform.position + offset, speed * Time.deltaTime);

                if (_isLookAtTarget)
                    transform.LookAt(playercContainer.transform.position + Vector3.up);
            }
        }

        public void ActiveLookAtTarget()
        {
            offset.y -= 1f;
            _isLookAtTarget = true;
        }
    }

}