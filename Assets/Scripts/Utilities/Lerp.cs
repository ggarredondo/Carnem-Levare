using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;
using System.Collections;

namespace LerpUtilities
{
    public class Lerp
    {
        public static async Task Value_Math<T>(T startValue, T targetValue, Action<T> setValue, float speed, Func<float, float> function)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1)
            {
                elapsedTime += speed * Time.deltaTime;
                float value = function(Mathf.Clamp01(elapsedTime));

                T interpolatedValue = LocalLerp(startValue, targetValue, value);
                setValue(interpolatedValue);

                await Task.Yield();
            }

            setValue(targetValue);
        }

        public static IEnumerator Value_Math_Coroutine<T>(T startValue, T targetValue, Action<T> setValue, float speed, Func<float, float> function)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1)
            {
                elapsedTime += speed * Time.deltaTime;
                float value = function(Mathf.Clamp01(elapsedTime));

                T interpolatedValue = LocalLerp(startValue, targetValue, value);
                setValue(interpolatedValue);

                yield return null;
            }

            setValue(targetValue);
        }

        public static async Task Value_Bezier(Vector3[] values, Action<Vector3> setValue, float speed, Func<float, Vector3[], Vector3> BezierCurve)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1)
            {
                elapsedTime += speed * Time.deltaTime;
                Vector3 interpolatedValue = BezierCurve(Mathf.Clamp01(elapsedTime), values);

                setValue(interpolatedValue);

                await Task.Yield();
            }

            setValue(values[1]);
        }

        public static async Task Value<T>(T startValue, T targetValue, Action<T> setValue, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                T interpolatedValue = LocalLerp(startValue, targetValue, t);
                setValue(interpolatedValue);

                await Task.Yield();
            }

            setValue(targetValue);
        }

        public static IEnumerator Value_Coroutine<T>(T startValue, T targetValue, Action<T> setValue, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                T interpolatedValue = LocalLerp(startValue, targetValue, t);
                setValue(interpolatedValue);

                yield return null;
            }

            setValue(targetValue);
        }

        public static async Task Value_Unscaled<T>(T startValue, T targetValue, Action<T> setValue, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                T interpolatedValue = LocalLerp(startValue, targetValue, t);
                setValue(interpolatedValue);

                await Task.Yield();
            }

            setValue(targetValue);
        }

        public static async Task Value_Cancel<T>(T startValue, T targetValue, Action<T> setValue, float duration, CancellationToken cancel)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {

                if (cancel.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                T interpolatedValue = LocalLerp(startValue, targetValue, t);
                setValue(interpolatedValue);

                await Task.Yield();
            }

            setValue(targetValue);
        }

        private static T LocalLerp<T>(T startValue, T targetValue, float t)
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)Mathf.Lerp(Convert.ToSingle(startValue), Convert.ToSingle(targetValue), t);
            }
            else if (typeof(T) == typeof(Vector2))
            {
                return (T)(object)Vector2.Lerp((Vector2)(object)startValue, (Vector2)(object)targetValue, t);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)Vector3.Lerp((Vector3)(object)startValue, (Vector3)(object)targetValue, t);
            }
            else if (typeof(T) == typeof(Color))
            {
                return (T)(object)Color.Lerp((Color)(object)startValue, (Color)(object)targetValue, t);
            }

            throw new ArgumentException("Unsupported type for lerping: " + typeof(T).Name);
        }
    }
}
