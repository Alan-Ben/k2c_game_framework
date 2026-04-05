package MGClient.Cmd.Cmds;

import GC2GS.p030_ShopOp.GC2GS_030_001_ReqBuyShopItem;
import GC2GS.p030_ShopOp.GC2GS_030_002_ReqFreeRefreshShop;
import GC2GS.p030_ShopOp.GC2GS_030_003_ReqRefreshShop;
import GC2GS.p030_ShopOp.GC2GS_030_004_ReqAutoRefreshShop;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "商店命令", name = "shop")
public class CmdShop extends CmdBase
{
    @Command(comment = "购买商品[商店配置id][商品id][数量]")
    public void buy(long _shopRefId, long _instanceId, long _count)
    {
        GC2GS_030_001_ReqBuyShopItem proto = new GC2GS_030_001_ReqBuyShopItem();
        proto.setShopRefId(_shopRefId);
        proto.setInstanceId(_instanceId);
        proto.setCount(_count);
        getOwner().sendGameMsg(proto);
    }

    @Command(comment = "免费刷新商店[商店配置id]")
    public void freeRefresh(long _shopRefId)
    {
        GC2GS_030_002_ReqFreeRefreshShop proto = new GC2GS_030_002_ReqFreeRefreshShop();
        proto.setShopRefId(_shopRefId);
        getOwner().sendGameMsg(proto);
    }

    @Command(comment = "付费刷新商店[商店配置id]")
    public void payRefresh(long _shopRefId)
    {
        GC2GS_030_003_ReqRefreshShop proto = new GC2GS_030_003_ReqRefreshShop();
        proto.setShopRefId(_shopRefId);
        getOwner().sendGameMsg(proto);
    }

    @Command(comment = "自动刷新商店[商店配置id]")
    public void autoRefresh(long _shopRefId)
    {
        GC2GS_030_004_ReqAutoRefreshShop proto = new GC2GS_030_004_ReqAutoRefreshShop();
        proto.setShopRefId(_shopRefId);
        getOwner().sendGameMsg(proto);
    }
}
