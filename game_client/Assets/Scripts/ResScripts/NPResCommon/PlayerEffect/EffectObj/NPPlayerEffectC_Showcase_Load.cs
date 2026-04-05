using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

using UnityEngine;


namespace GOE
{
    public class NPPlayerEffectC_Showcase_Load : _ANPPlayerEffectInfo
    {
        private string _m_sShowcaseTempIndex;//舞台资源下标字符串
        private int _m_iIndex;//替换的单位下标
        private long _m_lNpcId;//npc配置id
        private float _m_setLocalRotationY = 0;//localY

        public NPPlayerEffectC_Showcase_Load()
        {
            _m_sShowcaseTempIndex = string.Empty;
            _m_iIndex = 0;
            _m_lNpcId = 0;
            _m_setLocalRotationY = 0;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SHOWCASE_LOAD; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            string name = NPGShowcaseIndex.readIndexInfo(_m_sShowcaseTempIndex).objName;
            ShowcaseInfo showcaseInfo = ShowCaseMgr.instance.getShowCaseInfoByName(name);
            if (showcaseInfo == null)
            {
                UnityEngine.Debug.LogError($"【NPPlayerEffectC_Showcase_Load.DealPlayerEffect Error】:找不到舞台名称为{name}的可用showcase信息,_m_iIndex:{_m_iIndex},_m_lNpcId:{_m_lNpcId}");
                return;
            }

            if (_m_lNpcId <= 0)
            {
                showcaseInfo.removeLoadUnit(_m_iIndex);
            }
            else
            {
                NPNPCRefObj npcRef = GRefdataCoreMgr.instance.npcRefCore.getRef(_m_lNpcId);
                if (npcRef == null)
                    return;

                _AShowCaseUnitInfoObj unitInfoObj = npcRef.toUnitInfoObj();
                if(null == unitInfoObj)
                    return;
                
                showcaseInfo.additionLoadUnit(unitInfoObj, _m_iIndex, () =>
                {
                    unitInfoObj.setLocalPosition(npcRef.showCaseOffset);
                    if (_m_setLocalRotationY != 0)
                    {
                        Vector3 rotation = unitInfoObj.localEulerAngles;
                        rotation.y = _m_setLocalRotationY;
                        unitInfoObj.setLocalEulerAngles(rotation);
                    }
                });
            }
#endif
        }

        public static NPPlayerEffectC_Showcase_Load readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 3)
            {
                UnityEngine.Debug.LogError("配置错误 - C_Showcase_Load   example: showcaseTempIndex:npcId Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Showcase_Load effectObj = new NPPlayerEffectC_Showcase_Load();

            try
            {
                effectObj._m_sShowcaseTempIndex = strs[0];
                effectObj._m_iIndex = int.Parse(strs[1]);
                effectObj._m_lNpcId = long.Parse(strs[2]);
                
                if(strs.Length > 3)
                    effectObj._m_setLocalRotationY = ALCommon.ParseFloat(strs[3]);

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_Showcase_Load   example: showcaseTempIndex:npcId Error Str: " + _str);
                return null;
            }
        }
    }
}

