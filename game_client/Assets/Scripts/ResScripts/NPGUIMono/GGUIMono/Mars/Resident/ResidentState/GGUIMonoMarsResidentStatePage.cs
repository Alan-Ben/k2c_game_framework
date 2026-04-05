using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民状态页面
    /// </summary>
    public class GGUIMonoMarsResidentStatePage : _AALBasicUIWndMono
    {
        [ALHeader("可容纳人数(当前居民总数/人口数上限)")]
        public TextEx txtGalleryful; // 可容纳数量: {0}/{1}

        // 当前居民总数<人口数上限，出现补充居民按钮，点击前往移民飞船港口建筑坐标，进行补充居民相关操作
        [ALHeader("补充居民按钮")]
        public GameObject btnReplenish;
        [ALHeader("补充居民按钮点击效果")]
        public _NPPlayerEffectSerializeInfo btnReplenishClickEffect;
        
        // 当前居民总数>=人口数上限，出现提升居民上限按钮，点击前往居住舱，后按优先级，前往修建指定的居住舱
        [ALHeader("前往建造建筑按钮")]
        public GameObject btnGoToBuildBuilding;

        [ALHeader("工作中人数")]
        public TextEx txtWorkingNum;
        [ALHeader("生病中人数")]
        public TextEx txtSickNum;
        [ALHeader("休息中人数")]
        public TextEx txtRestNum;

        [ALHeader("居民派遣Item列表")]
        public GGUIMonoMarsResidentDispatchItemGrid monoDispatchItemGrid;
        
        [ALHeader("可派遣人数")]
        public TextEx canDispatchNum; // 可派遣数量: {0}/{1}
    }
}