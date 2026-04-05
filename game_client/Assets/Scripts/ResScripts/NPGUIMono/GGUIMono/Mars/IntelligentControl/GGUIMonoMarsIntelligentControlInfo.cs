using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 智能控制决策信息
    /// </summary>
    public class GGUIMonoMarsIntelligentControlInfo : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage icon;
        
        [ALHeader("指令名")]
        public TextEx txtName;

        [ALHeader("效果描述")]
        public TextEx txtEffectDesc;
        [ALHeader("效果描述Key(一个参数, 效果描述)")]
        public string txtEffectDescKey;
        
        [ALHeader("生效中时间倒计时")]
        public TextEx txtEffectiveTimeCountDown;
        [ALHeader("生效中时间倒计时key(一个参数, 生效中时间倒计时)")]
        public string txtEffectiveTimeCountDownKey;
        
        [ALHeader("冷却时间倒计时")]
        public TextEx txtCoolingTimeCountDown;
        [ALHeader("冷却时间倒计时Key(一个参数, 冷却时间倒计时)")]
        public string txtCoolingTimeCountDownKey;

        [ALHeader("冷却时长")]
        public TextEx txtCoolingTime;
        [ALHeader("冷却时长Key(一个参数, 冷却时长)")]
        public string txtCoolingTimeKey;

        [ALHeader("不同状态下的显示信息")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsIntelligentControlState>> stateShow;
    }
}