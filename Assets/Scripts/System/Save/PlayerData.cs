using System.Collections;
using System.Collections.Generic;
using TW;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public int scene;

    public int resolutionIndex;

    public bool fullscreen;
    public bool ambientOcclusion;
    public bool vignette;
    public bool dephtOfView;
    public bool bloom;

    public float general;
    public float effects;
    public float music;

    public PlayerData (SaveVolume save)
    {
        scene = save.sceneIndex;
        Debug.Log("dataSaving: " + scene);

        general = save.general;
        effects = save.effects;
        music = save.music;

        fullscreen = save.fullscreen;
        ambientOcclusion = save.ambientOcclusion;
        bloom = save.bloom;
        dephtOfView = save.dephtOfView;
        vignette = save.vignette;

        resolutionIndex = save.resolutionIndex;


    }
}
