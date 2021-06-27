using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private Car _car;


    [SerializeField]
    private float _speedH = 0.1f;
    [SerializeField]
    private float _speedV = 0.1f;
    [SerializeField]
    private LayerMask _interactableLayer;

    private InteractableObject _lookingInteraction = null;

    private Vector2 _lookDirection = Vector2.zero;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    bool _startedDriving = false;

    private void Start()
    {
        _car.OnStartedDriving.AddListener(OnStartedDriving);
    }

    public void OnStartedDriving()
    {
        _startedDriving = true;
        _car.OnStartedDriving.RemoveListener(OnStartedDriving);
    }

    private void Update()
    {
        if (!_startedDriving) return;

        if(_lookDirection != Vector2.zero)
        {
            pitch -= Time.deltaTime * _speedV * _lookDirection.y;
            yaw += Time.deltaTime * _speedH * _lookDirection.x;

            pitch = Mathf.Clamp(pitch, -10f, 60f);
            yaw = Mathf.Clamp(yaw, -60f, 40f);

            _camera.localEulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _interactableLayer))
        {

            InteractableObject interactable = hit.collider.gameObject.GetComponent<InteractableObject>();

            if(interactable != null)
            {
                _lookingInteraction = interactable;
                UIManager.Instance.ToggleInteractionMessage(true, interactable.InteractableName, interactable.CurrentState);
            }
        }
        else
        {
            UIManager.Instance.ToggleInteractionMessage(false);
        }
    }

    public void MoveInput(InputAction.CallbackContext pContext)
    {
        Vector2 direction = pContext.ReadValue<Vector2>();

        _lookDirection = direction;
    }

    public void InteractInput(InputAction.CallbackContext pContext)
    {
        if (!GameManager.Instance.Playing)
        {
            return;
        }
        if (pContext.started && _lookingInteraction != null)
        {
            _lookingInteraction.PerformInteraction();
        }
    }
}
