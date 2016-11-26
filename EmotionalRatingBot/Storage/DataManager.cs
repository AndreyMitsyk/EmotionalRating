namespace EmotionalRatingBot.Storage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.UI.WebControls;

    using EmotionalRatingBot.ImageCore;

    public class DataManager
    {
        private static DataManager manager;

        private static Object mux = new Object();

        private long _version;

        private DataManager()
        {
        }

        public static DataManager getInstance()
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
            this._version++;
        }

        public Task<long> IsUpdated(long version)
        {
            Task<long> t = Task.Run(
                () =>
                    {
                        while (version >= this._version)
                        {
                        }
                        return this._version;
                    });
            return t;
        }
    }
}