using System.Collections.Generic;
using UnityEngine;

namespace BimiBooTest
{
    //Система, проигрывающая аудиоклипы в ответ на ивенты.
    public class AudioSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private List<AudioClip> audioClips;

        private ItemsSystem itemsSystem;

        public void Initialize(ItemsSystem itemsSystem)
        {
            this.itemsSystem = itemsSystem;
        }

        public void Activate()
        {
            for (int i = 0; i < itemsSystem.Mounts.Count; i++)
            {
                itemsSystem.Mounts[i].ItemAppearEvent += OnItemAppearEvent;
            }
            for (int i = 0; i < itemsSystem.Images.Count; i++)
            {
                itemsSystem.Images[i].ItemAppearEvent += OnItemAppearEvent;
                ((ImageItem)itemsSystem.Images[i]).ItemSuccessEvent += OnItemSuccessEvent;
            }
        }

        private void OnItemAppearEvent()
        {
            audioSource.PlayOneShot(audioClips[0]);
        }


        private void OnItemSuccessEvent()
        {
            audioSource.PlayOneShot(audioClips[1]);
        }

        private void OnDisable()
        {
            for (int i = 0; i < itemsSystem.Mounts.Count; i++)
            {
                itemsSystem.Mounts[i].ItemAppearEvent -= OnItemAppearEvent;
            }
        }
    }
}
