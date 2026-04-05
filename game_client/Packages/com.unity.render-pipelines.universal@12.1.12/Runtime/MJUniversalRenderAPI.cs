namespace UnityEngine.Rendering.Universal
{
    public enum UIBlurState
    {
        /// <summary>
        /// 默认状态无模糊
        /// </summary>
        Default = 0,
        /// <summary>
        /// 静帧模糊，状态改为StaticBlur后，开启静帧模糊，模糊完，状态会自动改为AfterStaticBlur，要改为不模糊则改为Default
        /// </summary>
        StaticBlur = 1,
        /// <summary>
        /// 静帧模糊完保持模糊结果的状态
        /// </summary>
        AfterStaticBlur = 2,
        /// <summary>
        /// 实时模糊
        /// </summary>
        RealTimeBlur = 3
    }
    public static class MJUniversalRenderAPI
    {
        public static UIBlurState uiBlurState = UIBlurState.Default;
        public static RenderTexture uiBlurRenderTexture;
        static void Quit()
        {
            uiBlurState = UIBlurState.Default;
        }

        [RuntimeInitializeOnLoadMethod]
        static void RunOnStart()
        {
            Application.quitting += Quit;
        }
    }
}