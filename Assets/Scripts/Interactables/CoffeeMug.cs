using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMug : InteractableObject
{
    [SerializeField]
    private Transform _coffeeVisual;

    private float _coffeeVisualOriginalScale;

    private int _timesDrunk = 0;



    private void Start()
    {
        
    }

    public override void PerformInteraction()
    {
        if(_timesDrunk < 5)
        {
            Debug.Log("Perform interaction!");
            _timesDrunk++;
            AudioManager.Instance.PlayAudio(Audios.CoffeeSip);
            GameManager.Instance.IncreaseTired(0.3f);
            GameManager.Instance.changeTiredFactor(0.008f);
            StartCoroutine(NormalizeTire());
            _coffeeVisual.position = new Vector3(_coffeeVisual.position.x, _coffeeVisual.position.y - 0.02f, _coffeeVisual.position.z);
            if(_timesDrunk >= 5)
            {
                _currentState = "(EMPTY)";
            }
        }
    }

    IEnumerator NormalizeTire() {
        yield return new WaitForSeconds(10);
        GameManager.Instance.changeTiredFactor(0.01f);
    }
}
