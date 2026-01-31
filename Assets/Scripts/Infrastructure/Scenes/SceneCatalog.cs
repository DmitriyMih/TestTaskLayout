using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TestTaskLayout.Infrastructure.Scenes
{
    [CreateAssetMenu(menuName = "App/Scenes/Scene Catalog", fileName = "SceneCatalog")]
    public sealed class SceneCatalog : ScriptableObject
    {
        [Serializable]
        public sealed class Entry
        {
            [HorizontalGroup("Row", Width = 70)]
            [ToggleLeft, LabelText("On")]
            public bool Enabled = true;

            [HorizontalGroup("Row")]
            [Required, LabelWidth(40)]
            public string Key;

#if UNITY_EDITOR
            [HorizontalGroup("Row")]
            [Required, LabelWidth(55)]
            public SceneAsset Scene;
#endif

            [HideInInspector] public string SceneName;
            [HideInInspector] public string ScenePath;
        }

        [Title("Scenes")]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowItemCount = true)]
        [SerializeField]
        private List<Entry> scenes = new();

        public IEnumerable<string> GetEnabledKeys()
            => scenes
                .Where(x => x != null && x.Enabled && !string.IsNullOrWhiteSpace(x.Key))
                .Select(x => x.Key);

        public bool TryGetSceneName(string key, out string sceneName)
        {
            sceneName = null;
            if (string.IsNullOrWhiteSpace(key))
                return false;

            var entry = scenes.FirstOrDefault(x =>
                x != null &&
                x.Enabled &&
                string.Equals(x.Key, key, StringComparison.Ordinal));

            if (entry == null || string.IsNullOrWhiteSpace(entry.SceneName))
                return false;

            sceneName = entry.SceneName;
            return true;
        }

#if UNITY_EDITOR
        [Title("Tools")]
        [Button(ButtonSizes.Medium)]
        [InfoBox("Sync Build Settings from this catalog (add scenes and toggle Enabled).")]
        private void ApplyToBuildSettings()
        {
            RefreshDerivedData();

            var list = new List<EditorBuildSettingsScene>();
            foreach (var e in scenes)
            {
                if (e == null) continue;
                if (string.IsNullOrWhiteSpace(e.ScenePath)) continue;

                list.Add(new EditorBuildSettingsScene(e.ScenePath, e.Enabled));
            }

            EditorBuildSettings.scenes = list.ToArray();
            EditorUtility.SetDirty(this);
        }

        [Button(ButtonSizes.Small)]
        private void RefreshDerivedData()
        {
            foreach (var e in scenes)
            {
                if (e == null) continue;

                if (e.Scene == null)
                {
                    e.SceneName = null;
                    e.ScenePath = null;
                    continue;
                }

                e.SceneName = e.Scene.name;
                e.ScenePath = AssetDatabase.GetAssetPath(e.Scene);
            }
        }

        private void OnValidate()
        {
            RefreshDerivedData();
        }
#endif
    }
}