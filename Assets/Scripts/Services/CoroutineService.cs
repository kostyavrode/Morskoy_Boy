using System.Collections;
using UnityEngine;

namespace Services
{
    public class CoroutineService : MonoBehaviour
    {
        public void Run(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}