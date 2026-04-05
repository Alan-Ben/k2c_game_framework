package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.TreasureHuntObj.TreasureHunt_CaptureResult_Ore;
import Common.TreasureHuntObj.TreasureHunt_OreInfo;
import Common.TreasureHuntObj.TreasureHunt_OreRankItem;
import Common.TreasureHuntObj.TreasureHunt_TransOreResult;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.TreasureHuntErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.EQuality;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntArea;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntOre;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent.SkillInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerTreasureHuntOreBO;

import java.util.ArrayList;
import java.util.List;

public class TreasureHuntOreMgr
{
    private TreasureHuntComponent _m_comp;
    private ArrayList<TreasureHuntOreInfo> _m_oreList;

    public TreasureHuntOreMgr(TreasureHuntComponent _comp)
    {
        _m_comp = _comp;
        _m_oreList = new ArrayList<>();
    }

    public TreasureHuntComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 获取矿石列表
     */
    public List<TreasureHuntOreInfo> getOreList()
    {
        return new ArrayList<>(_m_oreList);
    }

    /**
     * 初始化矿石数据
     * @param _handler
     */
    public void _initOreFromDB(_ICallBackBool _handler)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerTreasureHuntOreBO.class).findAll("cid", _m_comp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerTreasureHuntOreBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerTreasureHuntOreBO> _boList)
                    {
                        for (PlayerTreasureHuntOreBO bo : _boList)
                        {
                            RefTreasureHuntOre refOre = RefTreasureHuntOre.getMgr().get(bo.getOreId());
                            if (refOre == null)
                            {
                                USLog.error(_m_comp.getUSServer(), "TreasureHuntOreMgr _initOreFromDB refOre is null， cid:{} oreId:{}",
                                        _m_comp.getUserData().getCid(), bo.getOreId());
                                continue;
                            }

                            TreasureHuntOreInfo oreInfo = new TreasureHuntOreInfo(TreasureHuntOreMgr.this, bo, refOre);
                            _m_oreList.add(oreInfo);
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
     * 获取矿石数量
     * @return
     */
    public long getOreNum()
    {
        return _m_oreList.size();
    }

    /**
     * 查找矿石信息
     * @param _id
     * @return
     */
    public TreasureHuntOreInfo lookupOre(long _id)
    {
        getUserData().lockUser();
        try
        {
            for (TreasureHuntOreInfo oreInfo : _m_oreList)
            {
                if (oreInfo.getOreId() == _id)
                {
                    return oreInfo;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取所有待处理矿石的数量
     * @return
     */
    public int getPendingOreNum()
    {
        getUserData().lockUser();
        try
        {
            int pendingNum = 0;
            for (TreasureHuntOreInfo oreInfo : _m_oreList)
            {
                pendingNum += oreInfo.getPendingNum();
            }
            return pendingNum;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获得矿石
     * @param _oreRef
     * @param _weight
     * @param _context
     */
    public boolean addOre(RefTreasureHuntOre _oreRef, int _weight, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            boolean isAdvanced = _oreRef.isAdvanced(_weight);
            boolean isFirstTimeDrawAdvanced = false;

            TreasureHuntOreInfo oreInfo = lookupOre(_oreRef.Id());
            if (oreInfo == null)
            {
                long nowTimeMS = CommonFunc.getNowTimeMS();
                BM bmObj = _m_comp.getUSServer().getBM();

                PlayerTreasureHuntOreBO oreBo = new PlayerTreasureHuntOreBO();
                oreBo.setCid(bmObj, _m_comp.getUserData().getCid());
                oreBo.setOreId(bmObj, _oreRef.Id());
                oreBo.setFirstGainTimeMs(bmObj, nowTimeMS);
                oreBo.setMaxRecord(bmObj, _weight);
                oreBo.setReachMaxRecordTimeMs(bmObj, nowTimeMS);
                if (isAdvanced)
                {
                    oreBo.setHadReachAdvanced(bmObj, true);
                    isFirstTimeDrawAdvanced = true;
                }
                oreBo.setTotalGainNum(bmObj, 1);
                oreBo.insert(bmObj);

                oreInfo = new TreasureHuntOreInfo(this, oreBo, _oreRef);
                _m_oreList.add(oreInfo);

                // 推送变更
                getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_053_OnTreasureHuntOreAdd(oreInfo.makeProto()));
            } else
            {
                isFirstTimeDrawAdvanced = oreInfo.onGainNewOre(_weight, _context);
            }

            // 注册异步任务，处理矿石获得
            ALSynTaskManager.getInstance().regTask(() -> _m_comp.getCompositeMgr().onGainOre(_oreRef, isAdvanced));

            getComp().getUSServer().getTreasureHuntRankMgr().ensureRank(_oreRef.Id())
                    .updatePlayerRecord(_m_comp.getUserData().getCid(), _weight, CommonFunc.getNowTimeMS());

            return isFirstTimeDrawAdvanced;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 转化待处理的矿石
     * @param _context
     * @return
     */
    public ResultOne<List<TreasureHunt_TransOreResult>> transPendingOre(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            List<TreasureHunt_TransOreResult> transResultList = new ArrayList<>();

            // 进行矿石转换
            for (TreasureHuntOreInfo oreInfo : _m_oreList)
            {
                oreInfo.transPendingOre(transResultList, _context);
            }

            // 如果没有待处理的矿石，则返回错误
            if (transResultList.isEmpty())
                return ResultOne.failed(TreasureHuntErr.TREASURE_HUNT_NO_PENDING_ORE_CAN_TRANS);

            return ResultOne.succ(transResultList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * @param _refArea
     * @param _quality
     * @param _context
     * @return
     */
    public TreasureHunt_CaptureResult_Ore randomGainOre(RefTreasureHuntArea _refArea, EQuality _quality, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 随机获取矿石
            RefTreasureHuntOre refOre = _refArea.randomOre(_quality);
            if (refOre == null)
            {
                USLog.error(_m_comp.getUSServer(), "TreasureHuntOreMgr randomGainOre refOre is null， cid:{} quality:{} ",
                        _m_comp.getUserData().getCid(), _quality);
                return null;
            }

            boolean isFirstTime = lookupOre(refOre.Id()) == null;

            int grade;
            if (isFirstTime)
            {
                grade = refOre.firstTimeGradeProbabilityList.getList().random();
            } else
            {
                grade = refOre.grade_probability_list.getList().randomWithExtraWeight(RefGeneral.Ref().treasure_hunt_advanced_ore_min_grade,
                        getComp().getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.TREASURE_HUNT_ADVANCED_ORE_WEIGHT_ADD));
            }

            if (grade < 0 || grade >= refOre.mass_reward_grade_list.getRewardGradeList().size())
            {
                USLog.error(_m_comp.getUSServer(), "TreasureHuntOreMgr randomGainOre grade is invalid, cid:{} oreId:{} quality:{} grade:{}",
                        _m_comp.getUserData().getCid(), refOre.Id(), _quality, grade);
                return null; // 无效档位
            }

            int weight = refOre.mass_reward_grade_list.getRewardGradeList().get(grade).randomMass();// 随机权重
            if (weight < 0)
            {
                USLog.error(_m_comp.getUSServer(), "TreasureHuntOreMgr randomGainOre weight is less than 0, cid:{} oreId:{} quality:{}",
                        _m_comp.getUserData().getCid(), refOre.Id(), _quality);
                return null;
            }

            // 添加矿石
            boolean isFirstTimeDrawAdvanced = addOre(refOre, weight, _context);

            TreasureHunt_CaptureResult_Ore captureResult = new TreasureHunt_CaptureResult_Ore();
            captureResult.setIsFirstCapture(isFirstTime);
            captureResult.setOreId(refOre.Id());
            captureResult.setWeight(weight);
            captureResult.setIsFirstDrawAdvanced(isFirstTimeDrawAdvanced);

            TreasureHunt_OreRankItem rankItem = getComp().getUSServer().getTreasureHuntRankMgr().ensureRank(refOre.Id()).lookupItemByRank(1);
            if (rankItem != null)
            {
                captureResult.setServerMaxCid(rankItem.getCid());
                captureResult.setServerMaxWeight(rankItem.getWeight());
            }

            // 返回新获得的矿石信息
            return captureResult;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 填充矿石列表到指定集合
     * @param _oreList
     */
    public void fillProto(List<TreasureHunt_OreInfo> _oreList)
    {
        getUserData().lockUser();
        try
        {
            for (TreasureHuntOreInfo oreInfo : _m_oreList)
            {
                _oreList.add(oreInfo.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 收集矿石技能信息到指定列表中
     * 
     * @param _skillList 技能信息列表
     */
    public void collectSkillInfo(List<SkillInfo> _skillList)
    {
        for (TreasureHuntOreInfo oreInfo : _m_oreList) 
        {
            // 普通技能
            if (oreInfo.getNormalSkillInfo() != null && oreInfo.getNormalSkillInfo().getLevel() > 0) 
            {
                long skillId = oreInfo.getRef().normal_skill_id;
                int level = oreInfo.getNormalSkillInfo().getLevel();
                _skillList.add(new SkillInfo(skillId, level));
            }
            
            // 高级技能
            if (oreInfo.getAdvancedSkillInfo() != null && oreInfo.getAdvancedSkillInfo().getLevel() > 0) 
            {
                long skillId = oreInfo.getRef().advanced_skill_id;
                int level = oreInfo.getAdvancedSkillInfo().getLevel();
                _skillList.add(new SkillInfo(skillId, level));
            }
        }
    }
}
