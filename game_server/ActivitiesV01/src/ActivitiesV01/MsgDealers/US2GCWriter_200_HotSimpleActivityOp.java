package ActivitiesV01.MsgDealers;

import Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_001_RetRegularActivityShopInfo;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_002_RetRegularActivityShopBuyItem;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_003_RetRegularActivityUseItem;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_004_RetRegularActivityShopRefresh;
import NPUSServer.Common.Context.NPPlayerContext;

/**
 * 200 协议writer
 */
public class US2GCWriter_200_HotSimpleActivityOp
{
    public static GS2GC_200_001_RetRegularActivityShopInfo make_001_RetRegularActivityShopInfo(RegularActivity_ShopInfo _proto)
    {
        GS2GC_200_001_RetRegularActivityShopInfo proto = new GS2GC_200_001_RetRegularActivityShopInfo();
        proto.setShopInfo(_proto);
        return proto;
    }

    public static GS2GC_200_002_RetRegularActivityShopBuyItem make_002_RetRegularActivityShopBuyItem()
    {
        return new GS2GC_200_002_RetRegularActivityShopBuyItem();
    }

    public static GS2GC_200_003_RetRegularActivityUseItem make_003_RetRegularActivityUseItem(NPPlayerContext _context)
    {
        GS2GC_200_003_RetRegularActivityUseItem proto = new GS2GC_200_003_RetRegularActivityUseItem();
        _context.getCollector().fillProtoList(proto.getItemList());
        return proto;
    }

    public static GS2GC_200_004_RetRegularActivityShopRefresh make_004_RetRegularActivityShopRefresh()
    {
        return new GS2GC_200_004_RetRegularActivityShopRefresh();
    }
}
