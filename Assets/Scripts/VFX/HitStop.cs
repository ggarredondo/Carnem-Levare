using UnityEngine;
using System.Collections;
using System;

public class HitStop // Maybe MonoBehaviour with serialized variables would be better
{
    private RNG shakeRNG;
    private Animator attackerAnimator, defenderAnimator;
    private Vector3 defenderPosition;

    public HitStop(in Animator attackerAnimator, in Animator defenderAnimator, in Vector3 defenderPosition)
    {
        this.attackerAnimator = attackerAnimator;
        this.defenderAnimator = defenderAnimator;
        this.defenderPosition = defenderPosition;
        shakeRNG = new RNG(GameManager.RANDOM_SEED);
    }

    public IEnumerator Start(double ms, float intensity)
    {
        attackerAnimator.speed = 0;
        defenderAnimator.speed = 0;
        Vector3 originalDefenderPosition = defenderPosition;
        float maxTime = (float)TimeSpan.FromMilliseconds(ms).TotalSeconds;
        float resolveRate = intensity / maxTime;

        for (float t = 0; t < maxTime; t += Time.deltaTime)
        {
            intensity -= resolveRate * t;
            defenderPosition.x += shakeRNG.RangeFloat(-intensity, intensity);
            defenderPosition.z += shakeRNG.RangeFloat(-intensity, intensity);
            yield return null;
        }

        defenderPosition = originalDefenderPosition;
    }
}
