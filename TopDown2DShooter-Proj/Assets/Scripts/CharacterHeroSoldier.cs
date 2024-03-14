using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHeroSoldier : CharacterBase
{
    Vector2 _movementInput = Vector2.zero;

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        if(!_movementInput.Equals(Vector2.zero))
        {
            Vector2 movementDirection = _movementInput;
            Vector2 newPosition = _rigidbody2D.position + movementDirection * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(newPosition);
        }
    }

    void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
