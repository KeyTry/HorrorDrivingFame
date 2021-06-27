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
}
