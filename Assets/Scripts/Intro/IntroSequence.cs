using PSX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class IntroSequence : MonoBehaviour
{
    [SerializeField]
    private FogController _fogController;

    [SerializeField]
    private TextMeshProUGUI _introText;

    [SerializeField]
    private TextMeshProUGUI[] _instructionsText;

    [SerializeField]
    private Car _car;


    private float _fogDensityMax = 20f;
    private float _fogDensityMin = 3f;
    private float _fogDistanceMax = 20f;

    private float _fogDistanceMin = 1f;

    private bool _started = false;

    // Start is called before the first frame update
    void Start()
    {
        _fogController.FogDensity = _fogDensityMax;
        _fogController.FogDistance = _fogDistanceMax;

        StartCoroutine(Sequence());
        _fogDensityMin = GameManager.Instance.GetCurrentFogDensity();
        _fogDistanceMin = GameManager.Instance.GetCurrentFogDistance();
    }

    public void OnStarted()
    {
        _car.OnStartedDriving.RemoveListener(OnStarted);
        _started = true;
    }

    private IEnumerator Sequence()
    {
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ToggleText(_introText, true, 3f));
        _car.OnStartedDriving.AddListener(OnStarted);

        while(!_started)
        {
            yield return null;
        }


        yield return new WaitForSeconds(1f);

        StartCoroutine(ClearFogCoroutine());
        StartCoroutine(ToggleText(_introText, false, 3f));

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < _instructionsText.Length; i++)
        {

            StartCoroutine(ToggleText(_instructionsText[i], true, 3f)); 

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < _instructionsText.Length; i++)
        {

            StartCoroutine(ToggleText(_instructionsText[i], false, 3f));

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ClearFogCoroutine()
    {
        float normal = 0f;

        while(normal < 1f)
        {
            _fogController.FogDistance = Mathf.Lerp(_fogDistanceMax, _fogDistanceMin, normal);
            _fogController.FogDensity = Mathf.Lerp(_fogDensityMax, _fogDensityMin, normal);
            normal += Time.deltaTime / 4f;

            yield return null;
        }
        GameManager.Instance.GameStarted = true;

        yield return null;
    }

    private IEnumerator ToggleText(TextMeshProUGUI pText, bool pToggle, float pTime = 1f)
    {
        Color targetColor = new Color(pText.color.r, pText.color.g, pText.color.b, 0f);

        if(pToggle)
        {
            pText.gameObject.SetActive(true);
            targetColor = new Color(pText.color.r, pText.color.g, pText.color.b, 1f);
        }

        Color originalColor = pText.color;

        float normal = 0f;

        while (normal < 1f)
        {
            pText.color = Color.Lerp(originalColor, targetColor, normal);

            normal += Time.deltaTime / pTime;

            yield return null;
        }


        if (!pToggle)
        {
            pText.gameObject.SetActive(false);
        }

        yield return null;
    }
}
