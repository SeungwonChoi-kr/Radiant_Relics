using UnityEngine;

namespace Ilumisoft.HealthSystem.UI
{
    [AddComponentMenu("Health System/UI/Player Healthbar")]
    [DefaultExecutionOrder(10)]
    public class PlayerHealthbar : Healthbar
    {
        public GameObject Player;

        protected virtual void Awake()
        {
            if (Player != null)
            {
                Health = Player.GetComponent<HealthComponent>();
            }
        }
    }
}