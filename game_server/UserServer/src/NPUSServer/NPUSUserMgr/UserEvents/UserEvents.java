package NPUSServer.NPUSUserMgr.UserEvents;

import NPCommon.Util.Delegate.ADelegateFour;
import NPCommon.Util.Delegate.ADelegateOne;
import NPCommon.Util.Delegate.ADelegateThree;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/******
 * 玩家事件类，用来管理玩家自身的事件
 */
public class UserEvents
{
    private NPUSUserData _m_userData;

    public UserEvents(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    public NPUSUserData getUserData()
    {
        return _m_userData;
    }

    //事件计数器变化 ENPPlayerEventRecordType，Id，count
    public ADelegateFour<Integer, Long, Long, Long> OnEventRecordChg = new ADelegateFour<>(this);
    //属性变更监听
    public ADelegateThree<ENPPlayerParam, Long, Long> OnParamChg = new ADelegateThree<>(this);
    //玩家升级
    public ADelegateOne<Integer> OnPlayerLvlUp = new ADelegateOne<>(this);
}
