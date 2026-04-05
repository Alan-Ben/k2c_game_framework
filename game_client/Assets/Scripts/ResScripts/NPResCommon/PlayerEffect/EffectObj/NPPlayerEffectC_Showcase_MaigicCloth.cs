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
    public class NPPlayerEffectC_Showcase_MaigicCloth : _ANPPlayerEffectInfo
    {
        private string _m_sShowcaseTempIndex;//舞台资源下标字符串
        private int _m_iIndex;//单位下标
        private bool _m_bIsEnable;//是否生效

        public NPPlayerEffectC_Showcase_MaigicCloth()
        {
            _m_sShowcaseTempIndex = string.Empty;
            _m_iIndex = 0;
            _m_bIsEnable = false;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SHOWCASE_MAGICA_CLOTH; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            string name = NPGShowcaseIndex.readIndexInfo(_m_sShowcaseTempIndex).objName;
            ShowcaseInfo showcaseInfo = ShowCaseMgr.instance.getShowCaseInfoByName(name);
            if (showcaseInfo == null)
            {
                Debug.LogError($"【NPPlayerEffectC_Showcase_MaigicCloth.DealPlayerEffect Error】:找不到舞台名称为{name}的可用showcase信息");
                return;
            }

            showcaseInfo.enableMagicaCloth(_m_iIndex, _m_bIsEnable);
#endif
        }

        public static NPPlayerEffectC_Showcase_MaigicCloth readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 3)
            {
                Debug.LogError("配置错误 - C_SHOWCASE_MAGICA_CLOTH   example: showcaseTempIndex:npcId Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Showcase_MaigicCloth effectObj = new NPPlayerEffectC_Showcase_MaigicCloth();

            try
            {
                effectObj._m_sShowcaseTempIndex = strs[0];
                effectObj._m_iIndex = int.Parse(strs[1]);
                effectObj._m_bIsEnable = bool.Parse(strs[2]);

                return effectObj;
            }
            catch (Exception)
            {
                Debug.LogError("配置错误 - C_SHOWCASE_MAGICA_CLOTH   example: showcaseTempIndex:npcId Error Str: " + _str);
                return null;
            }
        }
    }
}

