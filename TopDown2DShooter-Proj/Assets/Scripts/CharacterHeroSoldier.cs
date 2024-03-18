using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHeroSoldier : CharacterBase
{
    [Tooltip("How long the player stays in its dash state")]
    [SerializeField] float _dashTime;

    [Tooltip("How quickly the player moves while dashing")]
    [SerializeField] float _dashSpeed;

    [Tooltip("Position where the Pistol bullet will be fired from (i.e. the pistol barrel)")]
    [SerializeField] Transform _firePointPistol;

    [Tooltip("The child GameObject containing the Hero Sword Slash object")]
    [SerializeField] HeroSwordSlash _heroSwordSlash;

    [Header("Spread Gun")]
    [SerializeField] SpreadGunSize _spreadGunSize = SpreadGunSize.SingleBullet;
    [SerializeField] [Range(0.1f, 90.0f)] float _spreadGunAngle = 10.0f;

    [Header("Satellite Weapon")]
    [SerializeField] GameObject _satelliteWeapon;
    [SerializeField] bool _enableSatelliteWeapon = false;

    enum SpreadGunSize
    {
        SingleBullet = 1,
        ThreeBullets = 3,
        FiveBullets = 5,
        SevenBullets = 7,
        NineBullets = 9,
        ElevenBullets = 11
    }

    Vector2 _movementInput = Vector2.zero;
    Vector2 _lookInput = Vector2.zero;
    Vector2 _dashInput = Vector2.zero;
    Vector2 _mouseLookPosition = Vector2.zero;

    bool _useMouseLook = false;
    bool _isDashing = false;
    float _dashTimeElapsed;
    bool _isSwingingSword = false;

    protected override void Start()
    {
        base.Start();
        _heroSwordSlash.gameObject.SetActive(false);
        _heroSwordSlash.eventSwordSlashEnd.AddListener(OnSwordSlashEnd);
        _satelliteWeapon.SetActive(_enableSatelliteWeapon);
    }

    void FixedUpdate()
    {
        // Update Dash
        if(_isDashing)
        {
            _dashTimeElapsed += Time.fixedDeltaTime;
            if(_dashTimeElapsed >= _dashTime)
            {
                _isDashing = false;   
            }

            Vector2 movementDirection = _dashInput;
            Vector2 newPosition = _rigidbody2D.position + movementDirection * _dashSpeed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(newPosition);
            return;
        }

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

    void OnDash(InputValue inputValue)
    {
        // Only allow dashing if the player has some movement input
        if(!_movementInput.Equals(Vector2.zero))
        {
            // Use the player's current movement input to use for the dash input
            _dashInput = _movementInput;
            _isDashing = true;
            _dashTimeElapsed = 0.0f;
            AudioManager.Instance.PlaySound(AudioManager.SFX.Dash);
        }
    }

    void OnFire(InputValue inputValue)
    {
        // Do not allow the player to fire while dashing
        if(_isDashing)
        {
            return;
        }

        // Do not allow the player to fire while swinging the sword
        if(_isSwingingSword)
        {
            return;
        }

        // Always fire the first bullet straight in front of the barrel
        ProjectileController.Instance.SpawnBullet(_firePointPistol.position, _rigidbody2D.transform.rotation);

        int totalBulletsSpawned = 1;
        float angleMultiple = 1.0f;

        // In each loop iteration, spawn 2 bullets in both the left and right rotation directions
        while(totalBulletsSpawned < (int)_spreadGunSize)
        {
            Quaternion leftRotation = _rigidbody2D.transform.rotation * Quaternion.Euler(Vector3.forward * _spreadGunAngle * angleMultiple);
            Quaternion rightRotation = _rigidbody2D.transform.rotation * Quaternion.Euler(Vector3.forward * -_spreadGunAngle * angleMultiple);

            ProjectileController.Instance.SpawnBullet(_firePointPistol.position, leftRotation);
            ProjectileController.Instance.SpawnBullet(_firePointPistol.position, rightRotation);

            angleMultiple += 1.0f;
            totalBulletsSpawned += 2;
        }

        // Play only a single fire sound regardless of how many bullets were spawned
        AudioManager.Instance.PlaySound(AudioManager.SFX.PistolFire);

        // Fire a bullet from the satellite weapon, if enabled
        if(_satelliteWeapon.activeSelf)
        {
            Rigidbody2D satelliteWeaponRigidBody = _satelliteWeapon.GetComponent<Rigidbody2D>();
            ProjectileController.Instance.SpawnBullet(satelliteWeaponRigidBody.position, _rigidbody2D.transform.rotation);
        }
    }

    void OnFire2(InputValue inputValue)
    {
        // Do not allow the player to swing the sword while dashing
        if(_isDashing)
        {
            return;
        }

        // The player should not be able to swing the sword while already swinging the sword
        if(_isSwingingSword)
        {
            return;
        }

        // Unhide the Sword Slash gameobject
        _heroSwordSlash.gameObject.SetActive(true);
        _isSwingingSword = true;
    }

    void OnSwordSlashEnd()
    {
        _isSwingingSword = false;
    }
}
