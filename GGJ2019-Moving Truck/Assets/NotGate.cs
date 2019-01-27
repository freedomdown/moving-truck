using UnityEngine;
using System.Collections;

namespace RyanDStandard
{
    namespace Events
    {
        [ExecuteInEditMode]
        public class NotGate : MonoBehaviour
        {
            [Header("if(input)then(!output)")]
            [Tooltip("Thing that will get disabled by this script")]
            public GameObject output;
            [Tooltip("Thing that will trigger this script")]
            public GameObject input;
            [Tooltip("How many times this gate has been triggered")]
            public int counter = 0;
            [Tooltip("Should it costantly check both ways")]
            public bool twoWay = false;

            // Use this for initialization
            void Start()
            {
                if (output == null)
                    output = gameObject;//default output is self
            }

            // Update is called once per frame
            void Update()
            {
                if (Application.isPlaying)
                    if (input != null)
                    {
                        if (input.activeInHierarchy)//if the target is active
                        {
                            counter++;//keep a count
                            output.SetActive(false);//disable output
                        }
                        if (twoWay)//if it is a two way gate (reversable)
                        {
                            if (input.activeInHierarchy == false)//if target is not active
                            {
                                if (output.activeInHierarchy == false)//and we are still inactive
                                {
                                    counter++;
                                    output.SetActive(true);//enable output
                                }
                            }
                        }
                    }
            }
        }
    }
}