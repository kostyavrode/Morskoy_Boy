using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game
{
    public class ItemSelector : MonoBehaviour
    {
        public static ItemSelector Instance;
        
        [SerializeField]private ItemMover selectedItem1;
        [SerializeField]private ItemMover selectedItem2;

        public TMP_Text gameScore;
        private int currentScore;
        private ItemSpawner _itemSpawner;

        private void Awake()
        {
            Instance = this;
        }

        [Inject]
        public void Construct(ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
        }

        public void SelectItem(ItemMover item, bool isLine1)
        {
            _itemSpawner.StopSpawningAndMoving(isLine1);
            Debug.Log("IsFirstLine:"+isLine1);
            if (selectedItem1 == null)
            {
                selectedItem1 = item;
                selectedItem1.Stop();
                Debug.Log("Stopped :"+selectedItem1.name);
            }
            else if (selectedItem1 == selectedItem2)
            {
                selectedItem1.Resume();
                selectedItem2.Resume();
                _itemSpawner.ResumeSpawningAndMoving();
                selectedItem1 = null;
                selectedItem2 = null;
            }
            else
            {
                selectedItem2 = item;
                Debug.Log("Stopped :"+selectedItem1.name);
                selectedItem2.Stop();
                CheckMatch();
            }
            
        }
       /* public void SelectItem(ItemMover item, bool isLine1)
        {
            if (isLine1)
            {
                if (selectedItem1 == null)
                {
                    selectedItem1 = item;
                    _itemSpawner.StopSpawningAndMoving(true);
                    selectedItem1.Stop();
                }
                else
                {
                    selectedItem1.Resume();
                    selectedItem1 = null;
                    _itemSpawner.ResumeSpawningAndMoving();
                }
            }
            else
            {
                if (selectedItem2 == null)
                {
                    selectedItem2 = item;
                    _itemSpawner.StopSpawningAndMoving(false);
                    selectedItem2.Stop();
                    
                    CheckMatch();
                }
                else
                {
                    selectedItem2.Resume();
                    selectedItem2 = null;
                    _itemSpawner.ResumeSpawningAndMoving();
                }
            }
        }*/
       public void ResetScore()
       {
           currentScore = 0;
           gameScore.text = currentScore.ToString() + "/12";
           _itemSpawner.ResumeSpawningAndMoving();
       }

        private void CheckMatch()
        {
            if (selectedItem1 != null && selectedItem2 != null)
            {
                if (selectedItem1 == selectedItem2)
                {
                    selectedItem1.Resume();
                    selectedItem2.Resume();
                    selectedItem2.ScaleDown();
                    selectedItem1.ScaleDown();
                }
                else if (selectedItem1.name == selectedItem2.name)
                {
                    // Успешное совпадение – удаляем предметы
                    Destroy(selectedItem1.gameObject);
                    Destroy(selectedItem2.gameObject);
                    selectedItem1 = null;
                    selectedItem2 = null;
                    MoneyController.instance.AddMoney();
                    currentScore += 1;
                    gameScore.text = currentScore.ToString() + "/12";
                }
                else
                {
                    // Неверное совпадение – продолжаем игру
                    selectedItem1.Resume();
                    selectedItem2.Resume();
                    selectedItem2.ScaleDown();
                    selectedItem1.ScaleDown();
                }

                // Возобновляем движение
                _itemSpawner.ResumeSpawningAndMoving();

                selectedItem1 = null;
                selectedItem2 = null;
            }
        }

}
}