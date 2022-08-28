using UnityEngine;
using UnityEngine.UI;

namespace BimiBooTest
{
    //Расширяет логику класса Item от которого наследуется.
    public class MountItem : Item
    {
        [SerializeField]
        private GameObject mountedImage;

        private bool isMounted;

        public bool IsMounted { get => isMounted; set => isMounted = value; }

        public override void Initialize(Sprite movingImage, Sprite mountedImage, Transform startTransform, ItemType itemType, float movingTime)
        {
            base.Initialize(movingImage, mountedImage, startTransform, itemType, movingTime);
            this.mountedImage.GetComponent<Image>().sprite = mountedImage;
            MovingImage.gameObject.SetActive(true);
        }

        public void Match()
        {
            isMounted = true;
            PlacedImage.gameObject.SetActive(false);
            mountedImage.SetActive(true);
        }
    }
}
