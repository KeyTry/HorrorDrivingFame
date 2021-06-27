using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public enum Directions
    {
        Forward,
        Left,
        Right
    }

    [SerializeField]
    private Directions _direction;

    [SerializeField]
    private Transform _end;

    [SerializeField]
    private bool _firstPiece = false;

    [SerializeField]
    private GameObject _nextPieceTrigger;

    [SerializeField]
    private GameObject[] _lights;

    private RoadGenerator _roadGenerator;

    public Directions Direction { get => _direction; set => _direction = value; }
    public Transform End { get => _end; set => _end = value; }
    public RoadGenerator RoadGenerator { get => _roadGenerator; set => _roadGenerator = value; }
    public GameObject NextPieceTrigger { get => _nextPieceTrigger; set => _nextPieceTrigger = value; }

    private void Start()
    {
        foreach (GameObject light in _lights)
        {
            light.SetActive(false);
        }

        int lightsDecision = Random.Range(0, 12);

        int lightsMaxAmount = 0;

        if(lightsDecision > 10)
        {
            lightsMaxAmount = _lights.Length;
        }
        else if (lightsDecision > 8)
        {
            lightsMaxAmount = _lights.Length / 2;
        }
        else if (lightsDecision > 6)
        {
            lightsMaxAmount = _lights.Length / 3;
        }
        else if(lightsDecision > 3)
        {
            lightsMaxAmount = 1;
        }

        int lightsAmount = Random.Range(0, lightsMaxAmount);

        Debug.Log("Lights amount on road: "+_direction+": "+lightsAmount );

        if(lightsAmount > 0)
        {
            Debug.Log("Activating lights");
            List<GameObject> activeLights = new List<GameObject>();

            for(int i = 0; i < lightsAmount; i++)
            {
                Debug.Log("Finding light: "+i);
                GameObject selectedLight = null;

                do
                {
                    selectedLight = _lights[Random.Range(0, _lights.Length)];
                    Debug.Log("Trying light: " + selectedLight, selectedLight);
                } while (activeLights.Contains(selectedLight));

                Debug.Log("Did light: " + selectedLight, selectedLight);

                if (selectedLight != null)
                {
                    activeLights.Add(selectedLight);
                    Debug.Log("Activating light: "+selectedLight,selectedLight);
                    selectedLight.SetActive(true);
                }
            }

        }

        _roadGenerator = FindObjectOfType<RoadGenerator>();

        if(_firstPiece)
        {
            _nextPieceTrigger.SetActive(false);
            _roadGenerator.GenerateNextPieces(this);
        }
    }

    public void OnTriggerEnterEvent(Collider other)
    {
        if(other.CompareTag(Constants.CAR_TAG))
        {
            _roadGenerator.GenerateNextPieces(this);
        }
    }
}
