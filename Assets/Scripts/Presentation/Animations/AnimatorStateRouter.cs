using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTaskLayout.Presentation.Animations
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UI/Animations/Animator State Router")]
    public sealed class AnimatorStateRouter : MonoBehaviour
    {
        [SerializeField, Required] private Animator animator;

        [SerializeField, Required]
        [Tooltip("Имя state, в который нужно перейти.")]
        private string targetState;

        [SerializeField] private bool playOnEnable = true;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (playOnEnable)
                PlayTarget();
        }

        public void PlayTarget()
        {
            if (animator == null || string.IsNullOrWhiteSpace(targetState))
                return;

            animator.Play(targetState, 0, 0f);
        }

        public void SetTargetState(string stateName)
        {
            targetState = stateName;
        }
    }
}