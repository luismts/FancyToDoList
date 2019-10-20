using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using FancyTodoList.Interfaces;

namespace FancyTodoList.Data
{
    public class CacheData : ICacheData
    {
        public CacheData()
        {
            //OJO: native cache data on pcl
            //App.Current.Properties["myInteger"] = 1337;
            //App.Current.SavePropertiesAsync(); // <===================== DID ALL

            BlobCache.ApplicationName = "TODOList";
        }

        #region Local
        public async Task<T> GetLocalObject<T>(string key)
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<T>(key);
            }
            catch (KeyNotFoundException)
            {
                return default(T);
            }
        }

        public async Task InsertLocalObject<T>(string key, T value)
        {
            await BlobCache.LocalMachine.InsertObject(key, value);
        }

        public async Task RemoveLocalObject(string key)
        {
            await BlobCache.LocalMachine.Invalidate(key);
        }
        #endregion
        
    }

    public static class CacheKeyDictionary
    {
        public const string LastCheck = "lastCheck";

        public const string ItemList = "itemList";
        public const string CategoryItems = "categoryItems";
    }
}
