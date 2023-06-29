using UnityEngine;
using System.Collections;
using System;

public class HitStop
{
    private RNG shakeRNG;
    private Animator attackerAnimator, defenderAnimator;
    private Rigidbody defenderRB;
    private Collider defenderCol;

    public HitStop(in Animator attackerAnimator, in Animator defenderAnimator, in Rigidbody defenderRB, in Collider defenderCol)
    {
        this.attackerAnimator = attackerAnimator;
        this.defenderAnimator = defenderAnimator;
        this.defenderRB = defenderRB;
        this.defenderCol = defenderCol;
        shakeRNG = new RNG(GameManager.RANDOM_SEED);
    }

    // This should be done in FixedUpdate
    public IEnumerator Start(double ms, float intensity)
    {
        attackerAnimator.speed = 0;
        defenderAnimator.speed = 0;
        defenderRB.isKinematic = false;
        defenderCol.enabled = false;

        Vector3 originalDefenderPosition = defenderRB.position;
        float maxTime = (float)TimeSpan.FromMilliseconds(ms).TotalSeconds;
        float resolveRate = intensity / maxTime;

        for (float t = 0; t < maxTime; t += Time.deltaTime)
        {
            intensity -= resolveRate * t;
            defenderRB.position = originalDefenderPosition;
            defenderRB.position += new Vector3(shakeRNG.RangeFloat(-intensity, intensity), 
                0f, shakeRNG.RangeFloat(-intensity, intensity));
            yield return null;
        }

        attackerAnimator.speed = 1;
        defenderAnimator.speed = 1;
        defenderRB.isKinematic = true;
        defenderCol.enabled = true;
    }
}
