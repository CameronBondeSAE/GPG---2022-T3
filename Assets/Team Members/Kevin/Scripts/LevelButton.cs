using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class LevelButton : MonoBehaviour
    {
        public string currentLevel;

        public void SetSelectedScene()
        {
            LobbySceneManager.instance.selectedLevel = currentLevel;
            LobbySceneManager.instance.UpdateLevelText(currentLevel);
        }
    }
}

