package NPUSServer.NPUSUserMgr.UserComp.ArenaComp.Battle;

import Common.ArenaEnum.EArenaBuffType;
import Common.ArenaObj.Arena_SingleBuffInfo;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPGameRes.Refs.Arena.RefArenaBuff;
import NPUSServer.NPUSUserMgr.UserComp.ArenaComp.ArenaAKeyAttackDealer.ItemKeyList;

import java.util.List;

/**
 * 竞技场Buff处理器
 */
public class BuffProcessor
{
    /**
     * 获取buff增益百分比
     * @param buffList Buff列表
     * @return 增益百分比
     */
    public static int getBuffAddPercentage(List<Arena_SingleBuffInfo> buffList)
    {
        int addPer = 0;
        for (Arena_SingleBuffInfo buffInfo : buffList)
        {
            RefArenaBuff refArenaBuff = RefArenaBuff.getMgr().get(buffInfo.getBuffId());
            if (refArenaBuff == null)
                continue;

            addPer += refArenaBuff.value * buffInfo.getNum();
        }
        return addPer;
    }

    /**
     * 添加buff
     * @param buffList 当前buff列表
     * @param buffId   要添加的buffId
     */
    public static void addBuff(List<Arena_SingleBuffInfo> buffList, long buffId)
    {
        for (Arena_SingleBuffInfo buffInfo : buffList)
        {
            if (buffInfo.getBuffId() == buffId)
            {
                buffInfo.setNum(buffInfo.getNum() + 1);
                return;
            }
        }
        buffList.add(new Arena_SingleBuffInfo(buffId, 1));
    }

    /**
     * 选择最佳的可购买Buff
     * @param buffPrefer           偏好的buff类型
     * @param buffItemNum          各类buff道具数量
     * @param buffListRef          buff配置
     * @param _buffCostCollector
     * @return 选中的buff配置，如果没有可用的返回null
     */
    public static RefArenaBuff chooseBestBuff(EArenaBuffType buffPrefer, ItemKeyList buffItemNum, RefArenaBuff[] buffListRef, NPItemCostCollector_nosafe _buffCostCollector)
    {
        // 没有偏好buff，直接返回null
        if (buffPrefer == EArenaBuffType.NONE)
            return null;

        // 从偏好buff开始按顺序寻找可购买的buff
        for (int i = buffPrefer.ordinal(); i < buffListRef.length; i++)
        {
            RefArenaBuff refArenaBuff = buffListRef[i];
            if (refArenaBuff == null)
                continue;

            // 判断是否有足够的道具
            if (buffItemNum.consumeItem(refArenaBuff.cost.getItemType(), refArenaBuff.cost.getItemId(), refArenaBuff.cost.getCount()))
            {
                _buffCostCollector.addItem(refArenaBuff.cost);
                return refArenaBuff;
            }
        }
        return null;
    }
}