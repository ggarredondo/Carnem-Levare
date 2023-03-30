﻿using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumble : MonoBehaviour
{
    public static ControllerRumble Instance { get; private set; }
    private Gamepad gamepad;
    private bool isRumbling;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gamepad = Gamepad.current;
    }

    public void Rumble(float duration, float leftAmplitude, float rightAmplitude)
    {
        if (gamepad != null && SceneManagement.Instance.PlayerInput.currentControlScheme == "Gamepad" && !isRumbling)
        {
            gamepad.SetMotorSpeeds(leftAmplitude, rightAmplitude);
            isRumbling = true;
            StartCoroutine(StopRumble(duration));
        }
    }

    private IEnumerator StopRumble(float duration)
    {
        yield return new WaitForSeconds(duration);
        gamepad.SetMotorSpeeds(0f, 0f);
        isRumbling = false;
    }
}