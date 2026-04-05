using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 国力变化上浮提示
    /// </summary>
    public class GGUIMonoNationPowerTip : _AALBasicUIWndMono
    {
        [ALHeader("国力")]
        public Text txtNationPower;
        [ALHeader("值的key(一个参数, 增加的赚速值(带有大数缩写))")]
        public string valueKey;
        [ALHeader("增加的值")]
        public Text txtAddNum;
        [ALHeader("减少的值")]
        public Text txtReduceNum;
        [ALHeader("无效的时间间隔")]
        public float disableTime = 2f;
        [ALHeader("数值变化使用时间")]
        public float value_lerp_time = 0.5f;

        [ALHeader("数值增加时需要展示的GO列表")]
        public List<GameObject> goAddShowList;
        [ALHeader("数值减少时需要展示的GO列表")]
        public List<GameObject> goReduceShowList;

        [ALHeader("提升音效ID")]
        public long audioResId;
        [ALHeader("最大同时播放音效数量")]
        public long maxAudioCount = 5;
        
        [ALHeader("数字变动动画")]
        public Animation anim;
        [ALHeader("数字变动动画名")]
        public string numChgAnimName;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2806); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2806); } }
    }
}
