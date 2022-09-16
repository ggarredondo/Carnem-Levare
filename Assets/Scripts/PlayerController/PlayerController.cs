using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    private Vector2 left_stick;

    [Header("Animation Parameters")]
    [Range(-1f, 1f)] public float load = 0f;
    [Range(-5f, 5f)] public float speed = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Animation();
    }

    /// <summary>
    /// Set animation parameters for the animator
    /// </summary>
    private void Animation()
    {
        anim.SetFloat("load", load);
        anim.SetFloat("speed", speed);
    }

    //***CONTROLS***

    /// <summary>
    /// Obtain the direction movement from the left stick
    /// </summary>
    public void MoveControl(InputAction.CallbackContext context)
    {
        //Obtain de direction from the left stick
        left_stick = context.ReadValue<Vector2>();
    }
}
