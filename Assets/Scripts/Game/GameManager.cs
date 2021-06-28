using PSX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public bool Playing { get => _playing; set => _playing = value; }
    public float Sanity { get => _sanity; set => _sanity = value; }
    public float Tired { get => _tired; set => _tired = value; }
    public bool GameStarted { get => _gameStarted; set => _gameStarted = value; }
    public bool Lost { get => _lost; set => _lost = value; }

    private float _sanity = 1f;
    private float _tired = 1f;

    private float _sanityFactor = 0.006f;
    private float _tiredFactor = 0.01f;

    private FogController _fogController;

    private bool _gameStarted = false;
    private bool _playing = true;

    private void Start()
    {
        _fogController = FindObjectOfType<FogController>();
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

    bool _lost = false;

    private void Update()
    {
        _sanity -= _sanityFactor * Time.deltaTime;
        _tired -= _tiredFactor * Time.deltaTime;

        Debug.Log("Sanity: " + _sanity);
        Debug.Log("Tired: " + _tired);

        if(_gameStarted)
        {
            _fogController.FogDistance = Mathf.Lerp(12f, 0.6f, _tired);
        }

        if(_tired <= 0f && !_lost)
        {
            _lost = true;
            UIManager.Instance.DoGameOver();
        }
    }

    public void IncreaseTired(float pIncrease)
    {
        _tired += pIncrease;

        if(_tired > 1f)
        {
            _tired = 1f;
        }
    }

    public void LoseSequence()
    {
        _playing = false;
    }

    public void changeSanityFactor(float reference)
    {
        _sanityFactor = reference;
    }

    public void changeTiredFactor(float reference)
    {
        _tiredFactor = reference;
    }

    public void RestartInput(InputAction.CallbackContext pContext)
    {
        if(_lost)
        {
            if(pContext.started)
            {
                Debug.Log("Restarting");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
