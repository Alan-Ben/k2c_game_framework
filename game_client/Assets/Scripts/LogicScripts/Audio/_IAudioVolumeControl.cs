namespace GOE
{
    /// <summary>
    /// 音量大小控制接口
    /// </summary>
    public interface _IAudioVolumeControl
    {
        //TODO 背景音乐，音效音乐这种先写死了，在加音效类型可以考虑改成枚举当作参数来传
        
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        bool isInitDone { get; }
        
        /// <summary>
        /// 背景音源本身应该设置的音量的大小
        /// </summary>
        float audioSourceBgVolume { get; }
        
        /// <summary>
        /// 音效音源本身应该设置的音量的大小
        /// </summary>
        float audioSourceVolume { get; }
        
        /// <summary>
        /// 配音音源本身应该设置的音量的大小
        /// </summary>
        float voiceSourceVolume { get; }

        /// <summary>
        /// 设置背景音量大小
        /// </summary>
        /// <param name="_isOn">是否打开</param>
        /// <param name="_value">大小 0~1</param>
        void setBgAudioVolume(bool _isOn, float _value);
        
        /// <summary>
        /// 设置音效音量大小
        /// </summary>
        /// <param name="_isOn">是否打开</param>
        /// <param name="_value">大小 0~1</param>
        void setAudioVolume(bool _isOn, float _value);
        
        /// <summary>
        /// 设置配音音量大小
        /// </summary>
        /// <param name="_isOn">是否打开</param>
        /// <param name="_value">大小 0~1</param>
        void setVoiceVolume(bool _isOn, float _value);
    }
}