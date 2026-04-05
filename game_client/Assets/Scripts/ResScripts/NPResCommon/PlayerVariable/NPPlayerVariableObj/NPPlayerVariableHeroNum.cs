using ALPackage;
using NPEnum;
using System;


namespace GOE
{
    /// <summary>
    /// 已拥有大臣数量（可配置等级上下限，无限制即-1） CS_HERO_NUM（@min_lv@max_lv）
    /// </summary>
    public class NPPlayerVariableHeroNum : _ANPBasicPlayerVariableObj
    {
        private WCGIntRange _m_rLvlRng;
        protected NPPlayerVariableHeroNum()
        {
        }

        /******************
       * 获取条件类型
       */
        public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_HERO_NUM; } }

        public override long calPlayerValue(NPVarInfo _variableInfo)
        {
#if NP_GAME
            return NPPlayer.instance.heroComponent.getOwnCount((_petInfo) =>
            {
                return _petInfo != null && (_m_rLvlRng == null || _m_rLvlRng.inRange(_petInfo.level));
            });
#else
	        return 0L;
#endif
        }

        public static NPPlayerVariableHeroNum readVariable(ALStringReader _reader)
        {
            try
            {
                NPPlayerVariableHeroNum variableObj = new NPPlayerVariableHeroNum();
                int min = -1;
                int max = -1;
                string minVS = _reader.readItem('@');
                string maxVS = _reader.readItem('@');
                if (null != minVS)
                    min = int.Parse(minVS);
                if (null != maxVS)
                    max = int.Parse(maxVS);

                variableObj._m_rLvlRng = new WCGIntRange(min, max);

                return variableObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("高级公式——已拥有大臣数量（可配置等级上下限，无限制即-1） CS_HERO_NUM（@min_lv@max_lv） Error Str: " + _reader.srcString);
                return null;
            }
        }
    }
}