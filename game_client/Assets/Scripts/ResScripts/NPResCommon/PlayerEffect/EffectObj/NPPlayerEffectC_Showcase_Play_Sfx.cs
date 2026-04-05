using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    public class NPPlayerEffectC_Showcase_Play_Sfx : _ANPPlayerEffectInfo
    {
        private string _m_sShowcaseTempIndex;//舞台资源下标字符串
        private int _m_iIndex;//替换的单位下标
        private long _m_lSfxId;//sfx配置id

        public NPPlayerEffectC_Showcase_Play_Sfx()
        {
            _m_sShowcaseTempIndex = string.Empty;
            _m_iIndex = 0;
            _m_lSfxId = 0;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SHOWCASE_PLAY_SFX; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            string name = NPGShowcaseIndex.readIndexInfo(_m_sShowcaseTempIndex).objName;
            ShowcaseInfo showcaseInfo = ShowCaseMgr.instance.getShowCaseInfoByName(name);
            if (showcaseInfo == null)
            {
                UnityEngine.Debug.LogError($"【NPPlayerEffectC_SHOWCASE_PLAY_SFX.DealPlayerEffect Error】:找不到舞台名称为{name}的可用showcase信息");
                return;
            }

            _AShowCaseUnitInfoObj showCaseUnitInfoObj = showcaseInfo.getUnitInfo(_m_iIndex);
            if (null == showCaseUnitInfoObj)
            {
                UnityEngine.Debug.LogError($"【NPPlayerEffectC_SHOWCASE_PLAY_SFX.DealPlayerEffect Error】:找不到位置为{_m_iIndex}的可用单位信息");
                return;
            }
            
            showCaseUnitInfoObj.playSfx(_m_lSfxId);
#endif
        }

        public static NPPlayerEffectC_Showcase_Play_Sfx readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 3)
            {
                UnityEngine.Debug.LogError("配置错误 - C_SHOWCASE_PLAY_SFX   example: showcaseTempIndex:sfxID Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Showcase_Play_Sfx effectObj = new NPPlayerEffectC_Showcase_Play_Sfx();

            try
            {
                effectObj._m_sShowcaseTempIndex = strs[0];
                effectObj._m_iIndex = int.Parse(strs[1]);
                effectObj._m_lSfxId = long.Parse(strs[2]);

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_SHOWCASE_PLAY_SFX   example: showcaseTempIndex:sfxID Error Str: " + _str);
                return null;
            }
        }
    }
}

