using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public Toggle bgmToggle;
    public Toggle sfxToggle;

    public AudioSource[] bgmAudioSources;
    public AudioSource[] sfxAudioSources;

    private bool isBgmEnabled = true;
    private bool isSfxEnabled = true;
    void Start()
    {
        FindAudioSources();
        // Update bgmAudioSources
        GameObject backgroundManagerGO = GameObject.Find("BackgroundManager");
        if (backgroundManagerGO != null)
        {
            bgmAudioSources = backgroundManagerGO.GetComponentsInChildren<AudioSource>();
        }

        // Update sfxAudioSources
        GameObject effectsManagerGO = GameObject.Find("EffectsManager");
        if (effectsManagerGO != null)
        {
            AudioSource[] newSfxAudioSources = effectsManagerGO.GetComponentsInChildren<AudioSource>();
            sfxAudioSources = sfxAudioSources.Concat(newSfxAudioSources).ToArray();
        }

        GameObject enemyContainerGO = GameObject.Find("EnemyContainer");
        if (enemyContainerGO != null)
        {
            // Retrieve AudioSource components from the child objects of the EnemyContainer GameObject
            AudioSource[] newSfxAudioSources = enemyContainerGO.GetComponentsInChildren<AudioSource>(true);
            sfxAudioSources = sfxAudioSources.Concat(newSfxAudioSources).ToArray();
        }

        GameObject bgmusic = GameObject.Find("BGMusic");
        if (bgmusic != null)
        {
            // Retrieve AudioSource components from the child objects of the EnemyContainer GameObject
            AudioSource[] newBgmAudioSources = bgmusic.GetComponentsInChildren<AudioSource>();
            bgmAudioSources = bgmAudioSources.Concat(newBgmAudioSources).ToArray();
        }

        // Set the volume for bgmAudioSources
        if (bgmAudioSources != null)
        {
            foreach (AudioSource bgmSource in bgmAudioSources)
            {
                bgmSource.mute = !bgmToggle.isOn;
            }
        }

        // Set the volume for sfxAudioSources
        if (sfxAudioSources != null)
        {
            foreach (AudioSource sfxSource in sfxAudioSources)
            {
                sfxSource.mute = !sfxToggle.isOn;
            }
        }

        // Initialize toggles
        bgmToggle.isOn = isBgmEnabled;
        sfxToggle.isOn = isSfxEnabled;
    }

    private void FindAudioSources()
    {
        List<AudioSource> newBgmAudioSources = new List<AudioSource>();
        List<AudioSource> newSfxAudioSources = new List<AudioSource>();

        GameObject backgroundManagerGO = GameObject.Find("BackgroundManager");
        if (backgroundManagerGO != null)
        {
            newBgmAudioSources.AddRange(backgroundManagerGO.GetComponentsInChildren<AudioSource>());
        }

        GameObject effectsManagerGO = GameObject.Find("EffectsManager");
        if (effectsManagerGO != null)
        {
            newSfxAudioSources.AddRange(effectsManagerGO.GetComponentsInChildren<AudioSource>());
        }

        GameObject enemyContainerGO = GameObject.Find("EnemyContainer");
        if (enemyContainerGO != null)
        {
            // Retrieve AudioSource components from the child objects of the EnemyContainer GameObject
            AudioSource[] enemyAudioSources = enemyContainerGO.GetComponentsInChildren<AudioSource>(true);

            foreach (AudioSource enemyAudioSource in enemyAudioSources)
            {
                if (enemyAudioSource.gameObject.activeSelf)
                {
                    newSfxAudioSources.Add(enemyAudioSource);
                }
            }
        }

        GameObject bgmusic = GameObject.Find("BGMusic");
        if (bgmusic != null)
        {
            newBgmAudioSources.AddRange(bgmusic.GetComponentsInChildren<AudioSource>());
        }

        bgmAudioSources = newBgmAudioSources.ToArray();
        sfxAudioSources = newSfxAudioSources.ToArray();
    }


    private void Update()
    {
        FindAudioSources();
    }




    public void OnBgmToggleValueChanged()
    {
        isBgmEnabled = bgmToggle.isOn;

        // Toggle the mute state for bgmAudioSources
        if (bgmAudioSources != null)
        {
            foreach (AudioSource bgmSource in bgmAudioSources)
            {
                bgmSource.mute = !isBgmEnabled;
            }
        }
    }

    public void OnSfxToggleValueChanged()
    {
        isSfxEnabled = sfxToggle.isOn;

        // Toggle the mute state for sfxAudioSources
        if (sfxAudioSources != null)
        {
            foreach (AudioSource sfxSource in sfxAudioSources)
            {
                sfxSource.mute = !isSfxEnabled;
            }
        }
    }
}
