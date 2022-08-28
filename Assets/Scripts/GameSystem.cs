using System;
using System.Collections.Generic;
using UnityEngine;

namespace BimiBooTest
{
    //������� �������, ������� ������ � ���� ������ �� �������. ��������� ��� ���������� � �������.
    //������ ������ �� ������� �������� � ������������� �������, ���� ������� ����������� ����� � ����������.
    //������������ ����� ������� �� � ScriptableObject ��� JSON ������.
    //��� ������ ���� ���������� �����, � ����������� �� ������, ������� ������������ ItemsSystem.
    public class GameSystem : MonoBehaviour
    {
        [SerializeField]
        private int currentLevelNumber;
        [SerializeField]
        private List<Level> levels;

        public delegate void LevelStartHandle(int mountsCount, int imagesCount, List<int> itemTypes, bool isRandom);
        public event LevelStartHandle LevelStartEvent;

        public void Initialize()
        {

        }

        public void Activate()
        {
            List<int> itemTypes = new();
            for (int i = 0; i < levels[currentLevelNumber].items.Count; i++)
            {
                itemTypes.Add((int)levels[currentLevelNumber].items[i].itemType);
            }
            Level currentLevel = levels[currentLevelNumber];
            LevelStartEvent?.Invoke(currentLevel.mountsCount, currentLevel.imagesCount, itemTypes, currentLevel.isRandom);
        }
    }

    [Serializable]
    public class Level
    {
        public string id;
        public bool isRandom;
        public List<Item> items;
        public int mountsCount;
        public int imagesCount;

        [Serializable]
        public class Item
        {
            public ItemType itemType;
        }
    }
}
