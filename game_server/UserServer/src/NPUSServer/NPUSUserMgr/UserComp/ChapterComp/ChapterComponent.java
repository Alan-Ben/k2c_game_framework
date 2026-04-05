package NPUSServer.NPUSUserMgr.UserComp.ChapterComp;

import ALBasicServer.ALProcess.ALProcess;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Chapter.RefChapterStagePlot;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerChapterBO;
import USDB.Bo.PlayerChapterEventBO;
import USDB.Bo.PlayerChapterPlotRecordBO;

import java.util.ArrayList;
import java.util.List;

public class ChapterComponent extends _ANPUserComponent
{
    private ChapterInfo _m_chapterInfo;

    private List<Long> _m_hadDrawPlotRewardList;

    public ChapterComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.CHAPTER);

        _m_hadDrawPlotRewardList = new ArrayList<>();
    }

    public ChapterInfo getChapterInfo()
    {
        return _m_chapterInfo;
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("ChapterComponent._init");
        process.addResDelegateProcess(action -> _initChapterInfo(action::dealAction), "chapter_info_init",
                () -> USLog.error(getUSServer(), "player:{} load chapter bo fail.", getUserData().getCid()), false);
        process.addResDelegateProcess(action -> _initChapterEvent(action::dealAction), "chapter_event_init",
                () -> USLog.error(getUSServer(), "player:{} load chapter event bo fail.", getUserData().getCid()), false);
        process.addResDelegateProcess(action -> _initChapterPlotRecord(action::dealAction), "chapter_plot_init",
                () -> USLog.error(getUSServer(), "player:{} load chapter plot record bo fail.", getUserData().getCid()), false);
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "ChapterComponent _init fail cid:{}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 初始化章节信息
     * @param _handler
     */
    private void _initChapterInfo(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerChapterBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerChapterBO>()
        {
            @Override
            public void dealSuc(PlayerChapterBO _bo)
            {
                _m_chapterInfo = new ChapterInfo(ChapterComponent.this, _bo);
                _handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                if (getHasErr())
                {
                    USLog.error(getUSServer(), "ChapterComponent _initChapterInfo fail cid:{}", getUserData().getCid());
                    _handler.onRunOver(false);
                    return;
                }

                BM bmObj = getUSServer().getBM();

                PlayerChapterBO bo = new PlayerChapterBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setChapterId(bmObj, 1);
                bo.insert(bmObj);

                _m_chapterInfo = new ChapterInfo(ChapterComponent.this, bo);
                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 初始化章节事件信息
     * @param _handler
     */
    private void _initChapterEvent(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerChapterEventBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerChapterEventBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerChapterEventBO> _boList)
                    {
                        if (!_boList.isEmpty())
                        {
                            _m_chapterInfo.initEvent(_boList.get(0));
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    /**
     * 初始化章节剧情记录
     * @param _handler
     */
    private void _initChapterPlotRecord(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerChapterPlotRecordBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerChapterPlotRecordBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerChapterPlotRecordBO> _boList)
                    {
                        for (PlayerChapterPlotRecordBO bo : _boList)
                        {
                            _m_hadDrawPlotRewardList.add(bo.getPlotId());
                        }
                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
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
     * 获取已领取的剧情奖励列表
     * @return
     */
    public List<Long> getHadDrawPlotRewardList()
    {
        getUserData().lockUser();
        try
        {
            return new ArrayList<>(_m_hadDrawPlotRewardList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 批量领取剧情奖励
     *
     * 执行流程：
     * 1. 验证所有剧情ID的有效性
     * 2. 过滤已经领取过的剧情
     * 3. 批量插入数据库记录
     * 4. 发放所有奖励
     * 5. 推送领取成功通知
     *
     * @param _plotIdList 剧情ID列表
     * @param _context 玩家上下文
     * @return 结果
     */
    public Result drawPlotRewardList(List<Long> _plotIdList, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_plotIdList.isEmpty())
                return CommErr.PARAM_ERROR;

            // 收集实际需要领取的剧情ID
            List<Long> validPlotIdList = new ArrayList<>();
            // 奖励收集
            NPItemCostCollector_nosafe itemCollector = new NPItemCostCollector_nosafe();

            for (long plotId : _plotIdList)
            {
                // 跳过已领取的剧情
                if (_m_hadDrawPlotRewardList.contains(plotId))
                    continue;

                //检查剧情是否存在
                RefChapterStagePlot ref = RefChapterStagePlot.getMgr().get(plotId);
                if (ref == null)
                    continue;

                // 标记为已领取
                _m_hadDrawPlotRewardList.add(plotId);

                // 插入数据库记录
                PlayerChapterPlotRecordBO bo = new PlayerChapterPlotRecordBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.setPlotId(getUSServer().getBM(), plotId);
                bo.insert(getUSServer().getBM());

                //发放奖励
                itemCollector.addItemList(ref.reward_item_list);
                validPlotIdList.add(plotId);
            }

            // 发放奖励
            if (!itemCollector.isEmpty())
                getUserData().gainItemList(itemCollector.getItemList(), _context);

            // 推送领取成功通知
            if (!validPlotIdList.isEmpty())
                getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_055_OnChapterPlotRewardDraw(validPlotIdList));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
