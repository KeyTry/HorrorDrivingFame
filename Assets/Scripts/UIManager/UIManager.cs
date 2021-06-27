using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    [SerializeField]
    private TextMeshProUGUI _interactionMessage;

    [SerializeField]
    private GameObject _winContainer;
    [SerializeField]
    private GameObject _gameOverContainer;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _winText;

    private void Start()
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

    public void ToggleInteractionMessage(bool pToggle, string pObjectName = "", string pObjectState = "")
    {
        _interactionMessage.text = "[E] INTERACT -"+pObjectName.ToUpper() + " "+pObjectState;

        _interactionMessage.gameObject.SetActive(pToggle);
    }
    public void DoWin()
    {
        GameManager.Instance.Playing = false;
        StartCoroutine(DoGameOverCoroutine());
    }
    public IEnumerator DoWinCoroutine()
    {

        _winContainer.SetActive(true);

        yield return new WaitForSeconds(2f);

        StartCoroutine(ToggleText(_winText, true));
    }


    public void DoGameOver()
    {
        StartCoroutine(DoGameOverCoroutine());
    }

    public IEnumerator DoGameOverCoroutine()
    {

        AudioManager.Instance.StopAudio(Audios.CarEngine);
        AudioManager.Instance.PlayAudio(Audios.CrashSound);
        _gameOverContainer.SetActive(true);

        yield return new WaitForSeconds(2f);

        StartCoroutine(ToggleText(_gameOverText,true));
    }


    private IEnumerator ToggleText(TextMeshProUGUI pText, bool pToggle, float pTime = 1f)
    {
        Color targetColor = new Color(pText.color.r, pText.color.g, pText.color.b, 0f);

        if (pToggle)
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
