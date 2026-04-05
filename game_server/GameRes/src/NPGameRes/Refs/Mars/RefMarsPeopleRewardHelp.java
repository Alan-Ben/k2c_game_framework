package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "mars_people_reward_help")
public class RefMarsPeopleRewardHelp extends RefBase
{
    private static RefMarsPeopleRewardHelpMgr _g_mgr = new RefMarsPeopleRewardHelpMgr();

    public static RefMarsPeopleRewardHelpMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsPeopleRewardHelpMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsPeopleRewardHelpMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsPeopleRewardHelp newRef = (RefMarsPeopleRewardHelp) _newRef;
        id = newRef.id;
        add_satisfaction_degree = newRef.add_satisfaction_degree;
        reward_list = newRef.reward_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsPeopleRewardHelpMgr extends RefTableContainer<RefMarsPeopleRewardHelp>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }

        public RefMarsPeopleRewardHelp rndRefList()
        {
        	List<RefMarsPeopleRewardHelp> list = getList();
        	if(list.isEmpty())
        		return null;
        	
			int idx = CommonFunc.randomInt(list.size() - 1);
			return list.get(idx);
        }
    }

    public long id;
    public int add_satisfaction_degree;//增加满意度
    public ArrayList<NPCommonCostItem> reward_list = new ArrayList<>();//获取奖励列表
    
    @RefField(isIgnore = true)
    public RefMarsPeopleHelp helpRef;
}