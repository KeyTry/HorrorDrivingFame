using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDelegate : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<Collider> _onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        _onTriggerEnter.Invoke(other);
    }
}
