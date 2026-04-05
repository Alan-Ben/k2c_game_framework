package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;

@RefTable(tableName = "mars_immigration_per")
public class RefMarsImmigrationPer extends RefBase
{
    private static RefMarsImmigrationPerMgr _g_mgr = new RefMarsImmigrationPerMgr();

    public static RefMarsImmigrationPerMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsImmigrationPerMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsImmigrationPerMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsImmigrationPer newRef = (RefMarsImmigrationPer) _newRef;
        id = newRef.id;
        can_gain_perple_num_per = newRef.can_gain_perple_num_per;
        wei = newRef.wei;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsImmigrationPerMgr extends RefTableContainer<RefMarsImmigrationPer>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//id
    public int can_gain_perple_num_per;//可招收的人数上限(万分比)
    public int wei;//权重
    
    @RefField(isIgnore = true)
    public int minNumPer;
    
    /**
     * 计算本次移民数量
     * @param _value
     * @param _sb
     * @return
     */
    public long randNumPer(long _value, StringBuilder _sb)
    {
    	if(_value <= 0)
    	{
    		if(null != _sb)
    		{
    			_sb.append(", value:0");
    		}
    		return 0;
    	}
    	
    	if(_value <= 1)
    	{
    		if(null != _sb)
    		{
    			_sb.append(", value:1");
    		}
    		return 1;
    	}
    	
    	int curPer = CommonFunc.randomInt(this.minNumPer, can_gain_perple_num_per);
    	if(null != _sb)
		{
			_sb.append(", rang:").append(this.minNumPer).append("-").append(can_gain_perple_num_per);
		}
    	if(curPer <= 0)
    	{
    		if(null != _sb)
    		{
    			_sb.append(", per:0").append(", value:0");
    		}
    		return 0;
    	}
    	
    	long newValue = Math.max(_value * curPer / 10000, 1) ;
		if(null != _sb)
		{
			_sb.append(", per:").append(curPer).append(", value:").append(newValue);
		}
    	return newValue;
    }
}