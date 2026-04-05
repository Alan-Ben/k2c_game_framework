package NPUSServer.NPUSUserMgr.UserNameEqualMgr;

import NPCommon.CommonNameEqualMgr._ACommNameEqualMgr;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerNameBO;

import java.util.List;

/**
 * @description: 用于检测用户名有没有重复
 * @author: ricci
 * @date: 2023-03-03 14:04:04
 */
public class UserNameEqualMgr extends _ACommNameEqualMgr
{
    private NPUserServer _m_usUSServer;

    public UserNameEqualMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /**
     * 从数据库初始化
     */
    public boolean initFromDB()
    {
        List<PlayerNameBO> boList =
                getUSServer().getBM().getBM(PlayerNameBO.class).s_findAll();
        for (PlayerNameBO bo : boList)
        {
            initAddName(bo.getCname());
        }
        return true;
    }

    @Override
    protected Result _checkNameLegal(String _name)
    {
        //非法字符检查
        if (_name.contains("@"))
        {
            return PlayerErr.PLAYER_NAME_CONTAINS_ILLEGAL_CHARACTER;
        }

        //暂时不需要什么特殊的检查
        return Result.SUCC;
    }

    @Override
    public void __onAddNameRecord(String _name)
    {
        PlayerNameBO bo = new PlayerNameBO();
        bo.setCname(getUSServer().getBM(), _name);
        bo.insert(getUSServer().getBM());
    }

    @Override
    public void __onRemoveNameRecord(String _name)
    {
        getUSServer().getBM().getBM(PlayerNameBO.class).delAll("cname", _name);
    }

}
