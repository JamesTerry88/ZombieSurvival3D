using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Hit Settings")]
    public Transform attackOrigin;        // Drag PlayerCamera here
    public float range = 2.2f;
    public float radius = 0.6f;
    public int damage = 25;
    public LayerMask hitMask = ~0;        // Hits everything by default

    // Call this from an Animation Event OR from WeaponSwing's timed fallback.
    public void DoHit()
    {
        if (!attackOrigin)
        {
            var cam = Camera.main;
            attackOrigin = cam ? cam.transform : transform;
        }

        // Sphere cast forward from the camera to simulate a melee arc
        Ray ray = new Ray(attackOrigin.position, attackOrigin.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, range, hitMask, QueryTriggerInteraction.Ignore);

        // Pick the closest ZombieStats we hit (so we don't damage through targets)
        float best = Mathf.Infinity;
        ZombieStats bestTarget = null;

        foreach (var h in hits)
        {
            var zs = h.collider.GetComponentInParent<ZombieStats>();
            if (!zs) continue;

            if (h.distance < best)
            {
                best = h.distance;
                bestTarget = zs;
            }
        }

        if (bestTarget != null)
        {
            bestTarget.TakeDamage(damage);
        }
    }
}
