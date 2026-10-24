using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    [Serializable]
    public abstract partial class JsonSettings<T> where T : JsonSettings<T>, new()
    {
        [AutoStaticsCleanup] public static T current { get; protected set; }

        protected virtual string settingsPath { get; }
        [AutoStaticsCleanup] private static event Action<T> _onSettingsLoaded;

        [field: NonSerialized] public bool isLoaded { get; private set; }
        [SerializeField] public Dictionary<string, bool> bools;
        [SerializeField] public Dictionary<string, double> numbers;
        [SerializeField] public Dictionary<string, string> strings;

        [NonSerialized] public Dictionary<string, Action<bool>> onBoolChanged;
        [NonSerialized] public Dictionary<string, Action<double>> onNumberChanged;
        [NonSerialized] public Dictionary<string, Action<string>> onStringChanged;

        /// <summary>
        /// Subscribe to receive a callback whenever this data is loaded from disc.
        /// If the data is already loaded, the callback fires immediately.
        /// </summary>
        public static void AddLoadListener(Action<T> action)
        {
            _onSettingsLoaded += action;
            if (current != null && current.isLoaded)
            {
                action.Invoke(current);
            }
        }

        public static void RemoveLoadListener(Action<T> action)
        {
            _onSettingsLoaded -= action;
        }

        #region TryGet

        public bool TryGetBool(string settingName, out bool output) => bools.TryGetValue(settingName, out output);
        public bool TryGetNumber(string settingName, out double output) => numbers.TryGetValue(settingName, out output);
        public bool TryGetString(string settingName, out string output) => strings.TryGetValue(settingName, out output);

        #endregion

        // --- Set

        #region Set

        public void SetBool(string name, bool value, bool writeImmediately = true)
        {
            bools[name] = value;
            onBoolChanged[name]?.Invoke(value);
            if (writeImmediately) WriteSettingsToFile();
        }

        public void SetNumber(string name, double value, bool writeImmediately = true)
        {
            numbers[name] = value;
            onNumberChanged[name]?.Invoke(value);
            if (writeImmediately) WriteSettingsToFile();
        }

        public void SetString(string name, string value, bool writeImmediately = true)
        {
            strings[name] = value;
            onStringChanged[name]?.Invoke(value);
            if (writeImmediately) WriteSettingsToFile();
        }

        #endregion


        #region Register

        public void RegisterBool(string name, bool value, bool writeImmediately = true)
        {
            if (!bools.ContainsKey(name)) SetBool(name, value, writeImmediately);
            onBoolChanged.TryAdd(name, null);
        }

        public void RegisterNumber(string name, double value, bool writeImmediately = true)
        {
            if (!numbers.ContainsKey(name)) SetNumber(name, value, writeImmediately);
            onNumberChanged.TryAdd(name, null);
        }

        public void RegisterString(string name, string value, bool writeImmediately = true)
        {
            if (!strings.ContainsKey(name)) SetString(name, value, writeImmediately);
            onStringChanged.TryAdd(name, null);
        }

        #endregion


        #region Remove

        public void RemoveBoolValue(string name, bool writeImmediately = true)
        {
            bools.Remove(name);
            onBoolChanged.Remove(name);
            if (writeImmediately) WriteSettingsToFile();
        }

        public void RemoveNumberValue(string name, bool writeImmediately = true)
        {
            numbers.Remove(name);
            onNumberChanged.Remove(name);
            if (writeImmediately) WriteSettingsToFile();
        }

        public void RemoveStringValue(string name, bool writeImmediately = true)
        {
            strings.Remove(name);
            onStringChanged.Remove(name);
            if (writeImmediately) WriteSettingsToFile();
        }

        #endregion


        #region Propagation

        private CancellationTokenSource _writeTask;

        public void WriteSettingsToFile()
        {
            _writeTask?.Cancel();
            _writeTask?.Dispose();

            _writeTask = new();
            _ = WriteSettings(_writeTask);
        }

        private async Task WriteSettings(CancellationTokenSource token)
        {
            string path = Path.Combine(Application.persistentDataPath, settingsPath) + ".json";
            string json = JsonUtility.ToJson(this, true);

            try
            {
                if (token.IsCancellationRequested) return;
                await BasisFiles.WriteAllTextAsync(path, json, token.Token);
                if (token.IsCancellationRequested) return;
                OnSettingsWritten();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }

        }

        protected virtual void OnSettingsWritten()
        {
        }


        [AutoStaticsCleanup] private CancellationTokenSource _loadTask;

        public void LoadSettingsFromFile()
        {
            _loadTask?.Cancel();
            _loadTask?.Dispose();

            _loadTask = new();
            _ = LoadSettings(_loadTask);
        }

        private async Task LoadSettings(CancellationTokenSource token)
        {
            current ??= new();

            string path = Path.Combine(Application.persistentDataPath, current.settingsPath) + ".json";

            if (!File.Exists(path))
            {
                Debug.Log("Settings file not found.");
                current.WriteSettingsToFile();
                return;
            }

            try
            {
                if (token.IsCancellationRequested) return;
                string json = await BasisFiles.ReadAllTextAsync(path, token.Token);
                if (token.IsCancellationRequested) return;
                JsonUtility.FromJsonOverwrite(json, current);
                Debug.Log($"Loaded HandheldCameraSettings: {json}");

            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load settings: {ex.Message}");
                current = new();
                current.WriteSettingsToFile();
            }

            current.isLoaded = true;
            _onSettingsLoaded?.Invoke(current);
            OnSettingsLoaded();
        }

        protected virtual void OnSettingsLoaded()
        {
        }

        #endregion
    }
}
