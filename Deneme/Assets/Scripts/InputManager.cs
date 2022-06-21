using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Persistent<InputManager>
{
    #region Events
        public delegate void StartTouch(Vector2 position);
        public event StartTouch OnStartTouch;
        public delegate void EndTouch(Vector2 position);
        public event EndTouch OnEndTouch;
    #endregion

    private PlayerInputAction playerInputAction;
    private Camera mainCamera;

    public override void Awake()
    {
        playerInputAction = new PlayerInputAction();
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        playerInputAction.Enable();
    }
    void OnDisable()
    {
        playerInputAction.Disable();
    }
    void Start()
    {
        playerInputAction.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerInputAction.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
        
    }

    void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if(OnStartTouch != null)
        {
            OnStartTouch(Utils.ScreenToWorld(mainCamera, playerInputAction.Touch.PrimaryPosition.ReadValue<Vector2>()));
        } 
    }

    void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if(OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, playerInputAction.Touch.PrimaryPosition.ReadValue<Vector2>()));
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, playerInputAction.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}
