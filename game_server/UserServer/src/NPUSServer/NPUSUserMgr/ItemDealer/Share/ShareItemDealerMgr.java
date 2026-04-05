package NPUSServer.NPUSUserMgr.ItemDealer.Share;

import NPEnum.ENPShareItemType;

/**************
 *
 * 分享物件具体处理逻辑的管理
 *
 * @author mark
 *
 */
public class ShareItemDealerMgr
{
    private static ShareItemDealerMgr _g_instance = new ShareItemDealerMgr();

    public static ShareItemDealerMgr getInstance()
    {
        return _g_instance;
    }

    //////////////////////////// 实例部分 ////////////////////////////

    private final _AShareItemDealer[] _m_arrShareItemDealerArr;

    public ShareItemDealerMgr()
    {
        _m_arrShareItemDealerArr = new _AShareItemDealer[ENPShareItemType.ENPShareItemType_Length];

        regDealer(new ShareItemDealer_Box());
    }

    public void regDealer(_AShareItemDealer _dealer)
    {
        _m_arrShareItemDealerArr[_dealer.getShareItem().ordinal()] = _dealer;
    }

    public _AShareItemDealer getDealer(ENPShareItemType _type)
    {
        if (null == _type)
            return null;

        return _m_arrShareItemDealerArr[_type.ordinal()];
    }
}
