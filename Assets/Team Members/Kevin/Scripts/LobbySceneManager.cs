using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Kevin
{
    [Serializable]
    public class Level
    {
        public Object level;
        public string levelNameOnUI;
    }
    
    public class LobbySceneManager : NetworkBehaviour
    {

        [Header("Level Manager")] public List<Level> levels;
        
        //Level Selecter Manager
        public static LobbySceneManager instance;
        public string selectedLevel;
        public string selectedSceneToLoad;
        
        public GameObject levelPrefab;

        public Transform selectLevelPanelTransform;

        private Transform updateTransform;
        //UI Manager
        
        public GameObject ipCanvas;
        public GameObject lobbyCanvas;
        public GameObject selectLevelGameObject;
        public Button startButton;

        public TMP_Text selectedLevelText;
        
        //IP Address
        public TMP_InputField ipInputField;


        public void Awake()
        {
            instance = this;
        }
        
        #region ButtonManager

        public void HostGameButton()
        {
            /*check if input is entered or not, change to specifics later
            if (ipInputField.text == "")
            {
                Debug.Log("You must type in IP Address!");
                return;
            }*/
            
            NetworkManager.Singleton.StartHost();
            ipCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);

            foreach (Level level in levels)
            {
                GameObject levelObject = Instantiate(levelPrefab, selectLevelPanelTransform);
                levelObject.GetComponentInChildren<TMP_Text>().text = level.levelNameOnUI;
                levelObject.GetComponent<LevelButton>().currentLevel = level.level.name;
                //need to set scene to load if that level is selected
            }
        }

        public void JoinGameButton()
        {
            /*check if input is entered or not, change to specifics later
            if (ipInputField.text == "")
            {
                Debug.Log("You must type in IP Address!");
                return;
            }*/
            NetworkManager.Singleton.StartClient();
            ClientLobby();
        }

        void ClientLobby()
        {
            ipCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            startButton.gameObject.SetActive(false);
            selectLevelGameObject.SetActive(false);
        }

        public void StartGameButton()
        {
            if (selectedLevel == "")
            {
                Debug.Log("You must select a level!!!");
                return;
            }

            try
            {
                NetworkManager.Singleton.SceneManager.LoadScene(selectedLevelText.text, LoadSceneMode.Additive);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        #endregion

        public void UpdateLevelText(string level)
        {
            selectedLevelText.text = level;
        }
    }

}
