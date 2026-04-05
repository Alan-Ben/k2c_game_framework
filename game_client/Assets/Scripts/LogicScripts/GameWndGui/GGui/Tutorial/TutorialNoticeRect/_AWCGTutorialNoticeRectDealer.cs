using System;
using System.Collections.Generic;

using UnityEngine;

using ALPackage;

namespace GOE
{
    /*******************
    * 获取教程对应对象区域的获取方式
    **/
    public abstract class _AWCGTutorialNoticeRectDealer
    {
        /******************
        * 获取条件类型
        */
        public abstract EWCGTutorialNoticeRectType noticeRectType { get; }

        /*******************
        * 根据类型获取对应卡牌，纯客户端过程，不带任何参数
        **/
        public abstract bool getRect(out Rect _rect);

        /********************
        * 从节点中读取相关信息
        */
        public static _AWCGTutorialNoticeRectDealer readRectDealer(string _infoStr)
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
            EWCGTutorialNoticeRectType rectType = (EWCGTutorialNoticeRectType)ALCommon.EnumParse(typeof(EWCGTutorialNoticeRectType), rectTypeStr, true);
            return readRectDealer(rectType, rectInfoStr);
        }

        public static _AWCGTutorialNoticeRectDealer readRectDealer(EWCGTutorialNoticeRectType _rectType, string _infoStr)
        {
            switch(_rectType)
            {
                case EWCGTutorialNoticeRectType.NONE:
                    return null;
                case EWCGTutorialNoticeRectType.MAIN_PET_LIST_P:
                    return NPTutorialNoticeMainPetListIndexRectDealer.readVariable(_infoStr);
                case EWCGTutorialNoticeRectType.HERO_MAIN_LIST_P:
                    return NPTutorialNoticeHeroMainListRectDealer.readVariable(_infoStr);
                case EWCGTutorialNoticeRectType.CHILD_MAIN_LIST_P:
                    return NPTutorialNoticeChildMainListRectDealer.readVariable(_infoStr);
                case EWCGTutorialNoticeRectType.SHOP_LIST:
                    return NPTutorialNoticeShopListRectDealer.readVariable(_infoStr);
                case EWCGTutorialNoticeRectType.GUILD_COOPERATE_REWARD_POS_CAN_GET:
                    return NPTutorialNoticeGuildCooperateRewardPosCanGetRectDealer.readVariable(_infoStr);
                case EWCGTutorialNoticeRectType.GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT:
                    return NPTutorialNoticeGuildCooperateAttrPosCanConstructRectDealer.readVariable(_infoStr);
                case EWCGTutorialNoticeRectType.CONSORT_MAIN_LIST_P:
                    return NPTutorialNoticeConsortMainListRectDealer.readVariable(_infoStr);
                default:
                    return null;
            }
        }
    }
}
