using UnityEngine;

public class UnitForwardDiagonal : UnitForward
{
    public float rotationOffset = 45f;
    public float rotationSpeed = 10f;

    private Quaternion _targetRot;

    public override void Use()
    {
        base.Use();

        SetStartRotation();
    }

    private void SetStartRotation()
    {
        Vector3 rot = _cachedTransform.localEulerAngles;

        int randNum = Random.Range(0, 3);
        if (randNum == 0)
        {
            rot.y -= rotationOffset;
        }
        else if (randNum == 1)
        {
            rot.y += rotationOffset;
        }

        _targetRot = Quaternion.Euler(rot);
        _rigidBody.MoveRotation(Quaternion.Euler(rot));
    }

    private void InvertRotation()
    {
        Vector3 rot = _cachedTransform.localEulerAngles;
        rot.y *= -1f;

        _targetRot = Quaternion.Euler(rot);
    }

    protected override void Move()
    {
        base.Move();

        _rigidBody.MoveRotation(Quaternion.Lerp(_cachedTransform.rotation, _targetRot, rotationSpeed * Time.deltaTime));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            InvertRotation();
        }
    }
}
