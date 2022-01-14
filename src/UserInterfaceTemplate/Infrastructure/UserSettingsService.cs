using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace UserInterfaceTemplate.Infrastructure
{
    public class UserSettingsService
    {
        private readonly string _folder;
        private readonly string _fileName = "Settings.json";

        private readonly byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

        private UserSettingsService() : this("") { }

        private UserSettingsService(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                var assemblyName = Assembly.GetExecutingAssembly().FullName;
                if (string.IsNullOrEmpty(assemblyName))
                {
                    appName = "MyApp";
                }
                else
                {
                    int i = assemblyName.IndexOf(".");
                    if (i < 0)
                        appName = assemblyName;
                    else
                    {
                        appName = assemblyName.Substring(0, i);
                    }
                }
            }
            _folder = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), appName);
        }

        public static UserSettingsService GetUserSettingsService(string appName, bool encrypted)
        {
            var service = new UserSettingsService(appName);
            service.Initialization = service.Initialize(encrypted);
            
            return service;
        }

        private Task Initialization { get; set; }

        private async Task Initialize(bool encrypted)
        {
            UserSettings = null;
            if (encrypted)
                await LoadSettingsEncrypted();
            else
                await LoadSettings();

            if (UserSettings == null)
            {
                UserSettings = new UserSettings();
                if (encrypted)
                    await WriteSettingsEncrypted();
                else
                    await WriteSettings();
            }
        }

        internal UserSettings UserSettings { get; private set; }


        /// <summary>
        /// Access the user's app settings folder
        /// </summary>
        internal string AppDataFolder
        {
            get
            {
                if (!Directory.Exists(_folder))
                    Directory.CreateDirectory(_folder);
                return _folder;
            }
        }

        internal async Task WriteSettings()
        {
            string fn = Path.Combine(AppDataFolder, _fileName);
            string json = JsonSerializer.Serialize(UserSettings);
            await using StreamWriter writer = new StreamWriter(fn);
            await writer.WriteAsync(json).ConfigureAwait(false);
        }

        internal async Task LoadSettings()
        {
            string fn = Path.Combine(AppDataFolder, _fileName);

            if (!File.Exists(fn))
                return;

            using StreamReader reader = new StreamReader(fn);
            string json = await reader.ReadToEndAsync().ConfigureAwait(false);
            UserSettings = JsonSerializer.Deserialize<UserSettings>(json);
        }

        internal Task WriteSettingsEncrypted()
        {
            return Task.Run(() =>
            {
                string json = JsonSerializer.Serialize(UserSettings);
                using FileStream myStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                using Aes aes = Aes.Create();
                aes.Key = key;
                byte[] iv = aes.IV;
                myStream.Write(iv, 0, iv.Length);
                using CryptoStream cryptStream = new CryptoStream(
                    myStream,
                    aes.CreateEncryptor(),
                    CryptoStreamMode.Write);
                using StreamWriter sWriter = new StreamWriter(cryptStream);
                sWriter.Write(json);
            });
        }

        internal async Task LoadSettingsEncrypted()
        {
            UserSettings = await Task.Run<UserSettings>(() =>
            {
                if (string.IsNullOrEmpty(_fileName) || !File.Exists(_fileName))
                    return null;

                using FileStream myStream = new FileStream(_fileName, FileMode.Open);
                using Aes aes = Aes.Create();
                byte[] iv = new byte[aes.IV.Length];
                myStream.Read(iv, 0, iv.Length);
                using CryptoStream cryptStream = new CryptoStream(
                    myStream,
                    aes.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                using StreamReader sReader = new StreamReader(cryptStream);
                string json = sReader.ReadToEnd();
                return JsonSerializer.Deserialize<UserSettings>(json);
            });

        }
    }
}
