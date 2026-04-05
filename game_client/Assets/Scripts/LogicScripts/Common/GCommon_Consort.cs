using System;
using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseEnum;
using NPCommon;
using NPEnum;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 获取展示类型获取对应道具
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public static NPCommonItem getItemByConsortShowType(EBagItemUse_ConsortDrawShowType _type)
        {
            NPCommonItem item = null;
            if (_type == EBagItemUse_ConsortDrawShowType.CHARM)
                item = GRefdataCoreMgr.instance.npGeneral.consort_charm_item;
            else if (_type == EBagItemUse_ConsortDrawShowType.INTIMACY)
                item = GRefdataCoreMgr.instance.npGeneral.consort_intimacy_item;
            else if (_type == EBagItemUse_ConsortDrawShowType.CHARM_POINT)
                item = GRefdataCoreMgr.instance.npGeneral.consort_bless_point_item;
            // else if (_type == EBagItemUse_ConsortDrawShowType.LIKE)
            //     item = GRefdataCoreMgr.instance.npGeneral.consort_like_item;
            return item;
        }
    }
}