
#if NP_GAME
using ALPackage;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class NPGGUICommonConditionChgColorInfo
    {
        [ALHeader("条件字符串")]
        public string conditionStr;
        [ALHeader("颜色")]
        public Color normalColor = Color.black;
        [ALHeader("颜色载体")]
        public Graphic graphic;
        
        private bool _m_bIsInited = false;
        private NPPlayerConditionGroupObj _m_cgConditionGroupObj;
        public NPPlayerConditionGroupObj conditionGroup
        {
            get
            {
                if (_m_bIsInited)
                    return _m_cgConditionGroupObj;

                _m_cgConditionGroupObj = NPPlayerConditionGroupObj.readConditionGroupList(conditionStr, "condPref");
                _m_bIsInited = true;
                return _m_cgConditionGroupObj;
            }
        }
        
        /// <summary>
        /// 刷新颜色
        /// </summary>
        public void refreshColor()
        {
#if NP_GAME
            if (conditionGroup.IsEnable(null))
            {
                ALUGUICommon.setUIObjColor(graphic,normalColor);
            }
#endif
        }
    }
}