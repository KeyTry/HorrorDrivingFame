using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private float _maxSpeed = 10f;

    private float _shakeTimer = 0f;
    private float _shakeLimit = 0f;
    private bool _shake = true;

    private bool _startedMoving = false;

    private Vector3 _rotateDirection = Vector3.zero;

    private UnityEvent _onStartedDriving;

    public UnityEvent OnStartedDriving { get => _onStartedDriving; set => _onStartedDriving = value; }

    private float _currentSpeed = 0f;

    private bool _stopped = true;

    private float _distance = 0f;

    private Vector3 _oldPos;

    private float _timeStopped = 0f;

    //private bool _drivingBackwards = false;
    //private float _drivingBackwardsTime = 0f;

    //private bool _drivePaused = false;

    private void Awake()
    {
        _onStartedDriving = new UnityEvent();
    }

    private void Start()
    {
        _oldPos = _rigidbody.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.TREES_TAG))
        {
            GameManager.Instance.Playing = false;
            UIManager.Instance.DoGameOver();
        }
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.Playing)
        {
            return;
        }

        if (!_stopped /*&& !_drivePaused*/)
        {
            _timeStopped = 0f;

            float speed = _currentSpeed;

            //if(_drivingBackwards)
            //{
            //    speed = -_currentSpeed;
            //    _drivingBackwardsTime += Time.deltaTime;

            //    if(_drivingBackwardsTime > 1f)
            //    {
            //        _drivePaused = true;
            //    }
            //}

            _rigidbody.MovePosition(_rigidbody.position + transform.forward * speed * Time.fixedDeltaTime);

            if (_shake)
            {
                _rigidbody.AddForce(new Vector3(Random.Range(-2f, 2f), Random.Range(-6f, 6f), Random.Range(-2f, 2f)), ForceMode.Impulse);
                _shakeLimit = Random.Range(0.001f, 0.01f);
                _shake = false;
            }
            else
            {
                _shakeTimer += Time.fixedDeltaTime;

                if (_shakeTimer > _shakeLimit)
                {
                    _shakeTimer = 0f;
                    _shake = true;
                }
            }

            if (_rotateDirection != Vector3.zero)
            {
                _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_rotateDirection * Time.fixedDeltaTime));
            }

            _distance += Vector3.Distance(_rigidbody.position, _oldPos);

            _oldPos = _rigidbody.position;
            Debug.Log("Distance: " + _distance);

            if (_distance > 2000)
            {
                UIManager.Instance.DoWin();
            }
        }
        else
        {
            //_timeStopped += Time.deltaTime;

            //if(_timeStopped > 3f)
            //{
            //    GameManager.Instance.LoseSequence();
            //}
        }
    }

    public void MoveInput(InputAction.CallbackContext pContext)
    {
        Vector2 direction = pContext.ReadValue<Vector2>();

        _rotateDirection = new Vector3(0f, direction.x, 0f) * 25f;
    }

    public void DriveInput(InputAction.CallbackContext pContext)
    {
        if (pContext.performed)
        {
            Debug.Log("Pressed drive forward");
            if (!_startedMoving)
            {
                GameManager.Instance.changeTiredFactor(0.01f);
                Debug.Log("Start moving");
                //_drivePaused = false;
                //_drivingBackwardsTime = 0f;
                _startedMoving = true;
                //_drivingBackwards = false;
                _onStartedDriving.Invoke();

                if(_stopped)
                {
                    StartCar();
                }
                else
                {
                    ToggleSpeed(true);
                }
            }
        }
        else if (pContext.canceled)
        {
            if (_startedMoving)
            {
                _startedMoving = false;
                ToggleSpeed(false);
                GameManager.Instance.changeTiredFactor(0.07f);
            }
        }
    }
    public void DriveBackwardsInput(InputAction.CallbackContext pContext)
    {
        //if (pContext.performed)
        //{
        //    Debug.Log("Pressed drive backwards");
        //    if (!_startedMoving)
        //    {
        //        _startedMoving = true;
        //        _drivingBackwards = true;
        //        _onStartedDriving.Invoke();

        //        if (_stopped)
        //        {
        //            StartCar();
        //        }
        //        else
        //        {
        //            ToggleSpeed(true);
        //        }
        //    }
        //    else if (pContext.canceled)
        //    {
        //        if (_startedMoving)
        //        {
        //            _startedMoving = false;
        //            ToggleSpeed(false);
        //        }
        //    }
        //}
    }

    private void StartCar()
    {
        if (_startDelayCoroutine != null)
        {
            StopCoroutine(_startDelayCoroutine);
            _startDelayCoroutine = null;
        }

        _startDelayCoroutine = StartCoroutine(StartDelayCoroutine());
    }

    private Coroutine _startDelayCoroutine;

    private IEnumerator StartDelayCoroutine()
    {
        AudioManager.Instance.PlayAudio(Audios.CarStart);
        yield return new WaitForSeconds(1f);

        ToggleSpeed(true);
    }


    private void ToggleSpeed(bool pToggle)
    {
        if (_startDelayCoroutine != null)
        {
            StopCoroutine(_startDelayCoroutine);
            _startDelayCoroutine = null;
        }

        if (_toggleSpeedCoroutine != null)
        {
            StopCoroutine(_toggleSpeedCoroutine);
            _toggleSpeedCoroutine = null;
        }

        _toggleSpeedCoroutine = StartCoroutine(ToggleSpeedCoroutine(pToggle));
    }

    private Coroutine _toggleSpeedCoroutine;

    private IEnumerator ToggleSpeedCoroutine(bool pToggle)
    {
        float normal = 0f;

        float target = 0f;

        float time = 4f;
        if(pToggle)
        {
            AudioManager.Instance.PlayAudio(Audios.CarEngine, 0.3f);
            _stopped = false;
            target = _maxSpeed;
            time = 20f;
        }

        float originalSpeed = _currentSpeed;

        while(normal <= 1f)
        {
            _currentSpeed = Mathf.Lerp(originalSpeed, target, normal);

            normal += Time.deltaTime / time;
            yield return null;
        }

        if(!pToggle)
        {
            _stopped = true;
        }

        yield return null;
    }
}
