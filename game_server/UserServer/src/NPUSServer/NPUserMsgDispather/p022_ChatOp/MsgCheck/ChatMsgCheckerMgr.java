package NPUSServer.NPUserMsgDispather.p022_ChatOp.MsgCheck;

import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPChatMsgType;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class ChatMsgCheckerMgr
{
    public static final ChatMsgCheckerMgr _g_instance = new ChatMsgCheckerMgr();
    public static ChatMsgCheckerMgr getInstance() {return _g_instance;}

    private final _AChatMsgChecker<?, ?>[] _m_checkers = new _AChatMsgChecker<?, ?>[ENPChatMsgType.values().length];

    public ChatMsgCheckerMgr()
    {
    }

    private void regDealer(_AChatMsgChecker<?, ?> _dealer)
    {
        _m_checkers[_dealer.getType().ordinal()] = _dealer;
    }

    public _AChatMsgChecker<?, ?> getDealer(ENPChatMsgType _type)
    {
        return getDealer(_type.ordinal());
    }

    public _AChatMsgChecker<?, ?> getDealer(int _type)
    {
        return _m_checkers[_type];
    }


    public ResultOne<byte[]> doCheck(NPUSUserData _userData, int _msgType, byte[] _content)
    {
        if (_msgType < 0 || _msgType >= _m_checkers.length)
            return ResultOne.failed(CommErr.PARAM_ERROR);

        _AChatMsgChecker<?, ?> checker = _m_checkers[_msgType];
        if (null == checker)
            return ResultOne.succ(_content);

        return checker.checkMsg(_userData, _content);
    }
}
