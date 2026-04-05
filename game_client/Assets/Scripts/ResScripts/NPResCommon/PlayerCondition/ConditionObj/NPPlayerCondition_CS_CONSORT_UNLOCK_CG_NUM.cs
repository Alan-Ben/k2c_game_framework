using System;
using NPEnum;
using GOE;
using ALPackage;


namespace GOE
{
	/// <summary>
	/// 解锁妃子CG的数量 CS_CONSORT_UNLOCK_CG_NUM:min_value（:max_value）
	/// </summary>
	public class NPPlayerCondition_CS_CONSORT_UNLOCK_CG_NUM : _ANPBasicPlayerCondition
	{
		public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_CONSORT_UNLOCK_CG_NUM; } }


		//判断数量范围
		private WCGLongRange _m_lCountRange; // 默认值为-1忽略判断


		public WCGLongRange checkCountRange { get { return _m_lCountRange; } }


		/// <summary>
		/// 读取条件信息
		/// </summary>
		/// <param name="_reader"></param>
		/// <returns></returns>
		public static NPPlayerCondition_CS_CONSORT_UNLOCK_CG_NUM readStr(ALStringReader _reader)
		{
			try
			{
				NPPlayerCondition_CS_CONSORT_UNLOCK_CG_NUM cond = new NPPlayerCondition_CS_CONSORT_UNLOCK_CG_NUM();

				string minVS = _reader.readItem(':');
				if (null != minVS)
				{
					string maxVS = _reader.readItem(':');
					if (null != maxVS)
						cond._m_lCountRange = new WCGLongRange(minVS, maxVS);
					else
						cond._m_lCountRange = new WCGLongRange(minVS, "-1");
				}
				else
				{
					cond._m_lCountRange = new WCGLongRange(1, -1);
				}

				return cond;
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError($"Can not read str for ENPPlayerConditionType.CS_CONSORT_UNLOCK_CG_NUM[{_reader.srcString}], error : {e}");
				return null;
			}
		}


		public override bool isEnable(NPVarInfo _varVariableInfo)
		{
			if (_m_lCountRange == null)
				return true;

#if NP_GAME
			// 获取玩家妃子组件
			GConsortComponent consortComp = NPPlayer.instance?.consortComp;
			if (consortComp == null)
				return false;

			// 获取妃子CG信息列表
			System.Collections.Generic.List<ConsortCgInfo> cgInfoList = consortComp.consortCgInfoList;
			if (cgInfoList == null)
				return false;

			// 统计已解锁的CG数量（列表中的CG都是已解锁的）
			long unlockCgCount = cgInfoList.Count;

			// 判断数量是否在指定范围内
			return _m_lCountRange.inRange(unlockCgCount);
#else
			return false;
#endif
		}
	}
}
