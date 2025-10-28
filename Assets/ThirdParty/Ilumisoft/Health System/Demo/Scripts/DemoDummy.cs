using UnityEngine;

namespace Ilumisoft.HealthSystem.Demo
{
    public class DemoDummy : MonoBehaviour
    {
        HealthComponent HealthComponent { get; set; }

        private void Awake()
        {
            HealthComponent = GetComponent<HealthComponent>();

            HealthComponent.OnHealthEmpty += OnHealthEmpty;
        }

        private void OnHealthEmpty()
        {
            Destroy(gameObject, 1.0f);
        }
    }
}