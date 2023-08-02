using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameControls _gameControls;

    void Awake()
    {
        _gameControls = new GameControls();
        _gameControls.Enable();
    }

    public Vector2 GetPointerPos()
    {
        return _gameControls.FpsInput.PointerPosition.ReadValue<Vector2>();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return _gameControls.FpsInput.Movement.ReadValue<Vector2>();
    }

    public bool GetShootingInput()
    {
        return _gameControls.FpsInput.Shoot.WasPerformedThisFrame();
    }
}
