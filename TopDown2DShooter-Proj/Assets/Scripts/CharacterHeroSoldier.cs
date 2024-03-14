using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHeroSoldier : CharacterBase
{
    Vector2 _movementInput = Vector2.zero;
    Vector2 _lookInput = Vector2.zero;

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        // Update Movement
        if(!_movementInput.Equals(Vector2.zero))
        {
            Vector2 movementDirection = _movementInput;
            Vector2 newPosition = _rigidbody2D.position + movementDirection * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(newPosition);
        }

        // Update Look
        if(!_lookInput.Equals(Vector2.zero))
        {
            Vector3 cross = Vector3.Cross(Vector2.up, _lookInput);
            float flipValue = cross.z < 0.0f ? -1.0f : 1.0f;
            float rotateAngle = Vector2.Angle(Vector2.up, _lookInput);
            _rigidbody2D.MoveRotation(rotateAngle * flipValue);
        }
    }

    void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>(); 
    }

    void OnLook(InputValue inputValue)
    {
        _lookInput = inputValue.Get<Vector2>();
    }
}
