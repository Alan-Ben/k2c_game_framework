package MGClient.Cmd.Cmds;

import Hotfix.V01.GC2GS.p200_HotSimpleActivityOp.GC2GS_200_001_ReqRegularActivityShopInfo;
import Hotfix.V01.GC2GS.p200_HotSimpleActivityOp.GC2GS_200_002_ReqRegularActivityShopBuyItem;
import Hotfix.V01.GC2GS.p200_HotSimpleActivityOp.GC2GS_200_003_ReqRegularActivityUseItem;
import Hotfix.V01.GC2GS.p200_HotSimpleActivityOp.GC2GS_200_004_ReqRegularActivityShopRefresh;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_001_RetRegularActivityShopInfo;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_002_RetRegularActivityShopBuyItem;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_003_RetRegularActivityUseItem;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_004_RetRegularActivityShopRefresh;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;
import NPCommon.Log.CommLog;

@Commander(comment = "万能活动", name = "regular")
public class CmdRegular extends CmdBase
{
    @Command(comment = "信息[活动实例id]")
    public void info(long _activityInstanceId)
    {
        GC2GS_200_001_ReqRegularActivityShopInfo proto = new GC2GS_200_001_ReqRegularActivityShopInfo();
        proto.setActivityInstanceId(_activityInstanceId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_200_001_RetRegularActivityShopInfo>(GS2GC_200_001_RetRegularActivityShopInfo.class)
        {
            @Override
            public void handle(GS2GC_200_001_RetRegularActivityShopInfo _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }

    @Command(comment = "购买[活动实例id][商品id][数量]")
    public void buy(long _activityInstanceId, long _itemId, int _num)
    {
        GC2GS_200_002_ReqRegularActivityShopBuyItem proto = new GC2GS_200_002_ReqRegularActivityShopBuyItem();
        proto.setActivityInstanceId(_activityInstanceId);
        proto.setItemId(_itemId);
        proto.setNum(_num);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_200_002_RetRegularActivityShopBuyItem>(GS2GC_200_002_RetRegularActivityShopBuyItem.class)
        {
            @Override
            public void handle(GS2GC_200_002_RetRegularActivityShopBuyItem _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }

    @Command(comment = "使用[活动实例id][商品id][是否十连]")
    public void use(long _activityInstanceId, long _itemId, boolean _isTen)
    {
        GC2GS_200_003_ReqRegularActivityUseItem proto = new GC2GS_200_003_ReqRegularActivityUseItem();
        proto.setActivityInstanceId(_activityInstanceId);
        proto.setItemId(_itemId);
        proto.setIsTen(_isTen);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_200_003_RetRegularActivityUseItem>(GS2GC_200_003_RetRegularActivityUseItem.class)
        {
            @Override
            public void handle(GS2GC_200_003_RetRegularActivityUseItem _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }

    @Command(comment = "刷新[活动实例id]")
    public void refresh(long _activityInstanceId)
    {
        GC2GS_200_004_ReqRegularActivityShopRefresh proto = new GC2GS_200_004_ReqRegularActivityShopRefresh();
        proto.setActivityInstanceId(_activityInstanceId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_200_004_RetRegularActivityShopRefresh>(GS2GC_200_004_RetRegularActivityShopRefresh.class)
        {
            @Override
            public void handle(GS2GC_200_004_RetRegularActivityShopRefresh _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }
}
