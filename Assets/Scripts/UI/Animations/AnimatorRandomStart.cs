using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTaskLayout.Presentation.Animations
{
    [DisallowMultipleComponent]
    public sealed class AnimatorRandomStart : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required]
        private Animator animator;

        [Title("Delay")]
        [SerializeField, ToggleLeft]
        private bool useRandomDelay = true;

        [SerializeField, MinValue(0f), ShowIf(nameof(IsFixedDelay))]
        private float delay = 0.25f;

        [SerializeField, MinValue(0f), ShowIf(nameof(useRandomDelay))]
        private float minDelay = 0f;

        [SerializeField, MinValue(0f), ShowIf(nameof(useRandomDelay))]
        private float maxDelay = 0.6f;

        [Title("Desync")]
        [SerializeField]
        [Tooltip("Сдвиг фазы анимации (normalizedTime 0..1).")]
        private bool randomizeNormalizedTime = true;

        [Title("Speed")]
        [SerializeField, ToggleLeft]
        private bool randomizeSpeed = false;

        [SerializeField, MinValue(0f), ShowIf(nameof(randomizeSpeed))]
        private float minSpeed = 0.9f;

        [SerializeField, MinValue(0f), ShowIf(nameof(randomizeSpeed))]
        private float maxSpeed = 1.15f;

        [Title("Seed")]
        [SerializeField]
        [Tooltip("Если true — стабильный seed по InstanceID (одинаково между запусками).")]
        private bool deterministic = false;

        private bool IsFixedDelay => !useRandomDelay;
        private Coroutine _routine;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (animator == null) return;
            _routine = StartCoroutine(ApplyRoutine());
        }

        private void OnDisable()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            if (animator != null)
                animator.speed = 1f;
        }

        private IEnumerator ApplyRoutine()
        {
            var rng = deterministic
                ? new System.Random(gameObject.GetInstanceID())
                : new System.Random(Environment.TickCount ^ gameObject.GetInstanceID());

            yield return null;

            float d = GetDelay(rng);
            if (d > 0f)
                yield return new WaitForSeconds(d);

            if (randomizeSpeed)
            {
                float min = Mathf.Max(0f, minSpeed);
                float max = Mathf.Max(min, maxSpeed);
                animator.speed = Lerp(min, max, (float)rng.NextDouble());
            }

            if (randomizeNormalizedTime)
            {
                var state = animator.GetCurrentAnimatorStateInfo(0);
                float t = (float)rng.NextDouble();
                animator.Play(state.fullPathHash, 0, t);
                animator.Update(0f);
            }
        }

        private float GetDelay(System.Random rng)
        {
            if (!useRandomDelay)
                return Mathf.Max(0f, delay);

            float min = Mathf.Max(0f, minDelay);
            float max = Mathf.Max(min, maxDelay);

            if (Mathf.Approximately(min, max))
                return min;

            return Lerp(min, max, (float)rng.NextDouble());
        }

        private float Lerp(float a, float b, float t) => a + (b - a) * Mathf.Clamp01(t);
    }
}