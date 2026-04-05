using System;
using System.Collections.Generic;
using NPEnum;
using ALPackage;


namespace GOE
{
    /// <summary>
    /// 拥有指定大臣中的n个 CS_HAS_HERO:hero1&hero2:（n）
    /// </summary>
	public class NPPlayerCondition_CS_HAS_HERO : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_HAS_HERO; } }

	    private List<long> _m_lHeroIdList; //骑士id列表
        private int _m_needNum;//所需数量


		/// <summary>
		/// 读取条件信息
		/// </summary>
		/// <param name="_reader"></param>
		/// <returns></returns>
		public static NPPlayerCondition_CS_HAS_HERO readStr(ALStringReader _reader)
	    {
            NPPlayerCondition_CS_HAS_HERO cond = new NPPlayerCondition_CS_HAS_HERO();

            string rawHeroList = _reader.readItem();
            if (null == rawHeroList)
            {
                UnityEngine.Debug.LogError($"Can not read str for rawHeroList str:{_reader.srcString}");
                return null;
            }

            try
            {
                string[] heroIdList = rawHeroList.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                cond._m_lHeroIdList = new List<long>();
                for (int i = 0; i < heroIdList.Length; i++)
                {
                    long value = long.Parse(heroIdList[i]);
                    cond._m_lHeroIdList.Add(value);
                }

                //解析需要的数量
                string rawNeedNum = _reader.readItem(':');
                if (null != rawNeedNum)
                {
                    cond._m_needNum = int.Parse(rawNeedNum);
                }
                else
                {
                    cond._m_needNum = cond._m_lHeroIdList.Count;
                }
			}
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Can not read str for rawHeroList str:{_reader},{e}");
            }

            return cond;
		}
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            //计算拥有的大臣数量
            int hasHeroCount = 0;

            for (int i = 0; i < _m_lHeroIdList.Count; i++)
            {
                if (NPPlayer.instance.heroComponent.isHeroUnlock(_m_lHeroIdList[i]))
                    hasHeroCount++;
            }

            return hasHeroCount >= _m_needNum;
#else
	        return false;
#endif
        }
	}
}