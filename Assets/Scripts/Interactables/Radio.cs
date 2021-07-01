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
    private AudioSource RadioObj;
    [SerializeField]
    private GameObject monster1;
    [SerializeField]
    private GameObject monster2;
    [SerializeField]
    private AudioSource monsterSource1;
    [SerializeField]
    private AudioSource monsterSource2;


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
                GameManager.Instance.changeTiredFactor(0.012f);
                StartCoroutine(NormalizeTire());
                if (GameManager.Instance.Sanity > 0.5f)
                {
                    monster1.SetActive(true);
                    monsterSource1.Play();
                }
                else {
                    monster2.SetActive(true);
                    monsterSource2.Play();
                }
            }
            else
            {
                RadioObj.clip = audioMusic[Random.Range(0, audioMusic.Length)];
                RadioObj.loop = false;
                GameManager.Instance.IncreaseTired(0.05f);
                GameManager.Instance.changeTiredFactor(0.003f);
                StartCoroutine(NormalizeTire());
                StartCoroutine(ApagarRadio());
            }
            RadioObj.Play();
        }
        else {
            AudioManager.Instance.PlayAudio(Audios.RadioButton);
            AudioManager.Instance.PlayAudio(Audios.StaticChangeChannel);
            RadioObj.Stop();
            GameManager.Instance.changeTiredFactor(0.006f);
            isOn = false;
            monster1.SetActive(false);
            monsterSource1.Stop();
            monster2.SetActive(false);
            monsterSource2.Stop();
        }

            
            Debug.Log("Perform interaction!");
    }

    IEnumerator NormalizeTire()
    {
        yield return new WaitForSeconds(15);
        GameManager.Instance.changeTiredFactor(0.007f);
    }

    IEnumerator ApagarRadio() {
        yield return new WaitForSeconds(20);
        if (isOn) {
            AudioManager.Instance.PlayAudio(Audios.StaticChangeChannel);
            RadioObj.Stop();
            GameManager.Instance.changeTiredFactor(0.012f);
            isOn = false;
            monster1.SetActive(false);
            monsterSource1.Stop();
            monster2.SetActive(false);
            monsterSource2.Stop();
        }
    }
}
