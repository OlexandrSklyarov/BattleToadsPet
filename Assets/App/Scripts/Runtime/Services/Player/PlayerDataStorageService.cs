using System;
using System.Collections.Generic;
using BT.Runtime.Data.Persistent;
using Newtonsoft.Json;
using UnityEngine;

namespace BT.Runtime.Services.Player
{
    public class PlayerDataStorageService : IPlayerDataStorageService, IDisposable
    {       
        public Dictionary<Type, IData> _saveData;

        private const string DATA_KEY = "PlayerSaveData";
        
        public PlayerDataStorageService()
        {
            Load();
        }

        public T GetData<T>() where T : IData
        {
            var type = typeof(T);
            return (T)_saveData[type];
        }

        private void Load()
        {
            var str = PlayerPrefs.GetString(DATA_KEY, string.Empty);           

            var data = (str != string.Empty) ?
                JsonConvert.DeserializeObject<SaveDataContainer>(str) :                
                CreateEmptySaveData();

            _saveData = new Dictionary<Type, IData>()
            {
                {typeof(LevelData), data.Level}
            };
        }

        private SaveDataContainer CreateEmptySaveData()
        {
            return new SaveDataContainer()
            {
                Level = new LevelData()
            };
        }                

        private void Save()
        {
            var data = JsonConvert.SerializeObject(new SaveDataContainer()
            {
                Level = GetData<LevelData>()
            });

            PlayerPrefs.SetString(DATA_KEY, data);

            Debug.Log("Save...");
        }

        public void SaveAndDamp()
        {
            Save();
            PlayerPrefs.Save();
        }

        public void Dispose()
        {
            SaveAndDamp();
        }
    }
}