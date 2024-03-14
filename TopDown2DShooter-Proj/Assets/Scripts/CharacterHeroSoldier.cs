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
        // Using this:
        // https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/#:~:text=In%20Unity%2C%20getting%20the%20mouse,Simple.
        Vector3 mousePosition = inputValue.Get<Vector2>();
        mousePosition.z = Camera.main.nearClipPlane;

        //Vector3 mouseViewportPoint = Camera.main.ScreenToViewportPoint(mousePosition);
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

        _mouseLookPosition = mouseWorldPoint;
        _useMouseLook = true;
    }
}