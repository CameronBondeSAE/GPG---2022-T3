using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ollie
{
    public class ProjectSceneManager : NetworkBehaviour
    {
#if UNITY_EDITOR
        public SceneAsset SceneAsset;
        private void OnValidate()
        {
            if (SceneAsset != null)
            {
                m_SceneName = SceneAsset.name;
            }
        }
#endif
        private string m_SceneName;
        public override void OnNetworkSpawn()
        {
            if (IsServer && !string.IsNullOrEmpty(m_SceneName))
            {
                var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Additive);
                if (status != SceneEventProgressStatus.Started)
                {
                    print("Failed to load " + m_SceneName + "with a " + nameof(SceneEventProgressStatus));
                }
            }
        }
    }
}
