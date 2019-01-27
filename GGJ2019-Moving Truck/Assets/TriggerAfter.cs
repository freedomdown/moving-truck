using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RyanDStandard
{
    namespace Events
    {
        public class TriggerAfter : MonoBehaviour
        {
            public float Delay = 1f;
            private float spawnTime = 0f;
            public UnityEvent triggerEvent;

            // Use this for initialization
            void Start()
            {
                spawnTime = Time.time;
            }

            // Update is called once per frame
            void Update()
            {
                if (Time.time > spawnTime + Delay)
                {
                    triggerEvent.Invoke();
                }
            }
        }
    }
}