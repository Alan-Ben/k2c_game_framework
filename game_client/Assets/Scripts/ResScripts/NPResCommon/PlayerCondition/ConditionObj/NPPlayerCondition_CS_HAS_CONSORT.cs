using System;
using System.Collections.Generic;
using NPEnum;
using ALPackage;


namespace GOE
{
    /// <summary>
    /// 拥有指定大臣中的n个 CS_HAS_CONSORT:consort1&consort2:（n）
    /// </summary>
	public class NPPlayerCondition_CS_HAS_CONSORT : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_HAS_CONSORT; } }

	    private List<long> _m_lConsortIdList; //骑士id列表
        private int _m_needNum;//所需数量


		/// <summary>
		/// 读取条件信息
		/// </summary>
		/// <param name="_reader"></param>
		/// <returns></returns>
		public static NPPlayerCondition_CS_HAS_CONSORT readStr(ALStringReader _reader)
	    {
            NPPlayerCondition_CS_HAS_CONSORT cond = new NPPlayerCondition_CS_HAS_CONSORT();

            string rawConsortList = _reader.readItem();
            if (null == rawConsortList)
            {
                UnityEngine.Debug.LogError($"Can not read str for rawConsortList str:{_reader.srcString}");
                return null;
            }

            try
            {
                string[] consortIdList = rawConsortList.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                cond._m_lConsortIdList = new List<long>();
                for (int i = 0; i < consortIdList.Length; i++)
                {
                    long value = long.Parse(consortIdList[i]);
                    cond._m_lConsortIdList.Add(value);
                }

                //解析需要的数量
                string rawNeedNum = _reader.readItem(':');
                if (null != rawNeedNum)
                {
                    cond._m_needNum = int.Parse(rawNeedNum);
                }
                else
                {
                    cond._m_needNum = cond._m_lConsortIdList.Count;
                }
			}
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Can not read str for rawConsortList str:{_reader},{e}");
            }

            return cond;
		}
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            //计算拥有的妃子数量
            int hasConsortCount = 0;

            for (int i = 0; i < _m_lConsortIdList.Count; i++)
            {
                if (NPPlayer.instance.consortComp.getConsortUnlockType(_m_lConsortIdList[i]) == EGameCommonUnlockType.UNLOCK)
                    hasConsortCount++;
            }

            return hasConsortCount >= _m_needNum;
#else
	        return false;
#endif
        }
	}
}