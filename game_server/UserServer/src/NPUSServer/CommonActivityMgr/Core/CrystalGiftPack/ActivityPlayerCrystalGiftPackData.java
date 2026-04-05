package NPUSServer.CommonActivityMgr.Core.CrystalGiftPack;

import Common.CommonFuncObj.CrystalGiftPack_Info;
import GS2GC.p017_ActivityOp.GS2GC_017_059_OnActivityCrystalGiftPackRefresh;
import GS2GC.p017_ActivityOp.GS2GC_017_061_OnActivityCrystalGiftPackItemBuy;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.CrystalGiftPack.RefCrystalGiftPack;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.ActivityPlayerCrystalGiftPackBuyRecordBO;
import USDB.Bo.ActivityPlayerCrystalGiftPackInfoBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动玩家钻石礼包数据
 */
public class ActivityPlayerCrystalGiftPackData
{
    // 所属礼包组
    private ActivityCrystalGiftPackGroupInfo _m_groupInfo;
    // 礼包数据库对象
    private ActivityPlayerCrystalGiftPackInfoBO _m_bo;
    // 购买记录 <礼包ID, 购买记录>
    private List<ActivityPlayerCrystalGiftPackBuyRecord> _m_buyRecordList;

    /**
     * 构造函数
     * @param _groupInfo 所属礼包组
     */
    public ActivityPlayerCrystalGiftPackData(ActivityCrystalGiftPackGroupInfo _groupInfo)
    {
        _m_groupInfo = _groupInfo;
        _m_buyRecordList = new ArrayList<>();
    }

    /**
     * 初始化礼包信息数据库对象
     * @param _bo 数据库对象
     */
    public void initInfoFromDB(ActivityPlayerCrystalGiftPackInfoBO _bo)
    {
        _m_bo = _bo;
    }

    /**
     * 初始化购买记录
     * @param _bo 购买记录数据库对象
     */
    public void initBuyRecordFromDB(ActivityPlayerCrystalGiftPackBuyRecordBO _bo)
    {
        _m_buyRecordList.add(new ActivityPlayerCrystalGiftPackBuyRecord(_bo));
    }

    /**
     * 清空购买记录
     */
    public void clearBuyRecords()
    {
        //如果还没初始化礼包数据, 则不需要处理
        if (_m_bo == null)
            return;

        _m_buyRecordList.clear();

        // 清空购买记录
        _m_groupInfo.getActivity().getUSServer().getBM().getBM(ActivityPlayerCrystalGiftPackBuyRecordBO.class).delAll("group_db_id", _m_bo.getId());
    }

    /**
     * 尝试刷新礼包
     * @param _userData 玩家数据
     * @return 是否成功刷新
     */
    public Result tryRefresh(NPUSUserData _userData)
    {
        long nowMs = CommonFunc.getNowTimeMS();
        if (_m_bo == null)
        {
            // 计算下次刷新时间
            long nextRefreshMs = _m_groupInfo.calcNextRefreshTimeMs(nowMs);

            // 更新或创建数据库记录
            BM bmObj = _m_groupInfo.getActivity().getUSServer().getBM();

            ActivityPlayerCrystalGiftPackInfoBO bo = new ActivityPlayerCrystalGiftPackInfoBO();
            bo.setInstanceId(bmObj, _m_groupInfo.getActivityInstanceId());
            bo.setGroupId(bmObj, _m_groupInfo.getGroupId());
            bo.setCid(bmObj, _userData.getCid());
            bo.setNextRefreshTimeMs(bmObj, nextRefreshMs);
            bo.insert(bmObj);
            _m_bo = bo;
        }else
        {
            // 检查是否可以刷新
            if (_m_bo.getNextRefreshTimeMs() > nowMs)
                return Result.SUCC;

            // 计算下次刷新时间
            long nextRefreshMs = _m_groupInfo.calcNextRefreshTimeMs(nowMs);

            // 清空购买记录
            clearBuyRecords();

            // 更新或创建数据库记录
            _m_bo.saveNextRefreshTimeMs(_m_groupInfo.getActivity().getUSServer().getBM(), nextRefreshMs);
        }

        //推送变更
        _userData.sendMsgToGC(new GS2GC_017_059_OnActivityCrystalGiftPackRefresh(_m_groupInfo.getActivityInstanceId(), makeProto()));

        return Result.SUCC;
    }

    /**
     * 查询购买记录
     * @param _packId 礼包ID
     * @return 购买记录
     */
    public ActivityPlayerCrystalGiftPackBuyRecord lookupBuyRecord(long _packId)
    {
        for (ActivityPlayerCrystalGiftPackBuyRecord buyRecord : _m_buyRecordList)
        {
            if (buyRecord.getGiftPackId() == _packId)
            {
                return buyRecord;
            }
        }
        return null;
    }

