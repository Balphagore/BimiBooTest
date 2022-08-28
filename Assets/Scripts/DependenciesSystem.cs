using UnityEngine;

namespace BimiBooTest
{
    //Система, которая инициализирует зависимости между системами а затем активирует их.
    public class DependenciesSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InterfaceSystem interfaceSystem;
        [SerializeField]
        private ItemsSystem itemsSystem;
        [SerializeField]
        private GameSystem gameSystem;
        [SerializeField]
        private AudioSystem audioSystem;

        private void Start()
        {
            interfaceSystem.Initialize(itemsSystem);
            itemsSystem.Initialize(gameSystem, interfaceSystem);
            gameSystem.Initialize();
            audioSystem.Initialize(itemsSystem);

            interfaceSystem.Activate();
            itemsSystem.Activate();
            gameSystem.Activate();
            audioSystem.Activate();
        }
    }
}
