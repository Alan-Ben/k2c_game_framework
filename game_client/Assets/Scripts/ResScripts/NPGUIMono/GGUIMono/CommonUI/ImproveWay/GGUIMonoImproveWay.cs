using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 提升途径类型
    /// </summary>
    public enum EImproveWayType
    {
        NONE,
        [InspectorName("RECRUIT_EMPLOYEES（招募员工）")]
        RECRUIT_EMPLOYEES,
        [InspectorName("TRAIN_HERO（培养顾问）")]
        TRAIN_HERO,
        [InspectorName("USE_ITEM（使用道具）")]
        USE_ITEM,
        [InspectorName("TRAIN_CHILD（培养学员）")]
        TRAIN_CHILD,
        [InspectorName("TRAIN_CONSORT_SKILL（培养情人技能）")]
        TRAIN_CONSORT_SKILL,
        [InspectorName("TRAIN_CONSORT_BLESS（培养情人加护）")]
        TRAIN_CONSORT_BLESS,
    }

    /// <summary>
    /// 提升目标类型
    /// </summary>
    public enum EImproveTargetType
    {
        NONE,
        [InspectorName("EARNING（提升收益）")]
        EARNING,
        [InspectorName("HERO_POWER（提升顾问实力）")]
        HERO_POWER,
        [InspectorName("EMPLOYEES_NUM（提升员工数量）")]
        EMPLOYEES_NUM,
    }

    /// <summary>
    /// 提升目标类型配置参数
    /// </summary>
    [System.Serializable]
    public class GGUIMonoImproveWayTargetTypeParam
    {
        [ALHeader("提升目标类型")]
        public EImproveTargetType type;
        [ALHeader("需要展示的提升途径类型列表")]
        public List<EImproveWayType> showImproveWayList;
        [ALHeader("提升提示文本key")]
        public string improveTipDescKey;
        [ALHeader("使用道具途径描述Key")]
        public string useItemWayDescKey;
        [ALHeader("前往使用道具效果")]
        public string useItemGoToEffect;
    }

    /// <summary>
    /// 提升途径access表配置参数
    /// </summary>
    [System.Serializable]
    public class GGUIMonoImproveWayAccessParam
    {
        [ALHeader("提升途径类型")]
        public EImproveWayType type;
        [ALHeader("access表id")]
        public long accessId;
    }

    /// <summary>
    /// 提升途径弹窗
    /// </summary>
    public class GGUIMonoImproveWay : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("item 的父对象")]
        public Transform goItemParent;
        [ALHeader("提升提示文本")]
        public Text txtImproveTipDesc;

        [ALHeader("获取途径item")]
        public NPGGUIMonoAccessWayItem monoAccessWay;
        [ALHeader("使用道具途径item")]
        public GGUIMonoImproveWayUseItem monoUseItem;

        [ALHeader("不同提升目标类型配置参数")]
        public List<GGUIMonoImproveWayTargetTypeParam> targetTypeParmList;
        [ALHeader("不同提升目标类型access表配置参数列表")]
        public List<GGUIMonoImproveWayAccessParam> accessParamList;

        [ALInfo("====排序相关参数====")]
        [ALHeader("招募员工-标记1-可招募员工数量最小值")]
        public int recruitEmployeesMinNum_First = 200;
        [ALHeader("招募员工-标记2-可招募员工数量最小值")]
        public int recruitEmployeesMinNum_Second = 1;
        [ALHeader("培养顾问-标记1-顾问提升等级最小值")]
        public int trainHeroUpgradeLevel_First = 100;
        [ALHeader("培养顾问-标记2-顾问提升等级最小值")]
        public int trainHeroUpgradeLevel_Second = 1;
        [ALHeader("培养学员-标记1-累积剩余培养次数最小值")]
        public int trainChildLeftCount_First = 100;
        [ALHeader("培养学员-标记2-累积剩余培养次数最小值")]
        public int trainChildLeftCount_Second = 1;

        /// <summary>
        /// 获取目标类型配置参数
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public GGUIMonoImproveWayTargetTypeParam getImproveTargetTypeParam(EImproveTargetType _type)
        {
            if (targetTypeParmList == null)
                return null;

            foreach (GGUIMonoImproveWayTargetTypeParam item in targetTypeParmList)
            {
                if (item != null && item.type == _type)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 获取提升途径accessId
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getAccessId(EImproveWayType _type)
        {
            if (accessParamList == null)
                return 0;

            foreach (GGUIMonoImproveWayAccessParam item in accessParamList)
            {
                if (item != null && item.type == _type)
                    return item.accessId;
            }

            return 0;
        }

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(56); } }
        public static string objName { get { return UIResPathAssistant.getObjName(56); } }
    }
}
