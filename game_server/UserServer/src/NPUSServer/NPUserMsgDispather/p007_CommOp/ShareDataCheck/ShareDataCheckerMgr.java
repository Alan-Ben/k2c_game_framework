package NPUSServer.NPUserMsgDispather.p007_CommOp.ShareDataCheck;

import CommonEnum.EShareCodeType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class ShareDataCheckerMgr
{
    public static final ShareDataCheckerMgr _g_instance = new ShareDataCheckerMgr();

    public static ShareDataCheckerMgr getInstance()
    {
        return _g_instance;
    }

    private final _AShareDataChecker<?, ?>[] _m_checkers = new _AShareDataChecker<?, ?>[EShareCodeType.values().length];

    public ShareDataCheckerMgr()
    {
    }

    private void regDealer(_AShareDataChecker<?, ?> _dealer)
    {
        _m_checkers[_dealer.getType().ordinal()] = _dealer;
    }

    public ResultOne<?> doCheck(NPUSUserData _userData, int _msgType, byte[] _content)
    {
        if (_msgType < 0 || _msgType >= _m_checkers.length)
            return new ResultOne<>(CommErr.PARAM_ERROR, null);

        _AShareDataChecker<?, ?> checker = _m_checkers[_msgType];
        if (null == checker)
            return new ResultOne<>(CommErr.PARAM_ERROR, null);

        return checker.checkAndTranMsg(_userData, _content);
    }
}
