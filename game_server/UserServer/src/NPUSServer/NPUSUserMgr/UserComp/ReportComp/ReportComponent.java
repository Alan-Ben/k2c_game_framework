package NPUSServer.NPUSUserMgr.UserComp.ReportComp;

import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerReportBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 玩家举报组件
 * @author mj
 *
 */
public class ReportComponent extends _ANPUserComponent
{
    // 举报记录列表
    private ArrayList<PlayerReportBO> _m_alReportBoList;

    public ReportComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.REPORT);

        _m_alReportBoList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        // 异步从数据库加载举报记录
        getUSServer().getBM().getBM(PlayerReportBO.class).findAll("cid", getUserData().getCid(),
            new NPCommon.DB._ASelectCallback<List<PlayerReportBO>>()
            {
                @Override
                public void dealFail()
                {
                    // 日志记录加载失败
                    USLog.error(getUserData().getUSServer(), "Can not load report data[cid:" + getUserData().getCid() + "]");
                    getUserData().setDataLoadFail();
                }

                @Override
                public void dealSuc(List<PlayerReportBO> _list)
                {
                    _initFromBoList(_list);

                    setInited();
                }
            });
    }

    private void _initFromBoList(List<PlayerReportBO> _boList)
    {
        for (int i = 0; i < _boList.size(); i++)
        {
            PlayerReportBO bo = _boList.get(i);
            if (bo == null)
                continue;

            _m_alReportBoList.add(bo);
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

    @Override
    public void dispose()
    {
    }

    /**
     * 添加举报记录
     * @param _targetCid 被举报玩家CID
     * @param _reportContent 举报内容
     */
    public void addReport(long _targetCid, String _reportContent)
    {
        getUserData().lockUser();

        try
        {
            PlayerReportBO bo = new PlayerReportBO();
            bo.setCid(getBM(), getUserData().getCid());
            bo.setTargetCid(getBM(), _targetCid);
            bo.setContent(getBM(), _reportContent);
            bo.setReportMs(getBM(), CommonFunc.getNowTimeMS());
            bo.insert(getBM());

            _m_alReportBoList.add(bo);

            // 保持举报记录列表的大小不超过50条，删除最早的记录
            while(_m_alReportBoList.size() > 50)
            {
                PlayerReportBO removeBo = _m_alReportBoList.remove(0);
                if(null == removeBo)
                    continue;

                removeBo.del(getBM());
            }
        }
        finally
        {
            getUserData().unlockUser();
        }
    }
}
