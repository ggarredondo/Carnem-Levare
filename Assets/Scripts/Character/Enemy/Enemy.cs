using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : Character
{
    [Header("Movement Parameters")]
    [SerializeField] private bool debugInput;
    [SerializeField] private float enemyDirectionSpeed = 1f;

    public void Movement(InputAction.CallbackContext context) { if (debugInput) directionTarget = -context.ReadValue<Vector2>().normalized; }
    public void Block(InputAction.CallbackContext context) { if (debugInput) isBlocking = !isBlocking; }

    protected override void Start()
    {
        isBlocking = true;
        base.Start();
    }

    protected override void Update()
    {
        directionSpeed = enemyDirectionSpeed;
        base.Update();
    }
}
