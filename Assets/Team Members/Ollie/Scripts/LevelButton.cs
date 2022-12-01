using System.Collections;
using System.Collections.Generic;
using Ollie;
using UnityEngine;

namespace Ollie
{
    public class LevelButton : MonoBehaviour
    {
        public string myLevel;

        public void SetSceneToLoad()
        {
            LobbyUIManager.singleton.sceneToLoad = myLevel;
            LobbyUIManager.singleton.UpdateLevelSelectedText(myLevel);
        }
    }
}
