using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public bool Playing { get => _playing; set => _playing = value; }

    private float _sanity = 1f;
    private float _tired = 1f;

    private float _sanityFactor = 0.002f;
    private float _tiredFactor = 0.005f;

    private bool _playing = true;

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
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _sanity -= _sanityFactor * Time.deltaTime;
        _tired -= _tiredFactor * Time.deltaTime;

        //Debug.Log("Sanity: "+_sanity);
        //Debug.Log("Tired: " + _tired);
    }

    public void IncreaseTired(float pIncrease)
    {
        _tired += pIncrease;
    }

    public void LoseSequence()
    {
        _playing = false;
    }
}
