using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SatelliteWeapon : MonoBehaviour
{
    [Tooltip("Degrees per second")]
    [SerializeField] float _rotationSpeed;

    [Tooltip("Distance away from the 'owner' (basically the radius of rotation)")]
    [SerializeField] float _distanceFromOwner;

    [Tooltip("Clockwise or Counterwise rotation")]
    [SerializeField] RotationDirection _rotationDirection;

    [Tooltip("The GameObject this 'satellite' will rotate around")]
    [SerializeField] GameObject _owner;

    Rigidbody2D _rigidbody2D;
    Rigidbody2D _ownerRigidbody2D;
    float _curRotationAngle = 0.0f;
    
    enum RotationDirection
    {
        Clockwise,
        Counterclockwise
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _ownerRigidbody2D = _owner.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Increment the rotation angle
        float rotationDirMultiple = _rotationDirection == RotationDirection.Clockwise ? 1.0f : -1.0f;
        _curRotationAngle += _rotationSpeed * Time.fixedDeltaTime * rotationDirMultiple;

        // Ensure the rotation angle stays within (-360, 360)
        if(_curRotationAngle >= 360.0f)
        {
            _curRotationAngle = 0.0f;
        }
        else if(_curRotationAngle <= -360.0f)
        {
            _curRotationAngle = 0.0f;
        }

        // Calculate the local X and Y positions with the current rotation
        // Using SOHCAHTOA and Polar Coordinates: (opp = y, adj = x, r = hyp)
        // x = r * cos(theta)  <= cos(theta) = x / r
        // y = r * sin(theta)  <= sin(theta) = y / r
        // r = _distanceFromOwner
        // theta = _curRotationAngle

        float xPos = _distanceFromOwner * Mathf.Cos(_curRotationAngle * Mathf.Deg2Rad);
        float yPos = _distanceFromOwner * Mathf.Sin(_curRotationAngle * Mathf.Deg2Rad);

        // Adjust the position with the owner's position (move the position to the owner's coordinate space)
        Vector2 newPosition = new Vector2(xPos, yPos) + _ownerRigidbody2D.position;
        _rigidbody2D.MovePosition(newPosition);
    }
}
