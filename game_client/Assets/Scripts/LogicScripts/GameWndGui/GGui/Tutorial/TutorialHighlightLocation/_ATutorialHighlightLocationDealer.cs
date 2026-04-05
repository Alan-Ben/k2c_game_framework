using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _ATutorialHighlightLocationDealer
    {
        /******************
         * 获取高亮位置类型
         */
        public abstract ETutorialHighlightLocationType highlightLocationType { get; }

        /// <summary>
        /// 获取高亮中心在UI根节点中位置
        /// </summary>
        /// <param name="centerUIRootPosition"></param>
        /// <returns></returns>
        public abstract bool getCenterUIRootPosition(out Vector2 centerUIRootPosition);

        /********************
         * 从节点中读取相关信息
         */
        public static _ATutorialHighlightLocationDealer readHighlightLocationDealer(string _infoStr)
        {
            int splitPos = _infoStr.IndexOf(':');

            string rectTypeStr = _infoStr;
            string rectInfoStr = null;
            if(splitPos > 0)
            {
                //读取第一个字段：条件类型
                rectTypeStr = _infoStr.Substring(0, splitPos);
                //读取剩余字符串
                rectInfoStr = _infoStr.Substring(splitPos + 1);
            }

            //读取类型枚举
            ETutorialHighlightLocationType rectType = (ETutorialHighlightLocationType)ALCommon.EnumParse(typeof(ETutorialHighlightLocationType), rectTypeStr, true);
            return readHighlightLocationDealer(rectType, rectInfoStr);
        }

        public static _ATutorialHighlightLocationDealer readHighlightLocationDealer(ETutorialHighlightLocationType _rectType, string _infoStr)
        {
            switch(_rectType)
            {
                case ETutorialHighlightLocationType.CHAPTER_MAP_NODE:
                    return TutorialHighlightLocationDealer_CHAPTER_MAP_NODE.readVariable(_infoStr);
                case ETutorialHighlightLocationType.HERO_MAIN_LIST_P:
                    return TutorialHighlightLocationDealer_HERO_MAIN_LIST_P.readVariable(_infoStr);
                case ETutorialHighlightLocationType.CHILD_MAIN_LIST_P:
                    return TutorialHighlightLocationDealer_CHILD_MAIN_LIST_P.readVariable(_infoStr);
                case ETutorialHighlightLocationType.BAG_MAIN_USE_BAG_PAGE_ITEM_P:
                    return TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_ITEM_P.readVariable(_infoStr);
                case ETutorialHighlightLocationType.BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN:
                    return TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN.readVariable(_infoStr);
                case ETutorialHighlightLocationType.SHOP_LIST:
                    return TutorialHighlightLocationDealer_SHOP_LIST.readVariable(_infoStr);
                case ETutorialHighlightLocationType.HOME_ENTRY_POINT:
                    return TutorialHighlightLocationDealer_HOME_ENTRY_POINT.readVariable(_infoStr);
                case ETutorialHighlightLocationType.GUILD_COOPERATE_REWARD_POS_CAN_GET:
                    return TutorialHighlightLocationDealer_GUILD_COOPERATE_REWARD_POS_CAN_GET.readVariable(_infoStr);
                case ETutorialHighlightLocationType.GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT:
                    return TutorialHighlightLocationDealer_GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT.readVariable(_infoStr);
                case ETutorialHighlightLocationType.CONSORT_MAIN_LIST_P:
                    return TutorialHighlightLocationDealer_CONSORT_MAIN_LIST_P.readVariable(_infoStr);
                default:
                    return null;
            }
        }
    }
}