using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Utility
{
    public static class SceneLoader
    {
        public static int CurrentIndex = SceneManager.GetActiveScene().buildIndex;
        
        public static void LoadNextScene()
        {
            int nextScene = CurrentIndex % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextScene);
        }

        public static void LoadStart()
        {
            SceneManager.LoadScene(0);
        }

        public static void LoadGame()
        {
            SceneManager.LoadScene(1);
        }

        public static void Quit()
        {
            Debug.Log("Quitting application...");
        }
    }
}