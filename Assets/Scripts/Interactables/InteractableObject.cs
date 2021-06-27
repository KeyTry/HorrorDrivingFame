using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    protected string _interactableName;

    protected string _currentState;

    public string InteractableName { get => _interactableName; set => _interactableName = value; }
    public string CurrentState { get => _currentState; set => _currentState = value; }

    public virtual void PerformInteraction()
    {
        Debug.Log("Perform interaction!");
    }
}
