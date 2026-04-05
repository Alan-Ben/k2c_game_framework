package NPUSServer.ServerCallback;

import ALBasicServer.ALTask._IALAsynCallBackTask;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.PlayerBO;

public class NPSynLoadPlayerDataTask implements _IALSynTask
{
    private NPUSUserData _m_udUserData;

    public NPSynLoadPlayerDataTask(NPUSUserData _userData)
    {
        _m_udUserData = _userData;
    }

    @Override
    public void run()
    {
        _m_udUserData.getUSServer().getBM().getBM(PlayerBO.class).findOne("cid", _m_udUserData.getCid(), new _ASelectCallback<PlayerBO>()
        {
            @Override
            public void dealFail()
            {
                //设置玩家数据为创角项目，首次创角会给予一些初始化物品，给予之后重置本标记位
                _m_udUserData.markFirstCreate();

                BM bmObj = _m_udUserData.getUSServer().getBM();

                //创建新数据
                PlayerBO bo = new PlayerBO();
                bo.setCid(bmObj, _m_udUserData.getCid());
                bo.setUid(bmObj, _m_udUserData.getUid());
                bo.setCname(bmObj, "@" + _m_udUserData.getCid());
                bo.setLvl(bmObj, 1);
                bo.setCurTitleShow(bmObj, true);//默认开启-其他玩家可以查看当前玩家的头像

                //此时需要执行玩家的创角操作
                bo.insert(bmObj, new _IALAsynCallBackTask<PlayerBO>()
                {
                    @Override
                    public void dealFail()
                    {
                        USLog.error(_m_udUserData.getUSServer(), "Can not insert user data[cid:" + _m_udUserData.getCid() + "]");
                        _m_udUserData.setDataLoadFail();
                    }

                    @Override
                    public void dealSuc(PlayerBO _userBo)
                    {
                        //设置玩家基础数据，并从这个初始化开始各数据模块的初始化
                        _m_udUserData.getPlayerComponent().initBo(_userBo);
                        _m_udUserData.getComponentMgr().delayChekNotLoadComponents();
                    }
                });
            }

            @Override
            public void dealSuc(PlayerBO _userBo)
            {
                //设置玩家基础数据，并从这个初始化开始各数据模块的初始化
                _m_udUserData.getPlayerComponent().initBo(_userBo);
                _m_udUserData.getComponentMgr().delayChekNotLoadComponents();
            }
        });
    }
}
