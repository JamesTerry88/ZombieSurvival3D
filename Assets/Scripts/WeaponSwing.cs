using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator animator;           // drag the sword Animator here
    [SerializeField] string swingTrigger = "Swing";
    [SerializeField] string swingState = "Sword_Swing";  // exact state name in Animator

    [Header("Damage Call")]
    public MeleeWeapon weapon;                    // drag your MeleeWeapon here
    public bool useAnimationEvent = true;         // ON = call via Animation Event; OFF = timed fallback
    [Range(0f, 1f)] public float hitTime = 0.35f; // when to hit during the swing if not using events

    int swingHash;
    bool hitQueued;   // internal guard so we hit once per swing

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!weapon) weapon = GetComponent<MeleeWeapon>();
        swingHash = Animator.StringToHash(swingState);
    }

    void Update()
    {
        // Start swing on left click
        if (Input.GetMouseButtonDown(0) && animator && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger(swingTrigger);
            hitQueued = !useAnimationEvent; // if we're not using events, arm the timed hit
        }

        // Timed fallback: once the swing reaches hitTime, apply damage
        if (!useAnimationEvent && hitQueued && animator)
        {
            var s = animator.GetCurrentAnimatorStateInfo(0);
            if (s.shortNameHash == swingHash)
            {
                if (s.normalizedTime >= hitTime)
                {
                    weapon?.DoHit();
                    hitQueued = false; // ensure only one hit per swing
                }
            }
            else
            {
                // Left the swing state without hitting; disarm
                hitQueued = false;
            }
        }
    }

    // If you use an Animation Event on the Sword_Swing clip, point it here:
    // (Animation window -> select frame -> Add Event -> choose OnSwingHit)
    public void OnSwingHit()
    {
        if (!useAnimationEvent) return; // ignore if using timed fallback
        weapon?.DoHit();
    }
}
