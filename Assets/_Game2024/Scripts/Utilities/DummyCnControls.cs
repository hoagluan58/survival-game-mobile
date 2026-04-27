using UnityEngine;

namespace CnControls
{
    public class Axis
    {
        public float Value { get; set; }
    }

    public class SimpleJoystick : MonoBehaviour
    {
        public Axis HorizintalAxis = new Axis();
        public Axis VerticalAxis = new Axis();

        private void Update()
        {
            HorizintalAxis.Value = Input.GetAxis("Horizontal");
            VerticalAxis.Value = Input.GetAxis("Vertical");
        }

        public void ResetJoystick()
        {
            HorizintalAxis.Value = 0f;
            VerticalAxis.Value = 0f;
        }
    }
}
