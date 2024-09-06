using UnityEngine;

namespace Character
{
    public class BigBlockInsClamper
    {

        public float ClampValueX(float value)
        {
            float clampedValue = Mathf.Floor(value) + 0.5f;
            if (clampedValue > 6.5f)
            {
                return 6.5f;
            }
            else if (clampedValue < -6.5f)
            {
                return -6.5f;
            }

            return clampedValue;
        }

        public float ClampValueY(float value)
        {
            value = Mathf.Round(value);
            if (value >= 11.0f) value = 11.0f;
            return value;
        }

    }
}
