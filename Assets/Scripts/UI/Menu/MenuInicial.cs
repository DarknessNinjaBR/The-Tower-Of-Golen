using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

namespace TW {
    public class MenuInicial : MonoBehaviour
    {
        Resolution[] resolutions;
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown qualityDropdown;
        public GameObject menuOpcoes;

        public Slider slideGeral;
        public Slider slideEffects;
        public Slider slideMusic;
        public SaveVolume saveVolume;

        public int resolutionIndex;

        public Toggle bloom;
        public Toggle ao;
        public Toggle vignette;
        public Toggle dephtOfField;
        public Toggle fullscreen;

        public Slider loadingBar;

        public PostProcessVolume postProcess;

        private void Start()
        {

            postProcess = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();

            saveVolume = FindObjectOfType<SaveVolume>();
            slideGeral = GameObject.Find("Geral").GetComponent<Slider>();
            slideEffects = GameObject.Find("Efeitos").GetComponent<Slider>();
            slideMusic = GameObject.Find("Musica").GetComponent<Slider>();

            Debug.Log(saveVolume.general);
            slideGeral.value = saveVolume.general;
            SetVolumeGeneral(saveVolume.general);

            slideEffects.value = saveVolume.effects;
            SetVolumeEffects(saveVolume.effects);

            slideMusic.value = saveVolume.music;
            SetVolumeMusic(saveVolume.music);

            bloom = GameObject.Find("Bloom").GetComponent<Toggle>();
            ao = GameObject.Find("AmbientOcclusion").GetComponent<Toggle>();
            vignette= GameObject.Find("Vignette").GetComponent<Toggle>();
            dephtOfField = GameObject.Find("Depth Of Field").GetComponent<Toggle>();
            fullscreen = GameObject.Find("Fullscreen").GetComponent<Toggle>();

            bloom.isOn = saveVolume.bloom;
            SetBloom(saveVolume.bloom);

            ao.isOn = saveVolume.ambientOcclusion;
            SetAmbientOcclusion(saveVolume.ambientOcclusion);

            vignette.isOn = saveVolume.vignette;
            SetVignette(saveVolume.vignette);

            dephtOfField.isOn = saveVolume.dephtOfView;
            SetDepthOfField(saveVolume.dephtOfView);

            fullscreen.isOn = saveVolume.fullscreen;
            SetFullscreen(saveVolume.fullscreen);

            /*
             PlayerData data = SaveSystem.LoadPlayer();
            if (data != null)
            {
                general = data.general;
                effects = data.effects;
                music = data.music;


                slideGeral.value = data.general;
                slideEffects.value = data.effects;
                slideMusic.value = data.music;

                bloom.isOn = 

                Time.timeScale = 1;
                resolutions = Screen.resolutions;
                resolutionDropdown.ClearOptions();
            }*/



            Time.timeScale = 1;
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            qualityDropdown = GameObject.Find("Grafico").GetComponent<TMP_Dropdown>();
            qualityDropdown.value = QualitySettings.GetQualityLevel();

            resolutionDropdown.AddOptions(options);
            //resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            resolutionIndex = saveVolume.resolutionIndex;
            resolutionDropdown.value = saveVolume.resolutionIndex;
            saveVolume.SaveGraphcs(resolutionIndex);
            SetResolution(resolutionIndex);

            menuOpcoes = GameObject.Find("MenuOpcoes");

            menuOpcoes.SetActive(false);
        }

        public void Jogar(int sceneIndex)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
            //SceneManager.LoadScene("Andar 1", LoadSceneMode.Single);
        }

        public void Continuar()
        {
            PlayerData data = SaveSystem.LoadPlayer();

            if (data != null)
            {
                StartCoroutine(LoadAsynchronously(data.scene));
            }
            else
            {
                StartCoroutine(LoadAsynchronously(1));
            }

            /*DontDestroyOnLoad(this.gameObject);
            PlayerData data = SaveSystem.LoadPlayer();
            SceneManager.LoadScene(data.scene, LoadSceneMode.Single);
            Destroy(gameObject);*/

        }

        IEnumerator LoadAsynchronously (int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingBar.value = progress;

                yield return null;
            }
        }

        public void Sair()
        {
            SaveVolume pl = FindObjectOfType<SaveVolume>();
            SaveSystem.Save(pl);
            Application.Quit();
        }

        void CursorShow()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            saveVolume.resolutionIndex = resolutionIndex;
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            CursorShow();
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            saveVolume.fullscreen = isFullscreen;
            CursorShow();
        }

        public void SetBloom(bool enabled)
        {
            postProcess.profile.GetSetting<Bloom>().active = enabled;
            saveVolume.bloom = enabled;
        }
        public void SetVignette(bool enabled)
        {
            postProcess.profile.GetSetting<Vignette>().active = enabled;
            saveVolume.vignette = enabled;
        }
        public void SetDepthOfField(bool enabled)
        {
            postProcess.profile.GetSetting<DepthOfField>().active = enabled;
            saveVolume.dephtOfView = enabled;
        }
        public void SetAmbientOcclusion(bool enabled)
        {
            postProcess.profile.GetSetting<AmbientOcclusion>().active = enabled;
            saveVolume.ambientOcclusion = enabled;
        }

        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
            CursorShow();
        }

        public AudioMixer audioMixer;

        public void SetVolumeGeneral(float volume)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
            saveVolume.general = volume;
            CursorShow();

        }
        public void SetVolumeEffects(float volume)
        {
            audioMixer.SetFloat("Effects", Mathf.Log10(volume) * 20);
            saveVolume.effects = volume;
            CursorShow();

        }
        public void SetVolumeMusic(float volume)
        {
            audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            saveVolume.music = volume;
            CursorShow();

        }
    }
}
