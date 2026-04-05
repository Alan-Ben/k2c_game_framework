using CommonEnum;
using NPCommon;
using NPEnum;

namespace GOE
{
    public static class GGUIHarvestUtil
    {
#if NP_GAME
        
        /// <summary>
        /// 道具类型转化为资源收集目标对象枚举
        /// </summary>
        /// <param name="_itemInfo"></param>
        /// <returns></returns>
        public static EHarvestType toEHarvestType(this NPCommon_ItemInfo _itemInfo)
        {
            if (_itemInfo == null)
                return EHarvestType.NONE;

            ENPItemType itemType = (ENPItemType)_itemInfo.getItemType();
            long itemId = _itemInfo.getSubId();

            return toEHarvestType(itemType, itemId);
        }

        /// <summary>
        /// 道具类型转化为资源收集目标对象枚举
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static EHarvestType toEHarvestType(ENPItemType _itemType, long itemId)
        {
            switch (_itemType)
            {
                case ENPItemType.CURRENCY:
                    ECurrency currencyType = (ECurrency)itemId;
                    switch (currencyType)
                    {
                        case ECurrency.NONE:
                            return EHarvestType.DEFAULT;
                        case ECurrency.GEM:
                            return EHarvestType.GEM;
                        case ECurrency.SILVER:
                            return EHarvestType.SILVER;
                        // case ECurrency.FOOD:
                        //     return EHarvestType.FOOD;
                        case ECurrency.P_EXP:
                            return EHarvestType.P_EXP;
                        case ECurrency.DINNER_COIN:
                            return EHarvestType.DINNER_COIN;
                        case ECurrency.DAILY_QUEST_ACTIVE_POINT:
                            return EHarvestType.DAILY_QUEST_ACTIVE_POINT;
                        case ECurrency.HERO_EXP:
                            return EHarvestType.HERO_EXP;
                        case ECurrency.TOWER_COIN:
                            return EHarvestType.TOWER_COIN;
                        case ECurrency.GUILD_COIN:
                            return EHarvestType.GUILD_COIN;
                        case ECurrency.DUNGEON_COIN:
                            return EHarvestType.DUNGEON_COIN;
                        case ECurrency.MARS_ENERGY:
                            return EHarvestType.MARS_ENERGY;
                        default:
                            return EHarvestType.DEFAULT;
                    }

                case ENPItemType.ACHIEVE_POINT:
                    return EHarvestType.ACHIEVE_POINT;

                case ENPItemType.BAG_ITEM:
                    return EHarvestType.BAG_ITEM;

                case ENPItemType.SYS_INFO:
                    NPCommonItem guildBoxActivePointItem = GRefdataCoreMgr.instance.npGeneral.guild_box_active_point_item;
                    if (guildBoxActivePointItem != null && itemId == guildBoxActivePointItem.itemId)
                        return EHarvestType.GUILD_BOX_ACTIVE_POINT;
                    return EHarvestType.DEFAULT;

                default:
                    return EHarvestType.DEFAULT;
            }
        }
        
        /// <summary>
        /// 道具类型转化为资源收集目标对象枚举
        /// </summary>
        /// <param name="_itemInfo"></param>
        /// <returns></returns>
        public static EHarvestType getEHarvestType(ENPItemType _itemType, long _subId)
        {
            switch (_itemType)
            {
                case ENPItemType.CURRENCY:
                    ECurrency currencyType = (ECurrency)_subId;
                    switch (currencyType)
                    {
                        case ECurrency.NONE:
                            return EHarvestType.DEFAULT;
                        case ECurrency.GEM:
                            return EHarvestType.GEM;
                        case ECurrency.SILVER:
                            return EHarvestType.SILVER;
                        // case ECurrency.FOOD:
                        //     return EHarvestType.FOOD;
                        case ECurrency.P_EXP:
                            return EHarvestType.P_EXP;
                        case ECurrency.DINNER_COIN:
                            return EHarvestType.DINNER_COIN;
                        case ECurrency.DAILY_QUEST_ACTIVE_POINT:
                            return EHarvestType.DAILY_QUEST_ACTIVE_POINT;
                        case ECurrency.HERO_EXP:
                            return EHarvestType.HERO_EXP;
                        case ECurrency.TOWER_COIN:
                            return EHarvestType.TOWER_COIN;
                        case ECurrency.GUILD_COIN:
                            return EHarvestType.GUILD_COIN;
                        case ECurrency.DUNGEON_COIN:
                            return EHarvestType.DUNGEON_COIN;
                        case ECurrency.MARS_ENERGY:
                            return EHarvestType.MARS_ENERGY;
                        default:
                            return EHarvestType.DEFAULT;
                    }

                case ENPItemType.ACHIEVE_POINT:
                    return EHarvestType.ACHIEVE_POINT;

                case ENPItemType.BAG_ITEM:
                    return EHarvestType.BAG_ITEM;

                case ENPItemType.SYS_INFO:
                    NPCommonItem guildBoxActivePointItem = GRefdataCoreMgr.instance.npGeneral.guild_box_active_point_item;
                    if (guildBoxActivePointItem != null && _subId == guildBoxActivePointItem.itemId)
                        return EHarvestType.GUILD_BOX_ACTIVE_POINT;
                    return EHarvestType.DEFAULT;

                default:
                    return EHarvestType.DEFAULT;
            }
        }
#endif

    }
}