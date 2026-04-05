package NPUSServer.NPUSUserMgr.UserComp.ClientDataComp;

import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerClientDataBO;

import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class ClientDataComponent extends _ANPUserComponent
{
    private Map<Integer, ClientData> _m_dataMap = new HashMap<>();

    public ClientDataComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.CLIENT_DATA);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerClientDataBO.class).findAll("cid", getUserData().getCid(),
            new _ASelectCallback<List<PlayerClientDataBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Client Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerClientDataBO> _list)
            {
                _initBo(_list);

                setInited();
            }
        });
    }

    protected void _initBo(List<PlayerClientDataBO> _list)
    {
        for (int i = 0; i < _list.size(); i++)
        {
            ClientData data = new ClientData(getUSServer(), _list.get(i));
            _m_dataMap.put(data.getKey(), data);
        }
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {

    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }

    ClientData lookupClientData(int key)
    {
        return _m_dataMap.get(key);
    }

    public ClientData ensurseClientData(int key)
    {
        ClientData data = lookupClientData(key);
        if (null == data)
        {
            BM bmObj = getUSServer().getBM();

            PlayerClientDataBO bo = new PlayerClientDataBO();
            bo.setCid(bmObj, getUserData().getCid());
            bo.setKey(bmObj, key);
            bo.insert(bmObj);
            data = new ClientData(getUSServer(), bo);
            _m_dataMap.put(data.getKey(), data);
        }
        return data;
    }
}
