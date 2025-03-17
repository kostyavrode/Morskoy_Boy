using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using DI.SignalBus.States; // Пространство имен для сигналов

namespace Game
{
    public class ItemSpawner : MonoBehaviour
    {
        public GameObject[] itemPrefabs;
        public Transform spawnPointLine1;
        public Transform spawnPointLine2;
        public float spawnInterval = 1.5f;

        public GameObject cameraObject;
        
        public Transform startcamerapos;
        public Transform endcamerapos;
        public Vector3 startRotation;
        public Vector3 endRotation;

        private List<ItemMover> itemsLine1 = new List<ItemMover>();
        private List<ItemMover> itemsLine2 = new List<ItemMover>();

        private bool isSpawning = false;
        private bool isPaused = false;
        private bool isLine1Stopped = false;
        private bool isLine2Stopped = false;

        private Coroutine spawnCoroutine;
        private SignalBus _signalBus; // SignalBus для подписки

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            // Подписка на сигнал смены состояния игры
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            // Отписка от сигнала, чтобы избежать утечек памяти
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void StartSpawning()
        {
            if (!isSpawning)
            {
                isSpawning = true;
                spawnCoroutine = StartCoroutine(SpawnItems());
                ItemSelector.Instance.ResetScore();
                //cameraObject.transform.DOMove(endcamerapos.position, 1);
            }
        }

        public void StartCameraMove()
        {
            cameraObject.transform.DOMove(endcamerapos.position, 1);
        }

        public void StopSpawning()
        {
            isSpawning = false;
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
                cameraObject.transform.DOMove(startcamerapos.position, 1);
            }
        }

        public void PauseSpawning()
        {
            isPaused = true;
        }

        public void ResumeSpawning()
        {
            isPaused = false;
            StartCameraMove();
        }

        private IEnumerator SpawnItems()
        {
            while (isSpawning)
            {
                yield return new WaitUntil(() => !isPaused); // Ждем, пока пауза не снимется

                if (!isLine1Stopped)
                    SpawnItem(spawnPointLine1, itemsLine1);

                if (!isLine2Stopped)
                    SpawnItem(spawnPointLine2, itemsLine2);

                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnItem(Transform spawnPoint, List<ItemMover> itemList)
        {
            // Проверка, нет ли уже предмета на этой позиции
            if (itemList.Count > 0 && itemList.Exists(item => item != null && Vector3.Distance(item.transform.position, spawnPoint.position) < 0.1f))
            {
                return; // Не спавним новый предмет, если он уже есть в месте спавна
            }

            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            GameObject newItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
            ItemMover itemMover = newItem.GetComponent<ItemMover>();
            itemMover._signalBus = _signalBus;
            itemList.Add(itemMover);
        }

        public void StopSpawningAndMoving(bool isLine1)
        {
            if (isLine1)
            {
                isLine1Stopped = true;
                foreach (var item in itemsLine1)
                    item.Stop();
            }
            else
            {
                isLine2Stopped = true;
                foreach (var item in itemsLine2)
                    item.Stop();
            }
        }

        public void ResumeSpawningAndMoving()
        {
            isLine1Stopped = false;
            isLine2Stopped = false;

            foreach (var item in itemsLine1)
                item.Resume();

            foreach (var item in itemsLine2)
                item.Resume();
        }

        // Метод удаления всех заспавненных предметов
        private void ClearAllItems()
        {
            // Удаляем все предметы с линии 1
            foreach (var item in itemsLine1)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            itemsLine1.Clear(); // Очищаем список

            // Удаляем все предметы с линии 2
            foreach (var item in itemsLine2)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            itemsLine2.Clear(); // Очищаем список
        }

        // Метод обработки смены состояния игры
        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.StateType == typeof(GameStates.MenuState))
            {
                ClearAllItems(); // Удаляем все предметы при переходе в меню
            }
        }
    }
}
