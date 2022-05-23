using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TW
{
    public class SaveVolume : MonoBehaviour
    {
        public float general = 1, effects = 1, music = 1;
        public int sceneIndex;

        public int resolutionIndex;

        public bool fullscreen = true;
        public bool bloom = true;
        public bool ambientOcclusion = true;
        public bool vignette = true;
        public bool dephtOfView = true;

        private void Save()
        {
            PlayerData data = SaveSystem.LoadPlayer();
            Resolution[] resolutions = Screen.resolutions;

            resolutionIndex = resolutions.Length - 1;
            if (data != null)
            {

                general = data.general;
                effects = data.effects;
                music = data.music;
                sceneIndex = data.scene;

                fullscreen = data.fullscreen;
                bloom = data.bloom;
                ambientOcclusion = data.ambientOcclusion;
                vignette = data.vignette;
                dephtOfView = data.dephtOfView;

                resolutionIndex = data.resolutionIndex;

                int buildIndex = SceneManager.GetActiveScene().buildIndex;

                if (buildIndex > 1)
                    sceneIndex = buildIndex;
            }
            SaveSystem.Save(this);

        }

        private void Awake()
        {
            Save();
        }

        private void OnLevelWasLoaded(int level)
        {
            /* Resolution[] resolutions = Screen.resolutions;
             resolutionIndex = resolutions.Length - 1;

             PlayerData data = SaveSystem.LoadPlayer();

             if (data != null)
             {
                 general = data.general;
                 effects = data.effects;
                 music = data.music;
                 sceneIndex = data.scene;



                 fullscreen = data.fullscreen;
                 bloom = data.bloom;
                 ambientOcclusion = data.ambientOcclusion;
                 vignette = data.vignette;
                 dephtOfView = data.dephtOfView;

                 resolutionIndex = data.resolutionIndex;

                 Debug.Log("data: " + data.resolutionIndex);
                 Debug.Log("saveVolume: " + resolutionIndex);

                 int buildIndex = SceneManager.GetActiveScene().buildIndex;

                 if (buildIndex > 1)
                     sceneIndex = buildIndex;
             }
             */
            Save();

            SaveSystem.Save(this);
        }

        void Update()
        {
            DontDestroyOnLoad(this.gameObject);

        }

        public void SaveValues(float general, float effects, float music)
        {
            this.general = general;
            this.effects = effects;
            this.music = music;
        }

        public void SaveGraphcs(int resolutionIndex)
        {
            this.resolutionIndex = resolutionIndex;
        }

    }
}