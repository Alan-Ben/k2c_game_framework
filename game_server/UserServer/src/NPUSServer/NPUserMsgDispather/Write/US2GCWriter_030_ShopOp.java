package NPUSServer.NPUserMsgDispather.Write;

import Common.CommonFuncObj.GiftPack_Info;
import GS2GC.p030_ShopOp.*;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopInfo;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopItem;

import java.util.ArrayList;
import java.util.List;

/**
 * @description: 030 协议writer
 * @author: mark
 * @date: 2022-04-08 11:58:05
 */
public class US2GCWriter_030_ShopOp
{

    public static GS2GC_030_001_RetBuyShopItem make_001_RetBuyShopItem()
    {
        GS2GC_030_001_RetBuyShopItem proto = new GS2GC_030_001_RetBuyShopItem();

        return proto;
    }

    public static GS2GC_030_002_RetFreeRefreshShop make_002_RetFreeRefreshShop()
    {
        GS2GC_030_002_RetFreeRefreshShop proto = new GS2GC_030_002_RetFreeRefreshShop();

        return proto;
    }

    public static GS2GC_030_003_RetRefreshShop make_003_RetRefreshShop()
    {
        GS2GC_030_003_RetRefreshShop proto = new GS2GC_030_003_RetRefreshShop();

        return proto;
    }

    public static GS2GC_030_004_RetAutoRefreshShop make_004_RetAutoRefreshShop()
    {
        GS2GC_030_004_RetAutoRefreshShop proto = new GS2GC_030_004_RetAutoRefreshShop();

        return proto;
    }

    public static GS2GC_030_050_OnShopRefresh make_050_OnShopRefresh(PlayerShopInfo _shop)
    {
        GS2GC_030_050_OnShopRefresh proto = new GS2GC_030_050_OnShopRefresh();
        proto.setShop(_shop.makeProto());
        return proto;
    }

    public static GS2GC_030_051_OnShopItemChg make_051_OnShopItemChg(PlayerShopItem _goods)
    {
        GS2GC_030_051_OnShopItemChg proto = new GS2GC_030_051_OnShopItemChg();
        proto.setShopRefId(_goods.getShopRefId());
        proto.setShopItem(_goods.makeProto());
        return proto;
    }

    // ============= 礼包相关协议 =============

    /**
     * 刷新礼包返回消息
     * @return 刷新礼包返回协议
     */
    public static GS2GC_030_021_RetRefreshGiftPack make_021_RetRefreshGiftPack()
    {
        return new GS2GC_030_021_RetRefreshGiftPack();
    }

    /**
     * 购买礼包返回消息
     * @return 购买礼包返回协议
     */
    public static GS2GC_030_022_RetBuyGiftPack make_022_RetBuyGiftPack()
    {
        return new GS2GC_030_022_RetBuyGiftPack();
    }

    /**
     * 礼包刷新推送消息
     * @param _infoList 礼包信息列表
     * @return 礼包刷新推送协议
     */
    public static GS2GC_030_060_OnGiftPackRefresh make_060_OnGiftPackRefresh(List<GiftPack_Info> _infoList)
    {
        GS2GC_030_060_OnGiftPackRefresh proto = new GS2GC_030_060_OnGiftPackRefresh();
        proto.getGiftPack().addAll(_infoList);
        return proto;
    }

    /**
     * 礼包变更推送消息
     * @param giftPackInfo 礼包信息
     * @return 礼包刷新推送协议
     */
    public static GS2GC_030_061_OnGiftPackBuyRecordChg make_061_OnGiftPackBuyRecordChg(GiftPack_Info giftPackInfo)
    {
        return new GS2GC_030_061_OnGiftPackBuyRecordChg(giftPackInfo);
    }

    /**
     * 礼包购买记录新增推送消息
     * @param giftPackInfo 礼包信息
     * @return 礼包购买记录新增推送协议
     */
    public static GS2GC_030_062_OnGiftPackBuyRecordAdd make_062_OnGiftPackBuyRecordAdd(GiftPack_Info giftPackInfo)
    {
        return new GS2GC_030_062_OnGiftPackBuyRecordAdd(giftPackInfo);
    }

    /**
     * 礼包购买记录移除推送消息
     * @param giftPackIdList 礼包ID列表
     * @return 礼包购买记录移除推送协议
     */
    public static GS2GC_030_063_OnGiftPackBuyRecordRemove make_063_OnGiftPackBuyRecordRemove(ArrayList<Long> giftPackIdList)
    {
        return new GS2GC_030_063_OnGiftPackBuyRecordRemove(giftPackIdList);
    }
}
