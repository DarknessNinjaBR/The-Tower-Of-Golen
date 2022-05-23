using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TW
{
    public class HealthBar : MonoBehaviour
    {
        public Slider bar;

        private void Awake()
        {
            bar = GetComponent<Slider>();
        }

        public void SetBar(int value)
        {
            bar.maxValue = value;
            bar.value = value;
        }

        public void UpdateBar(int value)
        {
            bar.value = value;
        }

    }
}
