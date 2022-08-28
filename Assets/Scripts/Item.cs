using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BimiBooTest
{
    //Родительский класс предметов. От него наследуются Mount и Image чтобы реализовывать общую логику.
    public class Item : MonoBehaviour
    {
        private bool isPlaced;
        private ItemType itemType;
        private float movingTime;

        [Header("References")]
        [SerializeField]
        private Image placedImage;
        [SerializeField]
        private Transform movingImage;

        public Image PlacedImage { get => placedImage; set => placedImage = value; }
        public Transform MovingImage { get => movingImage; set => movingImage = value; }
        public bool IsPlaced { get => isPlaced; set => isPlaced = value; }
        public ItemType ItemType { get => itemType; set => itemType = value; }
        public float MovingTime { get => movingTime; set => movingTime = value; }

        public delegate void ItemAppearHandle();
        public event ItemAppearHandle ItemAppearEvent;

        public virtual void Initialize(Sprite movingImage, Sprite mountedImage, Transform startTransform, ItemType itemType, float movingTime)
        {
            this.itemType = itemType;
            this.movingTime = movingTime;
            MoveImage(movingImage, startTransform);
        }

        public virtual void Initialize(Sprite movingImage, Transform startTransform, float interfaceScale, ItemType itemType, float movingTime)
        {
            this.itemType = itemType;
            this.movingTime = movingTime;
            MoveImage(movingImage, startTransform);
        }

        void MoveImage(Sprite sprite, Transform startTransform)
        {
            placedImage.sprite = sprite;
            placedImage.gameObject.SetActive(false);
            movingImage.GetComponent<Image>().sprite = sprite;
            movingImage.transform.position = startTransform.position;
            movingImage.transform.position = new Vector3(startTransform.position.x + transform.position.x, startTransform.position.y, startTransform.position.z);
            movingImage
                .DOLocalMoveX(placedImage.transform.localPosition.x, movingTime)
                .SetEase(Ease.OutQuad)
                .OnComplete(OnAnimationComplete);
            movingImage
                .DOLocalMoveY(placedImage.transform.localPosition.y, movingTime)
                .SetEase(Ease.InQuad)
                .OnComplete(OnAnimationComplete);
        }

        public void OnAnimationComplete()
        {
            placedImage.gameObject.SetActive(true);
            isPlaced = true;
            ItemAppearEvent?.Invoke();
        }
    }
}
