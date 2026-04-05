package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import GC2GS.p006_BagItemOp.GC2GS_006_008_ReqAKeyConvert;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.BagItemErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_SingleItemConvert;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.BagItem.RefBagItemConvert;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;
import NPUSServer.USLog;
import USLOGDB.Bo.LogItemConvertBO;

import java.util.ArrayDeque;
import java.util.ArrayList;

/**
 * 准备使用藏宝地图
 */
public class MsgDealer_GC2GS_006_008_ReqAKeyConvert extends NPUserMsgDealer<GC2GS_006_008_ReqAKeyConvert>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_008_ReqAKeyConvert _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //源道具收集器
        NPItemCollector originItemCollect = new NPItemCollector(0);
        //目标道具收集器
        NPItemCollector targetItemCollect = new NPItemCollector(0);

        //逐个校验是否能够转换
        ArrayList<NPCommon_SingleItemConvert> itemConvertList = _msg.getItemConvertList();
        for (NPCommon_SingleItemConvert convertObj : itemConvertList)
        {
            //校验转换
            Result result = _checkConvert(convertObj, originItemCollect, targetItemCollect);
            if (!result.isSucc())
            {
                _commiter.commitFailRes(result.getCode());
                return;
            }
        }

        //检查道具是否足够
        if (!userData.hasCostItemList(originItemCollect.getAllItemList()))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        //消耗
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ITEM_CONVERT);
        if (!userData.spendCostItemList(originItemCollect.getAllItemList(), context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        userData.gainItemList(targetItemCollect.getAllItemList(), context);

        _commiter.commitSucRes(US2GCWriter_006_BagItemOp.make_008_RetAKeyConvert());

        userData.sendMsgToGC(context.getCollector().toProto());

        try
        {
            BM bmObj = getUSServer().getBM();
            for (NPCommon_SingleItemConvert singleItemConvert : _msg.getItemConvertList())
            {
                //道具合成日志
                LogItemConvertBO logBo = new LogItemConvertBO();
                logBo.setCid(bmObj, userData.getCid());
                logBo.setOriItemInfo(bmObj, CommonFunc.protoItemList2String(singleItemConvert.getOriginItemList()));
                logBo.setTargetItemInfo(bmObj, String.format("%s-%d:%d", ENPItemType.BAG_ITEM, singleItemConvert.getTargetItem().getSubId(), singleItemConvert.getTargetItem().getCount()));
                CommLogDB.log(bmObj, logBo, context);
            }
        } catch (Exception e)
        {
            USLog.error(getUSServer(), "", e);
        }
    }

    /**
     * 转换校验
     * @param _convertObj
     * @param _originItemCollect
     * @param _targetItemCollect
     * @return
     */
    private Result _checkConvert(NPCommon_SingleItemConvert _convertObj, NPItemCollector _originItemCollect, NPItemCollector _targetItemCollect)
    {
        //目标道具
        NPCommon_ItemInfo targetItem = _convertObj.getTargetItem();

        //源道具转换栈
        ArrayDeque<NPCommon_ItemInfo> itemStack = new ArrayDeque<>();
        //把转换队列copy一份
        for (NPCommon_ItemInfo item : _convertObj.getOriginItemList())
        {
            itemStack.add(new NPCommon_ItemInfo(item.getItemType(), item.getSubId(), item.getCount(), null));//加上转换消耗

            ENPItemType itemType = ENPItemType.ENPItemType_FromInt(item.getItemType());
            if (itemType == null)
                return BagItemErr.BAG_ITEM_ERROR;

            _originItemCollect.addItem(itemType, item.getSubId(), item.getCount());
        }

        ENPItemType targetItemType = ENPItemType.ENPItemType_FromInt(targetItem.getItemType());
        if (targetItemType == null)
            return BagItemErr.BAG_ITEM_ERROR;

        _targetItemCollect.addItem(new NPCommonCostItem(targetItemType, targetItem.getSubId(), targetItem.getCount()));

        //当队列非空
        while (!itemStack.isEmpty())
        {
            //取出最后一个物品，做一次合并
            NPCommon_ItemInfo oriItem = itemStack.pollLast();

            //如果是最后一个
            if (itemStack.isEmpty())
            {
                //检查是不是要的初始道具
                if (oriItem.getItemType() == targetItem.getItemType() && oriItem.getSubId() == targetItem.getSubId() && oriItem.getCount() == targetItem.getCount())
                {
                    return Result.SUCC;
                } else
                {
                    //检查是否能继续转换,如果不行,则直接返回错误
                    RefBagItemConvert ref = RefBagItemConvert.getMgr().get(oriItem.getSubId());
                    if (null == ref)
                    {
                        return BagItemErr.BAG_ITEM_A_KEY_CONVERT_CHECK_FAIL;
                    }
                }
            }

            //检查对应的配置
            RefBagItemConvert ref = RefBagItemConvert.getMgr().get(oriItem.getSubId());
            if (null == ref)
                return CommErr.REF_NOT_FOUND;

            //检查是否可以整除
            if (oriItem.getCount() % ref.ori_item_num != 0)
                return BagItemErr.BAG_ITEM_A_KEY_CONVERT_CHECK_FAIL;

            //检查转换次数，失败则返回
            long canConvertTime = oriItem.getCount() / ref.ori_item_num;
            if (canConvertTime == 0)
                return BagItemErr.BAG_ITEM_A_KEY_CONVERT_CHECK_FAIL;

            //单次转换后得到物品
            NPCommon_ItemInfo convertGetItem = new NPCommon_ItemInfo(ref.target_item.getItemType().ordinal(), ref.target_item.getItemId(), canConvertTime, null);

            //加上转换手续费
            _originItemCollect.addItemList(ref.calConvertConsume((int) canConvertTime));

            //查看栈接下去一个物品,如果道具一样就合并;不一样直接放进栈中
            NPCommon_ItemInfo nextItem = itemStack.peekLast();
            if (nextItem != null && nextItem.getItemType() == convertGetItem.getItemType() && nextItem.getSubId() == convertGetItem.getSubId())
            {
                nextItem.setCount(nextItem.getCount() + convertGetItem.getCount());
            } else
            {
                itemStack.offerLast(convertGetItem);
            }
        }

        //转换失败
        return BagItemErr.BAG_ITEM_A_KEY_CONVERT_CHECK_FAIL;
    }
}