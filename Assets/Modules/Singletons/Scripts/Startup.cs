using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Potions.Global
{
    [DefaultExecutionOrder(int.MinValue)]
    public class Startup : MonoBehaviour
    {
        private void Awake()
        {
            if (!_isInitialized)
            {
                // Load Master scene the first time the game is opened
                SceneManager.LoadScene("Master", LoadSceneMode.Additive);
                _isInitialized = true;
            }
                
            Destroy(gameObject);
        }

        private static bool _isInitialized;
    }
}