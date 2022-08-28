using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace BimiBooTest
{
    //Расширяет логику класса Item от которого наследуется. 
    public class ImageItem : Item, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private float interfaceScale;

        public delegate void ItemSuccessHandle();
        public event ItemSuccessHandle ItemSuccessEvent;

        public override void Initialize(Sprite sprite, Transform startTransform, float interfaceScale, ItemType itemType, float movingTime)
        {
            base.Initialize(sprite, startTransform, interfaceScale, itemType, movingTime);
            this.interfaceScale = interfaceScale;
            MovingImage.gameObject.SetActive(true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsPlaced = false;
            PlacedImage.gameObject.SetActive(false);
            MovingImage.gameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            MovingImage.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y, 0);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float distance = Vector2.Distance(MovingImage.position, eventData.pointerEnter.gameObject.transform.position);
            MountItem mountItem = eventData.pointerEnter.gameObject.GetComponentInParent<MountItem>();
            if (mountItem != null && (Vector2.Distance(MovingImage.position, eventData.pointerEnter.gameObject.transform.position) <= interfaceScale) && ItemType == mountItem.ItemType && !mountItem.IsMounted)
            {
                ItemSuccessEvent?.Invoke();
                mountItem.Match();
            }
            else
            {
                MovingImage
                    .DOLocalMoveX(PlacedImage.transform.localPosition.x, MovingTime)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(OnAnimationComplete);
                MovingImage
                    .DOLocalMoveY(PlacedImage.transform.localPosition.y, MovingTime)
                    .SetEase(Ease.InQuad)
                    .OnComplete(OnAnimationComplete);
            }
        }
    }
}
