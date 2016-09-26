using UnityEngine;

public class UnitForward : UnitBase
{
    [SerializeField]
    protected Rigidbody _rigidBody = null;

    public override UnitType Type { get { return UnitType.Forward; } }

    protected override void Move()
    {
        _rigidBody.MovePosition(_cachedTransform.position + (_cachedTransform.forward * movementSpeed * Time.deltaTime));
    }
}
