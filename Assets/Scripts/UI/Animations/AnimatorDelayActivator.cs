using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTaskLayout.Presentation.Animations
{
    [DisallowMultipleComponent]
    public sealed class AnimatorDelayActivator : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required] private Animator animator;

        [Title("Delay")]
        [SerializeField, ToggleLeft]
        private bool useRandomDelay = true;

        [SerializeField, MinValue(0f), ShowIf(nameof(IsFixedDelay))]
        private float delay = 0.25f;

        [SerializeField, MinValue(0f), ShowIf(nameof(useRandomDelay))]
        private float minDelay = 0f;

        [SerializeField, MinValue(0f), ShowIf(nameof(useRandomDelay))]
        private float maxDelay = 0.5f;

        [Title("Startup")]
        [SerializeField]
        [Tooltip("Если true — применяем стартовое состояние Animator (без проигрыша) на следующем кадре, затем снова стопаем.")]
        private bool applyInitialStateOnce = false;

        [Title("Options")]
        [SerializeField, ShowIf(nameof(useRandomDelay))]
        [Tooltip("Если true — задержка будет стабильной для объекта.")]
        private bool deterministic = false;

        private Coroutine _routine;
        private bool _initialized;

        private bool IsFixedDelay => !useRandomDelay;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            if (animator == null)
                return;

            animator.enabled = false;
            _initialized = true;
        }

        private void OnEnable()
        {
            if (!_initialized || animator == null)
                return;

            _routine = StartCoroutine(RunRoutine());
        }

        private void OnDisable()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            if (animator != null)
                animator.enabled = false;
        }

        private IEnumerator RunRoutine()
        {
            yield return null;

            if (applyInitialStateOnce)
            {
                animator.enabled = true;

                animator.Rebind();
                animator.Update(0f);

                animator.enabled = false;
            }

            float d = GetDelay();
            if (d > 0f)
                yield return new WaitForSeconds(d);

            if (animator != null)
                animator.enabled = true;
        }

        private float GetDelay()
        {
            if (!useRandomDelay)
                return Mathf.Max(0f, delay);

            float min = Mathf.Max(0f, minDelay);
            float max = Mathf.Max(min, maxDelay);

            if (Mathf.Approximately(min, max))
                return min;

            if (deterministic)
            {
                var rng = new System.Random(gameObject.GetInstanceID());
                return Mathf.Lerp(min, max, (float)rng.NextDouble());
            }

            return Random.Range(min, max);
        }
    }
}