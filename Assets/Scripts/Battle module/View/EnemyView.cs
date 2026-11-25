using Battle.Presentation;
using UnityEngine;
using UnityEngine.Events;

namespace Battle.View
{
    public class EnemyView : EntityView
    {
        [Header("Animation Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private Animation legacyAnimation;

        [Header("Animator Parameters")]
        [SerializeField] private string isMovingBool = "IsMoving";
        [SerializeField] private string beforeMoveTrigger = "BeforeMove";
        [SerializeField] private string afterMoveTrigger = "AfterMove";
        [SerializeField] private string damageTrigger = "Damage";

        [Header("Legacy Animation Clips (Optional)")]
        [SerializeField] private string beforeMoveClipName;
        [SerializeField] private string afterMoveClipName;
        [SerializeField] private string damageClipName;

        [Header("Movement Detection")]
        [SerializeField] private float arrivalThreshold = 0.01f;

        [Header("Events")]
        [SerializeField] private UnityEvent onBeforeMove;
        [SerializeField] private UnityEvent onAfterMove;
        [SerializeField] private UnityEvent<int> onDamaged;

        private bool isMoving;

        private void Awake()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            if (legacyAnimation == null)
                legacyAnimation = GetComponentInChildren<Animation>();
        }

        protected override void Update()
        {
            base.Update();

            if (!isMoving)
                return;

            float sqrDistance = (this.transform.position - targetPosition).sqrMagnitude;
            if (sqrDistance <= arrivalThreshold * arrivalThreshold)
            {
                this.transform.position = targetPosition;
                isMoving = false;
                GetPresenter().NotifyAfterMove();
            }
        }

        public override void UpdatePosition(PresenterPosition newPosition)
        {
            bool wasInitialized = positionInitialized;
            base.UpdatePosition(newPosition);

            if (!wasInitialized)
                return;

            float sqrDistance = (this.transform.position - targetPosition).sqrMagnitude;
            isMoving = sqrDistance > arrivalThreshold * arrivalThreshold;
        }

        public EnemyPresenter GetPresenter()
        {
            return (EnemyPresenter)presenter;
        }

        public void PlayBeforeMoveAnimation()
        {
            SetAnimatorBool(isMovingBool, true);
            SetAnimatorTrigger(beforeMoveTrigger);
            PlayLegacyClip(beforeMoveClipName);
            onBeforeMove?.Invoke();
        }

        public void PlayAfterMoveAnimation()
        {
            SetAnimatorBool(isMovingBool, false);
            SetAnimatorTrigger(afterMoveTrigger);
            PlayLegacyClip(afterMoveClipName);
            onAfterMove?.Invoke();
        }

        public void PlayDamageAnimation(int damage)
        {
            SetAnimatorTrigger(damageTrigger);
            PlayLegacyClip(damageClipName);
            onDamaged?.Invoke(damage);
        }

        private void SetAnimatorBool(string parameterName, bool value)
        {
            if (!HasAnimatorParameter(parameterName, AnimatorControllerParameterType.Bool))
                return;

            animator.SetBool(parameterName, value);
        }

        private void SetAnimatorTrigger(string parameterName)
        {
            if (!HasAnimatorParameter(parameterName, AnimatorControllerParameterType.Trigger))
                return;

            animator.SetTrigger(parameterName);
        }

        private bool HasAnimatorParameter(string parameterName, AnimatorControllerParameterType expectedType)
        {
            if (animator == null || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null)
                return false;

            if (string.IsNullOrEmpty(parameterName))
                return false;

            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name == parameterName && parameter.type == expectedType)
                    return true;
            }

            return false;
        }

        private void PlayLegacyClip(string clipName)
        {
            if (legacyAnimation == null || !legacyAnimation.isActiveAndEnabled || string.IsNullOrEmpty(clipName))
                return;

            if (legacyAnimation.GetClip(clipName) == null)
                return;

            legacyAnimation.CrossFade(clipName);
        }
    }
}

