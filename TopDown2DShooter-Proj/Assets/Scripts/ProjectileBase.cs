using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class ProjectileBase : MonoBehaviour
{
    [Header("ProjectileBase Fields")]
    [SerializeField] float _movementSpeed;
    [SerializeField] float _lifeTimeSeconds;

    Rigidbody2D _rigidbody2D;
    float _timeAlive = 0.0f;

    protected virtual void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if(Mathf.Approximately(_movementSpeed, 0.0f))
        {
            Debug.LogWarning(GetType().Name + ".Start - _movementSpeed is zero (which means it likely has not been set in the Prefab");
        }

        if(Mathf.Approximately(_lifeTimeSeconds, 0.0f))
        {
            Debug.LogWarning(GetType().Name + ".Start - _lifeTimeSeconds is zero (which means it likely has not been set in the Prefab");
        }
    }

    protected virtual void FixedUpdate()
    {
        // By default, move directly forward (2D up) direction
        // Use the "Up" vector as that is actually the forward vector in Unity 2D (Note: "forward" refers to the Z direction, i.e. in the camera facing direction)
        Vector2 movementDirection = _rigidbody2D.transform.up;
        Vector2 newPos = _rigidbody2D.position + movementDirection * _movementSpeed * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(newPos);

        // Destroy owning GameObject if time alive has exceeded the lifetime
        _timeAlive += Time.fixedDeltaTime;
        if(_timeAlive >= _lifeTimeSeconds)
        {
            Destroy(gameObject);
        }
    }
}
