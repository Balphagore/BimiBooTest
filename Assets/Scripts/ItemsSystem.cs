using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace BimiBooTest
{
    //—истема, котора€ инстанциирует и контроллирует предметы на игровом поле.
    //Ќа будущее заложен функционал спавна различного количества Mount и Image в зависимости от параметров, которые приход€т в ивенте от GameSystem
    public class ItemsSystem : MonoBehaviour
    {
        [SerializeField]
        private float mountMatchDistance;
        [SerializeField]
        private float itemsSpawnDelay;
        [SerializeField]
        private float itemsMovingTime;

        [SerializeField]
        private int maxMountsCount;
        private List<Item> mounts;

        [SerializeField]
        private int maxImagesCount;
        private List<Item> images;
        private List<int> itemTypes;
        private List<int> spawnedItemTypes;

        private float gameFieldSize;
        private float scaleValue;

        [Header("References")]
        [SerializeField]
        private GameObject mountPrefab;
        [SerializeField]
        private Transform mountsPanel;
        [SerializeField]
        private GridLayoutGroup mountsGridLayoutGroup;
        [SerializeField]
        private Transform mountStartPosition;

        [SerializeField]
        private GameObject imagePrefab;
        [SerializeField]
        private Transform imagesPanel;
        [SerializeField]
        private GridLayoutGroup imagesGridLayoutGroup;
        [SerializeField]
        private Transform imageStartPosition;

        [SerializeField]
        private SpriteAtlas itemsSpriteAtlas;

        private GameSystem gameSystem;
        private InterfaceSystem interfaceSystem;

        public List<Item> Mounts { get => mounts; set => mounts = value; }
        public List<Item> Images { get => images; set => images = value; }

        public delegate void WinGameHandle();
        public event WinGameHandle WinGameEvent;

        public void Initialize(GameSystem gameSystem, InterfaceSystem interfaceSystem)
        {
            this.gameSystem = gameSystem;

            gameSystem.LevelStartEvent += OnLevelStartEvent;

            this.interfaceSystem = interfaceSystem;

            interfaceSystem.InterfaceScaleEvent += OnInterfaceScaleEvent;
        }

        public void Activate()
        {
            float cellSize = gameFieldSize / maxMountsCount - mountsGridLayoutGroup.spacing.x;

            mountsGridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
            imagesGridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

            mounts = new();
            for (int i = 0; i < maxMountsCount; i++)
            {
                GameObject instance = Instantiate(mountPrefab, mountsPanel);
                instance.name = "Mount_" + i;
                Item item = instance.GetComponent<Item>();
                mounts.Add(item);
            }

            images = new();
            for (int i = 0; i < maxImagesCount; i++)
            {
                GameObject instance = Instantiate(imagePrefab, imagesPanel);
                instance.name = "Image_" + i;
                Item item = instance.GetComponent<Item>();
                images.Add(item);
                ((ImageItem)item).ItemSuccessEvent += OnItemSuccessEvent;
            }
            Canvas.ForceUpdateCanvases();
            imageStartPosition.GetComponent<RectTransform>().localPosition = new Vector2(imageStartPosition.GetComponent<RectTransform>().localPosition.x + cellSize / 2, 0);
            mountStartPosition.GetComponent<RectTransform>().localPosition = new Vector2(mountStartPosition.GetComponent<RectTransform>().localPosition.x - cellSize / 2, mountStartPosition.GetComponent<RectTransform>().localPosition.y + cellSize / 2);

            spawnedItemTypes = new();
        }

        private void OnDisable()
        {
            gameSystem.LevelStartEvent -= OnLevelStartEvent;
            interfaceSystem.InterfaceScaleEvent -= OnInterfaceScaleEvent;
        }

        private void OnInterfaceScaleEvent(float gameFieldSize, float scaleValue)
        {
            this.gameFieldSize = gameFieldSize;
            this.scaleValue = scaleValue;
        }

        private void OnItemSuccessEvent()
        {
            if (spawnedItemTypes.Count < itemTypes.Count)
            {
                StartCoroutine(SpawnItem(1, itemTypes, false, true));
            }
            else
            {
                foreach (ImageItem imageItem in images)
                {
                    imageItem.MovingImage.gameObject.SetActive(false);
                }
                WinGameEvent?.Invoke();
            }
        }

        private void OnLevelStartEvent(int mountsCount, int imagesCount, List<int> itemTypes, bool isRandom)
        {
            this.itemTypes = itemTypes;
            for (int i = 0; i < mounts.Count; i++)
            {
                if (i >= mountsCount)
                {
                    mounts[i].gameObject.SetActive(false);
                }
            }
            StartCoroutine(SpawnItem(mountsCount, itemTypes, true, false));
            for (int i = 0; i < images.Count; i++)
            {
                if (i >= imagesCount)
                {
                    images[i].gameObject.SetActive(false);
                }
            }
            StartCoroutine(SpawnItem(imagesCount, itemTypes, false, true));
        }

        private IEnumerator SpawnItem(int count, List<int> itemTypes, bool isMount, bool isRandom)
        {
            int counter = 0;
            float elapsedTime = itemsSpawnDelay;
            float waitTime = itemsSpawnDelay;
            int index;
            while (counter < count)
            {
                elapsedTime += Time.deltaTime;
                if (isRandom)
                {
                    if (isMount)
                    {
                        index = Random.Range(0, itemTypes.Count);
                    }
                    else
                    {
                        index = Random.Range(0, itemTypes.Count);
                        while (spawnedItemTypes.Contains(index))
                        {
                            index = Random.Range(0, itemTypes.Count);
                        }
                    }
                }
                else
                {
                    index = counter;
                }
                if (elapsedTime >= waitTime)
                {
                    elapsedTime = 0;
                    if (isMount)
                    {
                        mounts[counter].Initialize(itemsSpriteAtlas.GetSprite((ItemType)itemTypes[index] + "_Mount"), itemsSpriteAtlas.GetSprite((ItemType)itemTypes[index] + "_Image"), mountStartPosition, (ItemType)itemTypes[index], itemsMovingTime);
                    }
                    else
                    {
                        images[counter].Initialize(itemsSpriteAtlas.GetSprite((ItemType)itemTypes[index] + "_Image"), imageStartPosition, mountMatchDistance * scaleValue, (ItemType)itemTypes[index], itemsMovingTime);
                        spawnedItemTypes.Add(index);
                    }
                    counter++;
                }
                yield return null;
            }
        }
    }
}
