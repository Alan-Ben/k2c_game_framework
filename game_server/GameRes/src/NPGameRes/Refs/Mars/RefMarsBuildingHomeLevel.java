package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

@RefTable(tableName = "mars_building_home_level")
public class RefMarsBuildingHomeLevel extends RefBase
{
    private static RefMarsBuildingHomeLevelMgr _g_mgr = new RefMarsBuildingHomeLevelMgr();

    public static RefMarsBuildingHomeLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsBuildingHomeLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsBuildingHomeLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsBuildingHomeLevel newRef = (RefMarsBuildingHomeLevel) _newRef;
        level = newRef.level;
        energy_consume_per_min = newRef.energy_consume_per_min;
        overdrive_energy_consume_per_min = newRef.overdrive_energy_consume_per_min;
        on_oxygen_yield = newRef.on_oxygen_yield;
        overdrive_oxygen_yield = newRef.overdrive_oxygen_yield;
        off_oxygen_yield = newRef.off_oxygen_yield;
        oxygen_yield_adjust_coefficient = newRef.oxygen_yield_adjust_coefficient;
        output_per_min = newRef.output_per_min;
        build_output_bonus_s = newRef.build_output_bonus_s;
    }

    @Override
    public long Id()
    {
        return level;
    }

    public static class RefMarsBuildingHomeLevelMgr extends RefTableContainer<RefMarsBuildingHomeLevel>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public int level;
    public int energy_consume_per_min;//能源消耗（每分钟）
    public int overdrive_energy_consume_per_min;//最大功率能源消耗（每分钟）
    public int on_oxygen_yield;//开启时的功率
    public int overdrive_oxygen_yield;//最大功率时的功率
    public int off_oxygen_yield;//关闭时的功率
    public int oxygen_yield_adjust_coefficient;//氧气等级修正系数
    public ArrayList<NPCommonCostItem> output_per_min = new ArrayList<NPCommonCostItem>();//每分钟主基地产出

    //增加主城建设的时候给与的火星币产出时间
    //https://www.teambition.com/task/695a7da72626e1eef2ff84c7
    public int build_output_bonus_s;//建造本基地等级的时候，默认给与产出加成时间（秒）
    
    /**
     * 获取对应的消耗数值
     * @param _isNormalOn
     * @param _isOverdriveOn
     * @return
     */
    public int getConsumePerMin(boolean _isNormalOn, boolean _isOverdriveOn)
    {
    	if(_isNormalOn)
    	{
    		return _isOverdriveOn ? overdrive_energy_consume_per_min : energy_consume_per_min;
    	}
    	
    	return 0;
    }
    
    /**
     * 获取对应的氧气值
     * @param _isNormalOn
     * @param _isOverdriveOn
     * @return
     */
    public int getOxygenYield(boolean _isNormalOn, boolean _isOverdriveOn)
    {
    	if(_isNormalOn)
    	{
    		return _isOverdriveOn ? overdrive_oxygen_yield : on_oxygen_yield;
    	}
    	
    	return off_oxygen_yield;
    }
}