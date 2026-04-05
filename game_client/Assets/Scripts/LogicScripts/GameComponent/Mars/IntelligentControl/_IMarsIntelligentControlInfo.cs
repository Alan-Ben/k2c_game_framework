namespace GOE
{
    public interface _IMarsIntelligentControlInfo
    {
        /// <summary>
        /// id
        /// </summary>
        long id { get; }
        
        /// <summary>
        /// 配表数据
        /// </summary>
        MarsIntelligentControlRefObj refObj { get; }
        
        /// <summary>
        /// 对应buff配表数据
        /// </summary>
        NPPlayerBuffRefObj buffRefObj { get; }
        
        /// <summary>
        /// 对应的buff服务端数据
        /// </summary>
        NPPlayerBuffInfo buffInfo { get; }
        
        /// <summary>
        /// 冷却结束时间(毫秒)
        /// </summary>
        long coolingEndTimeMs { get; }
        
        /// <summary>
        /// 冷却剩余时间(毫秒)
        /// </summary>
        long coolingLeftTimeMs { get; }

        /// <summary>
        /// 智能控制状态
        /// </summary>
        EMarsIntelligentControlState getState(bool _showTip = false);
    }
}