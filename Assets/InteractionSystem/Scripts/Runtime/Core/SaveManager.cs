using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LuduInteraction.Runtime.Core
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private const string SAVE_FILE_NAME = "interaction_save.json";
        private Dictionary<string, string> m_StateDictionary = new Dictionary<string, string>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Load(); // Load on start
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void SetState(string id, object value)
        {
            if (string.IsNullOrEmpty(id)) return;
            string val = value.ToString();

            if (m_StateDictionary.ContainsKey(id))
                m_StateDictionary[id] = val;
            else
                m_StateDictionary.Add(id, val);
            
            Debug.Log($"[SaveManager] Set State: {id} = {val}");
        }

        public bool GetBool(string id, bool defaultValue = false)
        {
            if (string.IsNullOrEmpty(id) || !m_StateDictionary.ContainsKey(id)) return defaultValue;
            return bool.Parse(m_StateDictionary[id]);
        }

        public string GetString(string id, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(id) || !m_StateDictionary.ContainsKey(id)) return defaultValue;
            return m_StateDictionary[id];
        }

        [ContextMenu("Save Game")]
        public void Save()
        {
            SaveWrapper wrapper = new SaveWrapper();
            foreach (var kvp in m_StateDictionary)
            {
                wrapper.Keys.Add(kvp.Key);
                wrapper.Values.Add(kvp.Value);
            }

            string json = JsonUtility.ToJson(wrapper, true);
            string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            File.WriteAllText(path, json);
            Debug.Log($"Game Saved to: {path}");
        }

        [ContextMenu("Load Game")]
        public void Load()
        {
            string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);

                m_StateDictionary.Clear();
                for (int i = 0; i < wrapper.Keys.Count; i++)
                {
                    if (i < wrapper.Values.Count)
                    {
                        m_StateDictionary.Add(wrapper.Keys[i], wrapper.Values[i]);
                    }
                }
                Debug.Log("Game Loaded");
            }
        }

        [ContextMenu("Clear Save Data")]
        public void ClearSave()
        {
            string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Save file deleted at: {path}");
            }
            
            m_StateDictionary.Clear();
            Debug.Log("Save Data Cleared.");
        }

        [Serializable]
        private class SaveWrapper
        {
            public List<string> Keys = new List<string>();
            public List<string> Values = new List<string>();
        }
    }
}
