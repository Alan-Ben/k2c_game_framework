using System.Collections.Generic;

namespace GOE
{
    //玩家信息相关
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 获取气泡配置，对非法id做默认值处理，展示数据用这个
        /// </summary>
        /// <param name="_refId"></param>
        /// <returns></returns>
        public NPPlayerBubbleRefObj getPlayerBubbleRefObj(long _refId)
        {
            NPPlayerBubbleRefObj refObj = playerBubbleCore.getRef(_refId);
            if (refObj == null)
                refObj = playerBubbleCore.getRef(GRefdataCoreMgr.instance.npGeneral.chat_default_bubble_id);
            return refObj;
        }

        /// <summary>
        /// 根据经验获取对应玩家等级数据
        /// </summary>
        /// <param name="_exp"></param>
        /// <returns></returns>
        public PlayerLvlRefObj getPlayerLevelRefByExp(long _exp)
        {
            PlayerLvlRefObj targetRef = null;
            if (playerLvlCore.refList.Count > 0)
                targetRef = playerLvlCore.refList[0];

            for (int i = 0; i < playerLvlCore.refList.Count; i++)
            {
                if (playerLvlCore.refList[i].exp > _exp)
                    break;
                targetRef = playerLvlCore.refList[i];
            }

            return targetRef;
        }


        /// <summary>
        /// 获取玩家皮肤等级表数据
        /// </summary>
        /// <param name="_skinId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public PlayerSkinLevelRefObj getPlayerSkinLevelRef(long _skinId, long _level)
        {
            for (int i = 0; i < playerSkinLevelRefCore.refList.Count; i++)
            {
                if (playerSkinLevelRefCore.refList[i] != null && playerSkinLevelRefCore.refList[i].player_skin_id == _skinId &&
                    playerSkinLevelRefCore.refList[i].skin_level == _level)
                    return playerSkinLevelRefCore.refList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取排序后的伙伴解锁展示列表
        /// </summary>
        /// <returns></returns>
        public List<PlayerHeroUnlockShowRefObj> getPlayerHeroUnlockShowRefListBySort()
        {
            List<PlayerHeroUnlockShowRefObj> heroRefList = new List<PlayerHeroUnlockShowRefObj>();
            heroRefList.AddRange(GRefdataCoreMgr.instance.playerHeroUnlockShowCore.refList);
            heroRefList.Sort(_playerHeroUnlockShowRefSort);
            return heroRefList;
        }
        private int _playerHeroUnlockShowRefSort(PlayerHeroUnlockShowRefObj _a, PlayerHeroUnlockShowRefObj _b)
        {
            if (_a == null && _b != null)
                return 1;
            if (_a != null && _b == null)
                return -1;
            if (_a == null && _b == null)
                return 0;

            bool isGainA = NPPlayer.instance.heroComponent.getHeroInfo(_a.hero_id) != null;
            bool isGainB = NPPlayer.instance.heroComponent.getHeroInfo(_b.hero_id) != null;
            bool isUnlockA = _a.unlock_condition == null || _a.unlock_condition.IsEnable(null);
            bool isUnlockB = _b.unlock_condition == null || _b.unlock_condition.IsEnable(null);

            // 未获得的排前面
            if (isGainA && !isGainB)
                return 1;
            if (!isGainA && isGainB)
                return -1;
            // 都获得了就按 原顺序 排
            if (isGainA && isGainB)
                return _a.id.CompareTo(_b.id);

            // 都没获得就解锁的排前面
            if (isUnlockA && !isUnlockB)
                return -1;
            if (!isUnlockA && isUnlockB)
                return 1;

            // 如果都一样就按 原顺序 排
            return _a.id.CompareTo(_b.id);
        }
    }
}
