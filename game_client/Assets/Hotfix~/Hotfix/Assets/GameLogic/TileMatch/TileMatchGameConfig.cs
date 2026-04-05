namespace Hotfix
{
    public class TileMatchGameConfig
    {
        public const int defaultErrorSwitchShowTipCount = 5;
        public const float defaultIdleToTipTime = 10f;
        public const float defaultErrorSwitchResetDelayTime = 0.1f;//错误交换后重置的延迟时间
        
        public TileMatchGameConfig()
        {
            errorSwitchShowTipCount = defaultErrorSwitchShowTipCount;
            idleToTipTime = defaultIdleToTipTime;
            errorSwitchResetDelayTime = defaultErrorSwitchResetDelayTime;
        }

        /// <summary>
        /// 错误交换限制次数触发提示
        /// </summary>
        public int errorSwitchShowTipCount { get; set; }

        /// <summary>
        /// 未操作时, 多久显示提示
        /// </summary>
        public float idleToTipTime { get; set; }
        
        public float errorSwitchResetDelayTime { get; set; }
        
        /// <summary>
        /// 交换格子的音效ID
        /// </summary>
        public long exchangeBlockAudioId { get; set; }
    }
}