using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BimiBooTest
{
    //Система интерфейса. Скалирует его для соответствия разным разрешениям.
    public class InterfaceSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private SpriteRenderer background;
        [SerializeField]
        private CanvasScaler canvasScaler;
        [SerializeField]
        private RectTransform panel;
        [SerializeField]
        private TextMeshProUGUI winText;
        [SerializeField]
        private Button restartButton;

        private ItemsSystem itemsSystem;

        public delegate void InterfaceScaleHandle(float gameFieldSize, float scaleValue);
        public event InterfaceScaleHandle InterfaceScaleEvent;

        public void Initialize(ItemsSystem itemsSystem)
        {
            this.itemsSystem = itemsSystem;
            itemsSystem.WinGameEvent += OnWinGameEvent;
        }

        public void Activate()
        {
            Vector2 size = new Vector2(2 * Camera.main.orthographicSize * Camera.main.aspect, 2 * Camera.main.orthographicSize);
            background.size = size;
            canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            panel.sizeDelta = new Vector2(Screen.width / 4 * 3, panel.sizeDelta.y);

            float referenceHeight = canvasScaler.referenceResolution.y;
            float scale = referenceHeight * canvasScaler.GetComponent<RectTransform>().localScale.x / 100;

            InterfaceScaleEvent?.Invoke(Screen.width / 4 * 3, scale);
            winText.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            itemsSystem.WinGameEvent -= OnWinGameEvent;
        }

        private void OnWinGameEvent()
        {
            winText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }

        public void OnRestartButtonClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}
