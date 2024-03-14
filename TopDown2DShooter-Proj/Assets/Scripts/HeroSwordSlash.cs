using UnityEngine;
using UnityEngine.Events;

public class HeroSwordSlash : MonoBehaviour
{
    [SerializeField] Animator _swordSlashAnimator;

    [HideInInspector] public UnityEvent eventSwordSlashStart;
    [HideInInspector] public UnityEvent eventSwordSlashEnd;

    readonly int AnimTrigger_SlashSword = Animator.StringToHash("slashSword");

    void Awake()
    {
        eventSwordSlashStart.AddListener(SwordSlashAnimStart);
    }

    void OnEnable()
    {
        // Trigger the sword slash animation to start
        _swordSlashAnimator.SetTrigger(AnimTrigger_SlashSword);
    }

    void OnDestroy()
    {
        eventSwordSlashStart.RemoveAllListeners();
        eventSwordSlashEnd.RemoveAllListeners();
    }

    void SwordSlashAnimStart()
    {
        Debug.Log("HeroSwordSlash.SwordSlashAnimStart");
    }

    void SwordSlashAnimEnd()
    {
        // The sword slash animation has ended. Notify listenders and hide the sword slash gameobject.
        eventSwordSlashEnd.Invoke();
        gameObject.SetActive(false);
    }
}
