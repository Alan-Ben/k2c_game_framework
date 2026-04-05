package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "mars_satisfaction_degree")
public class RefMarsSatisfactionDegree extends RefBase
{
    private static RefMarsSatisfactionDegreeMgr _g_mgr = new RefMarsSatisfactionDegreeMgr();

    public static RefMarsSatisfactionDegreeMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsSatisfactionDegreeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsSatisfactionDegreeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsSatisfactionDegree newRef = (RefMarsSatisfactionDegree) _newRef;
        satisfaction_degree_per = newRef.satisfaction_degree_per;
        reward_list = newRef.reward_list;
    }

    @Override
    public long Id()
    {
        return satisfaction_degree_per;
    }

    public static class RefMarsSatisfactionDegreeMgr extends RefTableContainer<RefMarsSatisfactionDegree>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
        
        /**
         * 根据满意度查找对应的配置
         * @param _satisfaction
         * @return
         */
        public RefMarsSatisfactionDegree getBySatisfaction(int _satisfaction)
        {
        	List<RefMarsSatisfactionDegree> list = getList();
        	//从大往小查找，寻找第一个符合条件的配置
        	for(int i = list.size() - 1; i >= 0; i--)
        	{
        		RefMarsSatisfactionDegree ref = list.get(i);
        		if(null == ref)
        			continue;
        		
        		if(_satisfaction >= ref.satisfaction_degree_per)
        			return ref;
        	}
        	
        	return null;
        }
    }

    public int satisfaction_degree_per;//满意度万分比
    public ArrayList<NPCommonCostItem> reward_list = new ArrayList<>();//奖励列表
}