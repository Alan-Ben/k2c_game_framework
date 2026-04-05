package ShareCodeCenter;

import ALServerLog.ALServerLog;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ISelectDBInterface;
import NPCommon.DDAlert.DDAlert;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import ShareCodeCenter.Conf.Server.ShareCodeServerConfMgr;
import ShareCodeCenter.ShareCodeServer.ShareCodeServer;
import ShareCodeDB.ShareCodeDBInitializer;

import java.util.ArrayList;
import java.util.List;

public class ShareCodeCenter implements _ISelectDBInterface
{
    private static ShareCodeCenter _g_instance = new ShareCodeCenter();

    public static ShareCodeCenter getInstance()
    {
        return _g_instance;
    }

    protected ShareCodeCenter()
    {
        //设置全局变量
        _g_instance = this;

        _m_bm = new BM(this);
        _m_ddAlert = new DDAlert();
        _m_allServerList = new ArrayList<>();
    }

    //数据库访问对象
    private BM _m_bm;
    private DDAlert _m_ddAlert;
    private List<ShareCodeServer> _m_allServerList;

    public BM getBM()
    {
        return _m_bm;
    }

    public DDAlert getDDAlert() {return _m_ddAlert;}

    public List<ShareCodeServer> getAllServerList()
    {
        return _m_allServerList;
    }

    @Override
    public EDBTag switchDBTag(EDBTag _srcTag)
    {
        return _srcTag;
    }

    public void initAddServer(ShareCodeServer server)
    {
        _m_allServerList.add(server);
    }

    /**
     * 初始化
     * @return
     */
    public boolean init()
    {
        ALServerLog.initALServerLog();

        //检查配置
        if (!ShareCodeServerConfMgr.getInstance().init())
        {
            CommLog.error("PayCenter Conf Init Fail!!!");
            return false;
        }

        //初始化数据库连接
        if (!ShareCodeDBInitializer.initConnections())
        {
            CommLog.error("Share Code DB init connection Fail!!!");
            return false;
        }

        //初始化数据库连接
        if (!ShareCodeDBInitializer.initDB())
        {
            CommLog.error("Share Code DB init db Fail!!!");
            return false;
        }

        //初始化数据库连接
        if (!SCCParams.getInstance().initFromDB())
        {
            CommLog.error("SCCParams init Fail!!!");
            return false;
        }



        return true;
    }
}
