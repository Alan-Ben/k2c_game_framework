namespace GOE
{
    public interface _ICameraMonitor
    {
        // 当进入和退出时的操作处理
        void onEnter();
        void onExit();
        
        // 摄像头位置变更处理
        void onCameraPosChg();
        void onCameraFocusChg();
        void onCameraTransformChg();
        // 缩放变更的处理
        void onOrthographicSizeChg();
        void onFieldOfViewChg();
        void onViewScaleChg();

        // 当限制器变更时的处理
        void onPosLimiterChg();
        void onFocusPosLimiterChg();
    }
}