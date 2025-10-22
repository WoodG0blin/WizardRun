using System.IO;
using UnityEngine;

namespace WizardsPlatformer
{
    public static class DataSaveAndLoad
    {
        private static string DATA_PATH = Application.persistentDataPath + "/PlayerData.json";

        public static void Save(PlayerSavedData data)
        {
            Debug.Log("Saving game to " + DATA_PATH);
            var save = JsonUtility.ToJson(data);
            File.WriteAllText(DATA_PATH, save);
        }

        public static PlayerSavedData Load()
        {
            Debug.Log("Loading from " + DATA_PATH);

            PlayerSavedData loaded = new();

            if (File.Exists(DATA_PATH))
            {
                loaded = JsonUtility.FromJson<PlayerSavedData>(File.ReadAllText(DATA_PATH));
            }

            return loaded;
        }
    }
}
