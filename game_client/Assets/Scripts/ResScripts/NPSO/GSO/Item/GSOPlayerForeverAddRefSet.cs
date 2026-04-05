using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 玩家永久加成表
	/// </summary>
	[Serializable]
	public class PlayerForeverAddRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public int add_limit;
		public NPPlayerPropertyModifier player_pro_add;

		
		/// <summary>
		/// 获取对应属性，乘了倍率的
		/// </summary>
		/// <param name="_count"></param>
		/// <returns></returns>
		public NPPlayerPropertyModifier getProperty(int _count)
		{
			if (null == player_pro_add)
			{
				return null;
			}

			return player_pro_add.duplicate(_count);
		}
	}

	public class GSOPlayerForeverAddRefSet : _TALSOBasicRefSet<PlayerForeverAddRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
		public static string objName { get { return "player_forever_add"; } }
	}
}