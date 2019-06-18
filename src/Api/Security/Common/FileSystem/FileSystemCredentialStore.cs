using System.IO;
using System.Linq;
using System.Collections.Generic;
using Spi.Credentials;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.FileSystem
{
    public class FileSystemCredentialStore : ICredentialStore
    {
        private ICredentialKeyFactory _credentialKeyFactory;
        private string _file = "FileSystemCredentialStore.sjon";


        public FileSystemCredentialStore(ICredentialKeyFactory credentialKeyFactory)
        {
            _credentialKeyFactory = credentialKeyFactory;
        }

        public async Task<bool> Delete(ICredentials credentials)
        {
            var key = await _credentialKeyFactory.GenerateKey(credentials);
            return await Delete(key);
        }


        public async Task<bool> Delete(ICredentialKey key)
        {
            var credentials = ReadAll();
            var key2 = await key.Key;
            return credentials.Remove(key2);
        }


        public async Task<ICredentials> Read(ICredentialKey key)
        {
            var allCredentials = ReadAll();
            var key2 = await key.Key;
            return allCredentials.Where(c => c.Key.Equals(key2)).Select(c => c.Value).FirstOrDefault();
        }

        public async Task<bool> Write(ICredentials credentials)
        {
            await Delete(credentials);

            var allCredentials = ReadAll();
            var key = await _credentialKeyFactory.GenerateKey(credentials);
            var key2 = await key.Key;
            allCredentials.Add(key2, credentials);
            WriteAll(allCredentials);
            return true;
        }

        private Dictionary<string, ICredentials> ReadAll()
        {
            if(!File.Exists(_file))
            {
                File.WriteAllText(_file, string.Empty);
            }
            var json = File.ReadAllText(_file);
            var items = JsonConvert.DeserializeObject<Dictionary<string, ICredentials>>(json);
            if(items == null)
            {
                items = new Dictionary<string, ICredentials>();
            }
            return items;
        }

        private void WriteAll(Dictionary<string, ICredentials> credentials)
        {
            var json = JsonConvert.SerializeObject(credentials, Formatting.Indented);
            File.WriteAllText(_file, json);
        }
    }
}