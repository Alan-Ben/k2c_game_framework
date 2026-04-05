package NPGameRes.Refs.TreasureHunt;

import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;
import NPEnum.EQuality;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 区域id	矿石列表	奇物列表	解锁条件
 * area_id	ore_list	treasure_list	unlock_condition
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_area")
public class RefTreasureHuntArea extends RefBase
{
    private static RefTreasureHuntAreaMgr _g_mgr = new RefTreasureHuntAreaMgr();

    public static RefTreasureHuntAreaMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntAreaMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntAreaMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntArea newRef = (RefTreasureHuntArea) _newRef;
        area_id = newRef.area_id;
        ore_list = newRef.ore_list;
        treasure_list = newRef.treasure_list;
        unlock_condition = newRef.unlock_condition;
    }

    public static class RefTreasureHuntAreaMgr extends RefTableContainer<RefTreasureHuntArea>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return area_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long area_id;//区域id
    public List<Long> ore_list;//矿石列表
    public List<Long> treasure_list;//奇物列表
    public NPPlayerConditionGroupObj unlock_condition;//解锁条件

    @RefField(isIgnore = true)
    private Map<EQuality, List<RefTreasureHuntOre>> qualityOreMap = new HashMap<>(); // 按品质分组的矿石列表
    public void setQualityOreMap(Map<EQuality, List<RefTreasureHuntOre>> _qualityOreMap)
    {
        qualityOreMap = _qualityOreMap;
    }

    /**
     * 随机获取指定品质的矿石
     * @param _quality
     * @return
     */
    public RefTreasureHuntOre randomOre(EQuality _quality)
    {
        return CommonFunc.randSelect(qualityOreMap.get(_quality));
    }

    @RefField(isIgnore = true)
    private int treasureWeight = 0; // 奇物权重总和
    public void setTreasureWeight(int _treasureWeight)
    {
        treasureWeight = _treasureWeight;
    }
    public int getTreasureWeight()
    {
        return treasureWeight;
    }

    @RefField(isIgnore = true)
    private WeightValueList<RefTreasureHuntTreasure> treasureWeightList = new WeightValueList<>(); // 奇物权重列表
    public void setTreasureWeightList(WeightValueList<RefTreasureHuntTreasure> _treasureWeightList)
    {
        treasureWeightList = _treasureWeightList;
    }
    public RefTreasureHuntTreasure randomTreasure()
    {
        return treasureWeightList.random();
    }
}
