using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHeroSoldier : CharacterBase
{
    Vector2 _movementInput = Vector2.zero;
    Vector2 _lookInput = Vector2.zero;
    Vector2 _mouseLookPosition = Vector2.zero;
    bool _useMouseLook = false;

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
        else
        {
            if(_useMouseLook)
            {
                // Get the direction from the player character to the mouse position 
                Vector2 dirPlayerToMouse = (_mouseLookPosition - _rigidbody2D.position).normalized;

                Vector3 cross = Vector3.Cross(Vector2.up, dirPlayerToMouse);
                float flipValue = cross.z < 0.0f ? -1.0f : 1.0f;
                float rotateAngle = Vector2.Angle(Vector2.up, dirPlayerToMouse);
                _rigidbody2D.MoveRotation(rotateAngle * flipValue);
            }
        }
    }

    void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>(); 
    }

    void OnLook(InputValue inputValue)
    {
        _lookInput = inputValue.Get<Vector2>();
        _useMouseLook = false;
    }

    void OnMouseMove(InputValue inputValue)
    {
        Vector3 mousePosition = inputValue.Get<Vector2>();

        // The 2D Orthographic camera's nearClipPlane needs to be used for the z, otherwise the "z" (forward") will be outside the bounds of our game view
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        _mouseLookPosition = mouseWorldPoint;
        _useMouseLook = true;
    }
}
