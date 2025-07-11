using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileHandler : MonoBehaviour
{
    public static void SaveToJSON<T>(List<T> listToSave, string fileName)
    {
        string path = GetPersistentPath(fileName);
        string content = JsonHelper.ToJson<T>(listToSave.ToArray());
        //WriteFile(GetPath(fileName), content);
        File.WriteAllText(path, content);
    }
    public static void SaveToJSON<T>(T toSave, string fileName)
    {
        string path = GetPersistentPath(fileName);
        string content = JsonUtility.ToJson(toSave);
        //WriteFile(GetPath(fileName), content);
        File.WriteAllText(path, content);
    }
    //public static List<T> ReadFromJSON<T>(string fileName)
    //{
    //    string path = GetPath(fileName);
    //    if (path == null)
    //    {
    //        return new List<T>();
    //    }
    //    string content = ReadFile(GetPath(fileName));
    //    if (string.IsNullOrEmpty(content) || content == "{}")
    //    {
    //        return new List<T>();
    //    }
    //    List<T> res = JsonHelper.FromJson<T>(content).ToList();
    //    return res;
    //}
    public static List<T> ReadFromJSON<T>(string fileName)
    {
        string path = GetPersistentPath(fileName);

        if (File.Exists(path))
        {
            Debug.Log($"Found save file, loading from: {path}");
            string content = File.ReadAllText(path);

            if (string.IsNullOrEmpty(content) || content == "{}")
            {
                // The file exists but is empty. Return an empty list.
                return new List<T>();
            }

            return JsonHelper.FromJson<T>(content).ToList();
        }
        else
        {
            Debug.LogWarning($"No save file found at '{path}'. Attempting to load default from Resources.");

            // File doesn't exist, so let's try to load it from the Resources folder as a default.
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            TextAsset defaultData = Resources.Load<TextAsset>(fileNameWithoutExtension);

            if (defaultData != null)
            {
                Debug.Log($"Default file '{fileNameWithoutExtension}' found in Resources. Creating new save file.");
                string content = defaultData.text;

                // Save this default data to the persistent path for the next time the game loads
                File.WriteAllText(path, content);

                // Return the data from the default file
                return JsonHelper.FromJson<T>(content).ToList();
            }
            else
            {
                // This is a critical error. The game can't start without its default data.
                Debug.LogError($"FATAL: Could not find default data in Resources folder for: '{fileNameWithoutExtension}'.");

                // Return an empty list to prevent the game from crashing.
                return new List<T>();
            }
        }
    }
    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
    private static void WriteFile(string path, string content)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(content);
        }
    }
    private static string GetPath(string fileName)
    {
        // Persistent data path ensures cross-platform compatibility
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Check if the file exists; if not, copy the default from Resources
        if (!File.Exists(filePath))
        {
            TextAsset defaultData = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(fileName));

            if (defaultData != null)
            {
                File.WriteAllText(filePath, defaultData.text);
            }
            else return null;
        }
        return filePath;
    }

    private static string GetPersistentPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

}
