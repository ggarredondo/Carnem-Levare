using UnityEngine;
using System;
using System.Threading.Tasks;

public class HitStop : MonoBehaviour
{
    private Animator attackerAnimator, defenderAnimator;
    private Transform attackerTransform;
    private Rigidbody defenderRb;
    private Collider defenderCol;

    private float amplitude, frequency;
    private Vector3 originalPosition;
    private float timer, maxTime;

    private void Awake() => enabled = false;

    public event Action OnTriggered, OnFinish;

    public void Reference(in Animator attackerAnimator, in Transform attackerTransform,
        in Animator defenderAnimator, in Rigidbody defenderRb, 
        in Collider defenderCol)
    {
        this.attackerAnimator = attackerAnimator;
        this.attackerTransform = attackerTransform;
        this.defenderAnimator = defenderAnimator;
        this.defenderRb = defenderRb;
        this.defenderCol = defenderCol;
    }

    public async void Trigger(HitStopData hitStopData)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(hitStopData.StartTimeMS));

        OnTriggered?.Invoke();

        attackerAnimator.speed = hitStopData.AttackerAnimSpeed;
        defenderAnimator.speed = hitStopData.DefenderAnimSpeed;
        defenderRb.isKinematic = true;
        defenderCol.enabled = false;
        originalPosition = defenderRb.position;

        timer = 0;
        maxTime = (float)TimeSpan.FromMilliseconds(hitStopData.LengthMS).TotalSeconds;
        amplitude = hitStopData.Amplitude;
        frequency = hitStopData.Frequency;

        enabled = true;
    }

    public void FixedUpdate()
    {
        if (timer < maxTime)
        {
            defenderRb.position = originalPosition + attackerTransform.right * (amplitude * Mathf.Sin(frequency * timer));
            amplitude = Mathf.Lerp(amplitude, 0f, timer / maxTime);
            timer += Time.deltaTime;
        }
        else
            Finish();
    }

    public void Finish()
    {
        attackerAnimator.speed = 1;
        defenderAnimator.speed = 1;
        defenderRb.position = originalPosition;
        defenderRb.isKinematic = false;
        defenderCol.enabled = true;
        this.enabled = false;

        OnFinish?.Invoke();
    }
}
