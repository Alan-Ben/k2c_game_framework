using System.Collections.Generic;
using GC2GS.p006_BagItemOp;

namespace GOE
{
    public static class NPGSWriter_006_BagItemOp
    {
        public static GC2GS.p006_BagItemOp.GC2GS_006_001_ReqSellBagItem make_001_ReqSellBagItem(long _itemId, int _count)
        {
            GC2GS.p006_BagItemOp.GC2GS_006_001_ReqSellBagItem protocol = new GC2GS_006_001_ReqSellBagItem();
            protocol.setItemId(_itemId);
            protocol.setCount(_count);
            return protocol;
        }

        public static GC2GS.p006_BagItemOp.GC2GS_006_002_ReqBagUseItem make_002_ReqBagUseItem(long _itemId, int _count, List<int> _selectedIdxList)
        {
            GC2GS.p006_BagItemOp.GC2GS_006_002_ReqBagUseItem protocol = new GC2GS_006_002_ReqBagUseItem();
            protocol.setItemId(_itemId);
            protocol.setCount(_count);

            if(null != _selectedIdxList)
                protocol.getSelectedIdx().AddRange(_selectedIdxList);

            return protocol;
        }

        public static GC2GS.p006_BagItemOp.GC2GS_006_003_ReqRefreshQuitBagTime make_003_ReqRefreshQuitBagTime()
        {
            GC2GS.p006_BagItemOp.GC2GS_006_003_ReqRefreshQuitBagTime protocol = new GC2GS_006_003_ReqRefreshQuitBagTime();
            return protocol;
        }
        public static GC2GS.p006_BagItemOp.GC2GS_006_004_ReqClickBagTime make_004_ReqClickBagTime(long itemId)
        {
            GC2GS.p006_BagItemOp.GC2GS_006_004_ReqClickBagTime protocol = new GC2GS_006_004_ReqClickBagTime(itemId);
            return protocol;
        }
        
        //请求合成兑换物品
        public static GC2GS_006_007_ReqConvert make_007_ReqConvert(long _bagItemId, long _bagItemCount)
        {
            GC2GS_006_007_ReqConvert protocol = new GC2GS_006_007_ReqConvert(_bagItemId, _bagItemCount);
            return protocol;
        }

        //一键合成
        public static GC2GS_006_008_ReqAKeyConvert make_008_ReqAKeyConvert(List<NPCommon.NPCommon_SingleItemConvert> _list)
        {
            GC2GS_006_008_ReqAKeyConvert protocol = new GC2GS_006_008_ReqAKeyConvert(_list);
            return protocol;
        }

        /// <summary>
        /// 请求对骑士使用物品
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_count"></param>
        /// <returns></returns>
        public static GC2GS_006_009_ReqBagUseItemForSelectHero make_009_ReqBagUseItemForSelectHero(long _heroId, long _itemId, long _count)
        {
            GC2GS_006_009_ReqBagUseItemForSelectHero protocol = new GC2GS_006_009_ReqBagUseItemForSelectHero(_heroId, _itemId, (int)_count);
            return protocol;
        }

        /// <summary>
        /// 请求对情人使用物品
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_count"></param>
        /// <returns></returns>
        public static GC2GS_006_010_ReqBagUseItemForSelectConsort make_010_ReqBagUseItemForSelectConsort(long _consortId, long _itemId, int _count)
        {
            GC2GS_006_010_ReqBagUseItemForSelectConsort protocol = new GC2GS_006_010_ReqBagUseItemForSelectConsort(_consortId, _itemId, _count);
            return protocol;
        }
    }
}
