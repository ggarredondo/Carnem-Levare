using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;

namespace LerpUtilities
{
    public class Lerp
    {
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
