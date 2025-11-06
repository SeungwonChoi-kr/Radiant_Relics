using UnityEngine;

namespace Ilumisoft.HealthSystem
{
    /// <summary>
    /// Default implementation of the hitbox component.
    /// It allows you to define a damage multiplier in the inspector.
    /// In that way you can create multiple hitboxes on an actor for different damage zones (head, body, arms,...)
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("Health System/Hitbox")]
    public class Hitbox : HitboxComponent
    {
        [field: SerializeField]
        public override HealthComponent Health { get; protected set; }

        [Range(0f, 1f)]
        public float damageMultiplier = 1.0f;

        void Reset()
        {
            if (Health == null)
            {
                Health = GetComponentInParent<HealthComponent>();
            }
        }

        public override void ApplyDamage(float damageAmount)
        {
            if (isActiveAndEnabled && Health != null)
            {
                Health.ApplyDamage(damageAmount * damageMultiplier);
            }
        }
    }
}