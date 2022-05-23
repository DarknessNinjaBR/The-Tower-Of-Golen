using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TW
{
    public class PauseMenu : MonoBehaviour
    {
        InputManager inputManager;

        public Slider loadingBar;

        public PlayerManager playerManager;

        public GameObject loadingScreen;
        public GameObject optionsScreen;
        public GameObject pauseScreen;
        public GameObject deathScreen;

        private void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();

            if(pauseScreen != null)
                pauseScreen.SetActive(false);
        }

        private void Update()
        {
            bool dead = false;

            if (inputManager == null && !dead)
                inputManager = FindObjectOfType<InputManager>();

            if (playerManager == null && deathScreen != null)
            {
                dead = true;
                deathScreen.SetActive(true);
                return;
            }

            if (inputManager.pause_input)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
            }

        }
        public void Sair()
        {
            GameObject saveV;
            saveV = FindObjectOfType<SaveVolume>().gameObject;
            Destroy(saveV);

            StartCoroutine(LoadAsynchronously(0));
            SaveVolume pl = FindObjectOfType<SaveVolume>();
            SaveSystem.Save(pl);
        }

        public void Novamente()
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadAsynchronously(sceneIndex));
            //PlayerMovement pl = FindObjectOfType<PlayerMovement>();
            //SaveSystem.Save(pl);
        }

        public void Vitoria(int sceneIndex)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }

        IEnumerator LoadAsynchronously(int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                if (loadingScreen != null)
                    loadingScreen.SetActive(true);

                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingBar.value = progress;

                yield return null;
            }
        }

        public void Continue()
        {
            Time.timeScale = 1;

            loadingScreen.SetActive(false);
            optionsScreen.SetActive(false);
            pauseScreen.SetActive(false);
        }
    }
}
