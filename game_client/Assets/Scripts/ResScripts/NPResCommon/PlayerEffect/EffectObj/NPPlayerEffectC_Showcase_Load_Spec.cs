using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

#if NP_GAME
using GOE;
using UnityEngine;
#endif

namespace GOE
{
    public class NPPlayerEffectC_Showcase_Load_Spec : _ANPPlayerEffectInfo
    {
        private string _m_sShowcaseTempIndex;//舞台资源下标字符串
        private int _m_iIndex;//替换的单位下标
        private EShowcaseLoadType _m_eType;//类型
        private List<string> _m_lArgs;//参数列表

        public NPPlayerEffectC_Showcase_Load_Spec()
        {
            _m_sShowcaseTempIndex = string.Empty;
            _m_iIndex = 0;
            _m_eType = EShowcaseLoadType.NONE;
            _m_lArgs = new List<string>();
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SHOWCASE_LOAD_SPEC; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            string name = NPGShowcaseIndex.readIndexInfo(_m_sShowcaseTempIndex).objName;
            ShowcaseInfo showcaseInfo = ShowCaseMgr.instance.getShowCaseInfoByName(name);
            if (showcaseInfo == null)
            {
                UnityEngine.Debug.LogError($"【NPPlayerEffectC_Showcase_Load_Spec.DealPlayerEffect Error】:找不到舞台名称为{name}的可用showcase信息");
                return;
            }

            switch (_m_eType)
            {
                case EShowcaseLoadType.NONE:
                    showcaseInfo.removeLoadUnit(_m_iIndex);
                    return;
                case EShowcaseLoadType.TRAVEL:
                    _ATravelEventInfo travelEventInfo = NPPlayer.instance.travelComp.curDealEvent;
                    _ITravelEventRole travelEventRole = travelEventInfo?.travelEventRefObj?.eventTarget.roleInfo;
                    if (travelEventRole == null || travelEventRole.showCaseUnitInfo == null)
                        return;
                    
                    showcaseInfo.additionLoadUnit(travelEventRole.showCaseUnitInfo, _m_iIndex, () =>
                    {
                        travelEventRole.showCaseUnitInfo.setLocalPosition(travelEventRole.showCaseOffset);
                    });
                    return;
                case EShowcaseLoadType.TRAVEL_BG:
                    TravelPosRefObj travelPosRefObj = NPPlayer.instance.travelComp.curDealEvent?.travelPosRefObj;
                    if(travelPosRefObj == null || travelPosRefObj.dialogue_bg_index == null || !travelPosRefObj.dialogue_bg_index.isValid())
                        return;
                    
                    _AShowCaseUnitInfoObj travelBgUnitInfoObj = new ShowCaseCommonResUnitInfoObj(travelPosRefObj.dialogue_bg_index);
                    showcaseInfo.additionLoadUnit(travelBgUnitInfoObj, _m_iIndex, null);
                    return;
            }
#endif
        }

        public static NPPlayerEffectC_Showcase_Load_Spec readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 3)
            {
                UnityEngine.Debug.LogError("配置错误 - C_Showcase_Load   example: NPGShowcaseIndex:单位下标:EShowcaseLoadType Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Showcase_Load_Spec effectObj = new NPPlayerEffectC_Showcase_Load_Spec();

            try
            {
                effectObj._m_sShowcaseTempIndex = strs[0];
                effectObj._m_iIndex = int.Parse(strs[1]);
                ALCommon.TryEnumParse(typeof(EShowcaseLoadType),strs[2],out effectObj._m_eType);

                if (effectObj._m_lArgs == null)
                    effectObj._m_lArgs = new List<string>();
                effectObj._m_lArgs.Clear();
                for(int i = 3; i < strs.Length; i++)
                {
                    effectObj._m_lArgs.Add(strs[i]);
                }

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_Showcase_Load   example: NPGShowcaseIndex:单位下标:EShowcaseLoadType Error Str: " + _str);
                return null;
            }
        }
    }
}

