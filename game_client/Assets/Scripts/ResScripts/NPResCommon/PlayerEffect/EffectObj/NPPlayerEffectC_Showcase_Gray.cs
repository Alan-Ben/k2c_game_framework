using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;
using UnityEngine;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    public class NPPlayerEffectC_Showcase_Gray : _ANPPlayerEffectInfo
    {
        private string _m_sShowcaseTempIndex;//舞台资源下标字符串
        private int _m_iIndex;//单位下标
        private bool _m_bIsGray;//是否置灰

        public NPPlayerEffectC_Showcase_Gray()
        {
            _m_sShowcaseTempIndex = string.Empty;
            _m_iIndex = 0;
            _m_bIsGray = false;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SHOWCASE_GRAY; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            string name = NPGShowcaseIndex.readIndexInfo(_m_sShowcaseTempIndex).objName;
            ShowcaseInfo showcaseInfo = ShowCaseMgr.instance.getShowCaseInfoByName(name);
            if (showcaseInfo == null)
            {
                Debug.LogError($"【NPPlayerEffectC_Showcase_Gray.DealPlayerEffect Error】:找不到舞台名称为{name}的可用showcase信息,_m_iIndex:{_m_iIndex},_m_bIsGray:{_m_bIsGray}");
                return;
            }

            showcaseInfo.darkMaterial(_m_iIndex, _m_bIsGray);
#endif
        }

        public static NPPlayerEffectC_Showcase_Gray readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 3)
            {
                Debug.LogError("配置错误 - C_Showcase_Gray   example: showcaseTempIndex:npcId Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Showcase_Gray effectObj = new NPPlayerEffectC_Showcase_Gray();

            try
            {
                effectObj._m_sShowcaseTempIndex = strs[0];
                effectObj._m_iIndex = int.Parse(strs[1]);
                effectObj._m_bIsGray = bool.Parse(strs[2]);

                return effectObj;
            }
            catch (Exception)
            {
                Debug.LogError("配置错误 - C_Showcase_Gray   example: showcaseTempIndex:npcId Error Str: " + _str);
                return null;
            }
        }
    }
}

