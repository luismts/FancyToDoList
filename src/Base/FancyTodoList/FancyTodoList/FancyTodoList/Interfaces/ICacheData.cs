using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace FancyTodoList.Interfaces
{
    /// <summary>
    /// There are four build-in locations, that have some magic applied on some systems.
    /// 
    /// Xamarin.iOS may remove data, stored in BlobCache.LocalMachine, to free up disk space (only if your app is not running). 
    /// The locations BlobCache.UserAccount and BlobCache.Secure will be backed up to iCloud and iTunes. 
    /// 
    /// Xamarin.Android may also start deleting data, stored in BlobCache.LocalMachine, if the system runs out of disk space. 
    /// It isn't clearly specified if your app could be running while the system is cleaning this up. 
    /// 
    /// Windows 10 (UWP) will replicate BlobCache.UserAccount and BlobCache.Secure to the cloud and synchronize it to all user 
    /// devices on which the app is installed
    /// </summary>
    public interface ICacheData : ILocalMachineCache
    {

    }




    /// <summary>
    /// Cached data. This data may get deleted without notification.
    /// </summary>
    public interface ILocalMachineCache
    {
        Task<T> GetLocalObject<T>(string key);

        Task InsertLocalObject<T>(string key, T value);

        Task RemoveLocalObject(string key);
    }

    /// <summary>
    /// User settings. Some systems backup this data to the cloud.
    /// </summary>
    public interface IUserAccountCache
    {
        Task<T> GetUserAccountObject<T>(string key);

        Task InsertUserAccountObject<T>(string key, T value);

        Task RemoveUserAccountObject(string key);
    }

    /// <summary>
    /// For saving sensitive data - like credentials.
    /// </summary>
    public interface ISecureCache
    {
        Task<T> GetSecureObject<T>(string key);

        Task InsertSecureObject<T>(string key, T value);

        Task RemoveSecureObject(string key);
    }

    /// <summary>
    /// A database, kept in memory. The data is stored for the lifetime of the app.
    /// </summary>
    public interface IInMemoryCache
    {
        Task<T> GetInMemoryObject<T>(string key);

        Task InsertInMemoryObject<T>(string key, T value);

        Task RemoveInMemoryObject(string key);
    }

    /// <summary>
    /// Downloading and caching URLs and Images
    /// </summary>
    interface IDownloadingCaching
    {
        /// <summary>
        /// Download a file as a byte array
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="fetchAlways"></param>
        /// <param name="absoluteExpiration"></param>
        /// <returns></returns>
        IObservable<byte[]> DownloadUrl(string url, IDictionary<string, string> headers = null, bool fetchAlways = false, DateTimeOffset? absoluteExpiration = null);

        /// <summary>
        ///  Load a given key as an image
        /// </summary>
        /// <param name="key"></param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        /// <returns></returns>
        IObservable<IBitmap> LoadImage(string key, float? desiredWidth = null, float? desiredHeight = null);

        /// <summary>
        /// Download an image from the network and load it
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fetchAlways"></param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        /// <param name="absoluteExpiration"></param>
        /// <returns></returns>
        IObservable<IBitmap> LoadImageFromUrl(string url, bool fetchAlways = false, float? desiredWidth = null, float? desiredHeight = null, DateTimeOffset? absoluteExpiration = null);

    }
}
