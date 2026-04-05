using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 实验室表
	/// </summary>
	[Serializable]
	public class TreasureHuntLabRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public _NPPlayerConditionSerializeInfo unlock_condition; //解锁条件
		public string unlock_condition_desc;//解锁条件描述
		public string name; //名称
		public long scene_id;//场景id
		public NPGTextureIndex banner_img;// banner图片
		
		private List<TreasureHuntTreasureRefObj> _m_lTreasureList = null;
		public List<TreasureHuntTreasureRefObj> treasureList { get { return _m_lTreasureList; } }
		
		public bool isUnlock(bool showTip = false)
		{
			bool isUnlock = unlock_condition == null || unlock_condition.isNoConditionOrEnable(null);
			
#if NP_GAME
			if (!isUnlock && showTip)
				NPGUIAddSceneCenterTip.instance.showTransTextInfo(unlock_condition_desc);
#endif
			
			return isUnlock;
		}
		
		public void addTreasure(TreasureHuntTreasureRefObj _treasureRefObj)
		{
			if (_m_lTreasureList == null)
				_m_lTreasureList = new List<TreasureHuntTreasureRefObj>();
			_m_lTreasureList.Add(_treasureRefObj);
		}
	}

	public class GSOTreasureHuntLabRefSet : _TALSOBasicRefSet<TreasureHuntLabRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_lab"; } }
	}
}