    /**
     * 获取购买数量
     * @param _packId 礼包ID
     * @return 购买数量
     */
    public long getHadBuyCount(long _packId)
    {
        ActivityPlayerCrystalGiftPackBuyRecord buyRecord = lookupBuyRecord(_packId);
        return buyRecord == null ? 0 : buyRecord.getHadBuyCount();
    }

    /**
     * 获取折扣
     * @param _refItem
     * @return
     */
    public int getDiscount(RefCrystalGiftPack _refItem)
    {
        if (_refItem.discount > 0)
            return _refItem.discount;

        return 10000;
    }

    /**
     * 购买礼包
     * @param _userData 玩家数据
     * @param _packId 礼包ID
     * @param _buyCount 购买数量
     * @param _context 玩家上下文
     * @return 是否成功购买
     */
    public Result buyGiftPack(NPUSUserData _userData, long _packId, int _buyCount, NPPlayerContext _context)
    {
        //查询礼包配置
        RefCrystalGiftPack refCrystalGiftPack = RefCrystalGiftPack.getMgr().get(_packId);
        if (refCrystalGiftPack == null)
            return CommErr.REF_NOT_FOUND;

        //检查礼包组ID是否匹配
        if (refCrystalGiftPack.crystal_gift_pack_group_id != _m_groupInfo.getGroupId())
            return CommErr.PARAM_ERROR;

        //尝试刷新
        tryRefresh(_userData);

        //判断是否超过上限
        long hadBuyCount = getHadBuyCount(_packId);
        if (hadBuyCount + _buyCount > refCrystalGiftPack.buy_num)
            return ActivityErr.ACTIVITY_CRYSTAL_GIFT_BUY_LIMIT;

        List<NPCommonCostItem> costItemList = new ArrayList<>();

        if (refCrystalGiftPack.times_price_type_id != 0)
        {
            for (int i = 0; i < _buyCount; i++)
            {
                //检查购买价格
                NPCommonCostItem cost = UsFunc.calCostPrice(_userData, refCrystalGiftPack.times_price_type_id, (int) (hadBuyCount + i));
                if (cost == null)
                    return CommErr.REF_NOT_FOUND;

                CommonFunc.mergeCostItem(costItemList, cost);
            }
        }else
        {
            costItemList.addAll(CommonFunc.itemMultiple(refCrystalGiftPack.cost_item, _buyCount));

            int discount = getDiscount(refCrystalGiftPack);
            //计算折扣 只有固定消耗需要计算折扣 【GOB-6161】
            if (discount != 10000)
                costItemList = CommonFunc.costItemDiscount(costItemList, discount);
        }

        if (!_userData.hasCostItemList(costItemList))
            return CommErr.ITEM_NOT_ENOUGH;

        if (!_userData.spendItem(costItemList, _context))
            return CommErr.CONSUME_FAIL;


        BM bmObj = _m_groupInfo.getActivity().getUSServer().getBM();

        //查询购买记录
        ActivityPlayerCrystalGiftPackBuyRecord record = lookupBuyRecord(_packId);
        if (record == null)
        {
            ActivityPlayerCrystalGiftPackBuyRecordBO bo = new ActivityPlayerCrystalGiftPackBuyRecordBO();
            bo.setInstanceId(bmObj, _m_groupInfo.getActivityInstanceId());
            bo.setGroupDbId(bmObj, _m_bo.getId());
            bo.setGroupId(bmObj, _m_groupInfo.getGroupId());
            bo.setCid(bmObj, _userData.getCid());
            bo.setItemId(bmObj, _packId);
            bo.setHadBuyCount(bmObj, _buyCount);
            bo.insert(bmObj);

            record = new ActivityPlayerCrystalGiftPackBuyRecord(bo);
            _m_buyRecordList.add(record);
        } else
        {
            record.addBuyCount(bmObj, _buyCount);
        }

        //获取道具
        _userData.gainItemList(CommonFunc.itemMultiple(refCrystalGiftPack.item_list, _buyCount), _context);

        //推送变更
        _userData.sendMsgToGC(new GS2GC_017_061_OnActivityCrystalGiftPackItemBuy(
                _m_groupInfo.getActivityInstanceId(),
                _m_groupInfo.getGroupId(),
                record.makeProto()));

        return Result.SUCC;
    }

    /**
     * 构造礼包组协议
     * @return 礼包组协议
     */
    public CrystalGiftPack_Info makeProto()
    {
        CrystalGiftPack_Info proto = new CrystalGiftPack_Info();
        proto.setGroupId(_m_groupInfo.getGroupId());
        proto.setNextRefreshTimeMs(_m_bo == null ? 0 : _m_bo.getNextRefreshTimeMs());
        for (ActivityPlayerCrystalGiftPackBuyRecord buyRecord : _m_buyRecordList)
        {
            proto.addBuyRecordList(buyRecord.makeProto());
        }
        return proto;
    }
}
