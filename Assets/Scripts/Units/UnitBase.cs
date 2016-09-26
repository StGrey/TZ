using UnityEngine;
using System;

public abstract class UnitBase : MonoBehaviour, IUnitPoolObject
{
    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    private CapsuleCollider _capsule = null;

    public float movementSpeed = 1f;

    public bool IsDead { get; private set; }

    protected Transform _cachedTransform = null;

    private float _endZPoint = 0f;
    private Action _onDeadEffectDoneHandler = null;
    private Action<UnitBase> _onUnitPassBottomLineHandler = null;

    protected virtual void Awake()
    {
        _cachedTransform = transform;
    }

    public void Init(Vector3 startPosition, float endZPoint, Action<UnitBase> onUnitPassBottomLineHandler)
    {
        _onUnitPassBottomLineHandler = onUnitPassBottomLineHandler;

        _cachedTransform.position = startPosition;
        _endZPoint = endZPoint;
    }

    protected virtual void FixedUpdate()
    {
        if (!IsDead)
        {
            Move();
            CheckForPassingBottomLine();
        }
    }

    private void CheckForPassingBottomLine()
    {
        if (_cachedTransform.position.z + _capsule.radius < _endZPoint)
        {
            IsDead = true;

            if (_onUnitPassBottomLineHandler != null)
            {
                _onUnitPassBottomLineHandler(this);
                _onUnitPassBottomLineHandler = null;
            }
        }
    }

    public void Dead(Action onDeadEffectDoneHandler)
    {
        _onUnitPassBottomLineHandler = null;

        _onDeadEffectDoneHandler = onDeadEffectDoneHandler;
        IsDead = true;

        _capsule.enabled = false;

        _animator.applyRootMotion = true;
        _animator.SetTrigger("dead");
    }

    public void OnDeadAnimationDone()
    {
        if (_onDeadEffectDoneHandler != null)
        {
            _onDeadEffectDoneHandler();
        }
    }

    public virtual void Reset()
    {
        IsDead = false;

        _animator.applyRootMotion = false;

        _cachedTransform.localEulerAngles = new Vector3(0f, -180f, 0f);

        _capsule.enabled = true;

        _onDeadEffectDoneHandler = null;
        _onUnitPassBottomLineHandler = null;

        gameObject.SetActive(false);
    }

    public virtual void Use()
    {
        gameObject.SetActive(true);
    }

    public abstract UnitType Type { get; }

    protected abstract void Move();
}