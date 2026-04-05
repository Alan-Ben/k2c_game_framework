package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopInfo;

/**
 * @description: 任务相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "商店", name = "shop")
public class CmdShop extends UsCmdBase
{
    @ACommand(comment = "shop info")
    public String info()
    {
        return getOwner().getShopComponent().toString();
    }

    @ACommand(comment = "刷新商店(会清空刷新上限)[商店配置id]")
    public String refresh(long _shopRefId)
    {
        getOwner().getShopComponent().refreshShop(_shopRefId);
        return "ok";
    }

    @ACommand(comment = "刷新商品[商店配置id]")
    public String cRefresh(long _shopRefId)
    {
        getOwner().getShopComponent().refreshGoods(_shopRefId);
        return "ok";
    }

    @ACommand(comment = "设置商店刷新在n秒后刷新[商店配置id][秒数]")
    public String setRefreshSec(long _shopRefId, int _sec)
    {
        PlayerShopInfo shop = getOwner().getShopComponent().lookup(_shopRefId);
        if (shop == null)
        {
            return "shop not found";
        }
        shop.getBo().saveNextFreshTimeMs(getUserServer().getBM(), CommonFunc.getNowTimeMS() + _sec * 1000L);
        shop.pushChg();
        return "ok";
    }
}
