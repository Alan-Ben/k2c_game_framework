using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民属性页面
    /// </summary>
    public class GGUIMonoMarsResidentAttribute : _AALBasicUIWndMono
    {
        [ALHeader("健康值进度条")]
        public GGUIMonoMarsResidentAttributeSlider monoHealthSlider;
        [ALHeader(" 幸福值进度条")]
        public GGUIMonoMarsResidentAttributeSlider monoHappinessSlider;
        
        [ALHeader("氧气值进度条")]
        public GGUIMonoMarsResidentAttributeSlider monoOxygenSlider;
        
        [ALHeader("饱腹值进度条")]
        public GGUIMonoMarsResidentAttributeSlider monoSatietySlider;
        
        [ALHeader("睡眠值进度条")]
        public GGUIMonoMarsResidentAttributeSlider monoSleepSlider;
        
        [ALHeader("舒适值")]
        public GGUIMonoMarsResidentAttributeSlider monoComfortSlider;
        
        [ALHeader("心情值")]
        public GGUIMonoMarsResidentAttributeSlider monoMoodSlider;
    }
}