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



    private void Start()
    {

    }

    public override void PerformInteraction()
    {

            Debug.Log("Perform interaction!");
            GameManager.Instance.IncreaseTired(0.5f);
            GameManager.Instance.changeTiredFactor(0.003f);
            StartCoroutine(NormalizeTire());
    }

    IEnumerator NormalizeTire()
    {
        yield return new WaitForSeconds(10);
        GameManager.Instance.changeTiredFactor(0.005f);
    }
}
