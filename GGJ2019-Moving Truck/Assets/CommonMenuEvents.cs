using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanDStandard
{
    namespace Events
    {
        public class CommonMenuEvents : MonoBehaviour
        {
            [Header("public functions you can call from other scripts")]
            public float Timeout = 2f;

            public void SwitchScenesNumber(int SceneNumber)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNumber);
            }
            public void SwitchScenes(string SceneName)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
            }
            public void QuitApplication()
            {
                Application.Quit();
            }
            public void EnableObject(GameObject obj)
            {
                obj.SetActive(true);
            }
            public void WaitThenEnableObject(GameObject obj)
            {
                StartCoroutine(TimerDone(obj));
            }

            private IEnumerator TimerDone(GameObject obj)
            {
                yield return new WaitForSeconds(Timeout);
                EnableObject(obj);
            }
        }
    }
}