using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace TestTaskLayout.Presentation.Animations
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UI/Animations/Animator Auto Switch On Complete")]
    public sealed class AnimatorAutoSwitchOnComplete : MonoBehaviour
    {
        [SerializeField, Required] private Animator animator;
        [SerializeField, Required] private RuntimeAnimatorController nextController;

        [SerializeField]
        [Tooltip("Имя state в текущем контроллере, который должен завершиться.")]
        private string waitStateName;

        [SerializeField]
        [Tooltip("State, который нужно запустить сразу после смены контроллера. Можно оставить пустым, если старт делается через событие.")]
        private string playStateAfterSwitch;

        [Title("Events")]
        [SerializeField]
        [Tooltip("Вызывается после смены контроллера (и после playStateAfterSwitch, если он задан).")]
        private UnityEvent onSwitched;

        private bool _switched;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _switched = false;
        }

        private void Update()
        {
            if (_switched || animator == null || nextController == null)
                return;

            var st = animator.GetCurrentAnimatorStateInfo(0);

            if (!string.IsNullOrEmpty(waitStateName) && !st.IsName(waitStateName))
                return;

            if (st.loop)
                return;

            if (st.normalizedTime < 1f)
                return;

            _switched = true;
            animator.runtimeAnimatorController = nextController;

            if (!string.IsNullOrEmpty(playStateAfterSwitch))
                animator.Play(playStateAfterSwitch, 0, 0f);

            onSwitched?.Invoke();
        }
    }
}