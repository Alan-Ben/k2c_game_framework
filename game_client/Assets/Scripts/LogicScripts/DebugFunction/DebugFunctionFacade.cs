using JetBrains.Annotations;

namespace GOE
{
    public class DebugFunctionFacade
    {
        private static DebugFunctionFacade _g_instance;

        [NotNull]
        public static DebugFunctionFacade instance
        {
            get
            {
                if (_g_instance == null)
                {
                    _g_instance = new DebugFunctionFacade();
                }
                return _g_instance;
            }
        }

        [NotNull] public OpenDebugFunction openDebugFunction = new OpenDebugFunction("_openDebugFunction");
        [NotNull] public OpenDebugCDN openDebugCDN = new OpenDebugCDN("_openDebugCDN");
        [NotNull] public OpenDebugProtocol openDebugProtocol = new OpenDebugProtocol("_openDebugProtocol");
        [NotNull] public OpenDebugGPM openDebugGPM = new OpenDebugGPM("_openDebugGPM");
    }
}