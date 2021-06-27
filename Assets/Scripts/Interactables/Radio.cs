using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : InteractableObject
{
    [SerializeField]
    private AudioClip[] audioMusic;
    [SerializeField]
    private AudioClip[] staticAudio;
    [SerializeField]
    private AudioClip RadioButton;
    [SerializeField]
    private AudioClip RadioChangeChanel;
    [SerializeField]
    private AudioSource RadioObj;

    private bool isOn;

    private int staticdecision;


    private void Start()
    {

    }

    public override void PerformInteraction()
    {
        if (!isOn)
        {
            isOn = true;
            AudioManager.Instance.PlayAudio(Audios.RadioButton);
            AudioManager.Instance.PlayAudio(Audios.StaticChangeChannel);
            staticdecision = Random.Range(0, 12);
            bool useStatic = false;
            int threshold = 0;
            if (GameManager.Instance.Sanity > 0.5f)
            {
                threshold = 8;
            }
            else if (GameManager.Instance.Sanity > 0.3f)
            {
                threshold = 6;
            }
            else
            {
                threshold = 3;
            }
            if (staticdecision > threshold)
            {
                useStatic = true;
            }
            if (useStatic)
            {
                RadioObj.clip = staticAudio[Random.Range(0, staticAudio.Length)];
                RadioObj.loop = true;
                GameManager.Instance.changeTiredFactor(0.007f);
                StartCoroutine(NormalizeTire());
            }
            else
            {
                RadioObj.clip = audioMusic[Random.Range(0, audioMusic.Length)];
                RadioObj.loop = false;
                GameManager.Instance.IncreaseTired(0.5f);
                GameManager.Instance.changeTiredFactor(0.003f);
                StartCoroutine(NormalizeTire());
            }
            RadioObj.Play();
        }
        else {
            AudioManager.Instance.PlayAudio(Audios.RadioButton);
            AudioManager.Instance.PlayAudio(Audios.StaticChangeChannel);
            RadioObj.Stop();
            GameManager.Instance.changeTiredFactor(0.005f);
            isOn = false;
        }

            
            Debug.Log("Perform interaction!");
    }

    IEnumerator NormalizeTire()
    {
        yield return new WaitForSeconds(15);
        GameManager.Instance.changeTiredFactor(0.005f);
    }
}
