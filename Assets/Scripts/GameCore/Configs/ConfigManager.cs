using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore.Configs
{
    [Serializable]
    public class Manifest
    {
        [Serializable]
        public class ConfigInfo
        {
            public string name;
            public int version;
        }

        public ConfigInfo[] configs;
    }

    public class ConfigManager : IGameManager
    {
        private const string ManifestAlias = "manifest";
        private const string ConfigExtension = ".json";

        private string _defaultConfigPath;

        private string _serverUrl;
        private JsonSerializerSettings _deserializeSettings;

        public ConfigManager(string defaultConfigPath)
        {
            _defaultConfigPath = defaultConfigPath;
            _deserializeSettings = new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Populate,
            };
        }

        public T TryGetConfig<T>(string configName, JsonSerializerSettings settings = null) where T : class
        {
            var text = GetLocalConfig(configName);
            if (!string.IsNullOrEmpty(text))
            {
                return JsonConvert.DeserializeObject<T>(text, _deserializeSettings == null ? _deserializeSettings : settings);
            }
            
            return null;
        }

        public async Task Init()
        {
            var defaultConfig = await ResourceLoadTask<DefaultConfig>.LoadAsync(_defaultConfigPath);
            _serverUrl = defaultConfig.remoteConfigAddress;
#if USE_REMOTE_CONFIG
            var remoteManifestSource = GetURLContent(ManifestAlias);
            var remoteManifest = JsonConvert.DeserializeObject<Manifest>(remoteManifestSource,_deserializeSettings);
             var localManifest = GetLocalManifest();
#else
            string remoteManifestSource = null;
            Manifest remoteManifest = null;
            Manifest localManifest = null;
#endif

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (remoteManifest != null)
            {
                SetLocalConfig(ManifestAlias, remoteManifestSource);
                CheckConfigs(localManifest, remoteManifest);
            }
            else if (localManifest == null)
            {
                SetLocalConfig(ManifestAlias, defaultConfig.manifest.text);
                foreach (var config in defaultConfig.defaultConfigs)
                {
                    SetLocalConfig(config.name, config.text);
                }
            }
        }

        void CheckConfigs(Manifest localManifest, Manifest remoteManifest)
        {
            foreach (var config in remoteManifest.configs)
            {
                if (localManifest == null)
                {
                    UpdateLocalConfig(config.name);
                    continue;
                }

                var localConfig = localManifest.configs.FirstOrDefault(x => x.name == config.name);
                if (localConfig == null || localConfig.version < config.version)
                {
                    UpdateLocalConfig(config.name);
                }
            }
        }

        void UpdateLocalConfig(string configName)
        {
            var content = GetURLContent(configName);
            SetLocalConfig(configName, content);
        }

        private Manifest GetLocalManifest()
        {
            var manifestData = GetLocalConfig(ManifestAlias);
            if (string.IsNullOrEmpty(manifestData))
            {
                return null;
            }

            var manifest = JsonConvert.DeserializeObject<Manifest>(manifestData, _deserializeSettings);
            return manifest;
        }

        private void SetLocalConfig(string configName, string content)
        {
            File.WriteAllText(Path.Combine(Application.persistentDataPath, configName + ConfigExtension), content);
        }

        private string GetLocalConfig(string configName)
        {
            var path = Path.Combine(Application.persistentDataPath, configName + ConfigExtension);
            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllText(path);
        }

        private string GetURLContent(string uri)
        {
            var fullUri = Path.Combine(_serverUrl, uri + ConfigExtension);
            try
            {
                var request = WebRequest.Create(fullUri);
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            return string.Empty;
                        }

                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("[ConfigManager] GetURLContent Exception. Message : " + ex.Message);
                return string.Empty;
            }
        }
    }
}