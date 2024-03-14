using System.Reflection;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public static ProjectileController Instance { get; private set; }

    [SerializeField] PistolBullet _pistolBulletPrefab;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogWarning(GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name + " - Instance of ProjectileController already exists!");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void SpawnBullet(Vector2 bulletPosition, Quaternion bulletRotation)
    {
        // RKS TODO: Save the references for pooling
        PistolBullet newPistolBullet = GameObject.Instantiate(_pistolBulletPrefab, bulletPosition, bulletRotation);
    }
}
