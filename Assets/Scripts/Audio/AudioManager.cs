using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Audios
{
    CarStart,
    CarEngine,
    CoffeeSip,
    RadioButton,
    StaticChangeChannel,
    CrashSound,
    Win
}
public enum AudioType
{
    Car,
    Player,
    Environment,
}

[Serializable]
public struct AudioStruct
{
    [SerializeField]
    private Audios _audio;
    [SerializeField]
    private AudioType _audioType;
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private bool _oneShot;

    public Audios Audio { get => _audio; set => _audio = value; }
    public AudioType AudioType { get => _audioType; set => _audioType = value; }
    public AudioClip Clip { get => _clip; set => _clip = value; }
    public bool OneShot { get => _oneShot; set => _oneShot = value; }
}

[Serializable]
public struct AudioSourceStruct
{
    [SerializeField]
    private AudioSource _thisAudioSource;

    [SerializeField]
    private AudioType _audioType;

    public AudioType AudioType { get => _audioType; set => _audioType = value; }
    public AudioSource ThisAudioSource { get => _thisAudioSource; set => _thisAudioSource = value; }
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

    [SerializeField]
    private AudioStruct[] _audios;

    [SerializeField]
    private AudioSourceStruct[] _audioSources;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayAudio(Audios pAudio, float pVolume = 1f)
    {
        AudioStruct audio = _audios[0];

        for(int i = 0; i < _audios.Length; i ++)
        {
            if(_audios[i].Audio == pAudio)
            {
                audio = _audios[i];
                break;
            }
        }

        AudioSourceStruct audioSourceStruct = _audioSources[0];

        for (int i = 0; i < _audioSources.Length; i++)
        {
            if (_audioSources[i].AudioType == audio.AudioType)
            {
                audioSourceStruct = _audioSources[i];
                break;
            }
        }

        AudioSource audioSource = audioSourceStruct.ThisAudioSource;

        audioSource.volume = pVolume;

        if (audio.OneShot)
        {
            audioSource.PlayOneShot(audio.Clip);
        }
        else
        {
            audioSource.clip = audio.Clip;
            audioSource.Play();
        }
    }

    public void StopAudio(Audios pAudio)
    {
        AudioStruct audio = _audios[0];

        for (int i = 0; i < _audios.Length; i++)
        {
            if (_audios[i].Audio == pAudio)
            {
                audio = _audios[i];
                break;
            }
        }

        AudioSourceStruct audioSourceStruct = _audioSources[0];

        for (int i = 0; i < _audioSources.Length; i++)
        {
            if (_audioSources[i].AudioType == audio.AudioType)
            {
                audioSourceStruct = _audioSources[i];
                break;
            }
        }

        AudioSource audioSource = audioSourceStruct.ThisAudioSource;

        if (audioSource.clip == audio.Clip)
        {
            audioSource.Stop();
        }
    }
}
