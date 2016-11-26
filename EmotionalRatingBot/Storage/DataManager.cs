namespace EmotionalRatingBot.Storage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.UI.WebControls;

    using EmotionalRatingBot.ImageCore;

    public class DataManager : ReaderWriterLockSlim
    {
        private static DataManager manager;

        private static Object mux = new Object();

        private long _version;

        private DataManager()
        {
        }

        public static DataManager GetInstance()
        {
            if (manager == null)
            {
                lock (mux)
                {
                    if (manager == null) manager = new DataManager();
                }
            }
            return manager;
        }

        public void Update()
        {
            this.EnterWriteLock();
            this._version++;
            this.ExitWriteLock();
        }

        public long GetVersion()
        {
            this.EnterReadLock();
            var version = this._version;
            this.ExitReadLock();
            return version;
        }
    }
}