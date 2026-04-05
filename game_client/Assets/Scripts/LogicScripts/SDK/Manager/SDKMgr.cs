using GOESDK;
using JetBrains.Annotations;

namespace GOE
{
    public class SDKMgr : _ASDKMgr
    {
        private static SDKMgr _g_instance;

        [NotNull]
        public static SDKMgr instance
        {
            get
            {
                if (_g_instance == null)
                {
                    _g_instance = new SDKMgr();
                }
                return _g_instance;
            }
        }
    }
}
