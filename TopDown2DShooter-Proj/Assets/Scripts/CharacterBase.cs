using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class CharacterBase : MonoBehaviour
{
    [Header("CharacterBase Fields")]
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _moveSpeed;

    protected Rigidbody2D _rigidbody2D;
    protected Collider2D _collider2D;
    protected float _currentHealth;

    protected virtual void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        _collider2D = GetComponent<Collider2D>();
        _collider2D.isTrigger = true;

        _currentHealth = _maxHealth;
    }
}
