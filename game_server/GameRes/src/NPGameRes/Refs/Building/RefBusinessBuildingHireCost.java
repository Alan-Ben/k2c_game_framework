package NPGameRes.Refs.Building;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;


@RefTable(tableName = "business_building_hire_cost")
public class RefBusinessBuildingHireCost extends RefBase implements _ILevelBasicObj
{
	private static RefBusinessBuildingHireCostMgr _g_mgr = new RefBusinessBuildingHireCostMgr();
    public static RefBusinessBuildingHireCostMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefBusinessBuildingHireCostMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefBusinessBuildingHireCostMgr) _mgr;
    }

    public static class RefBusinessBuildingHireCostMgr extends RefTableContainer<RefBusinessBuildingHireCost>
    {
    	//针对雇佣人数进行区间数据管理的对象，继承 _TLevelMapMgr
    	private BusinessBuildingHireCostAreaMgr _m_mgrHireCostArea = new BusinessBuildingHireCostAreaMgr();
        public BusinessBuildingHireCostAreaMgr getHireCostMgr() {return _m_mgrHireCostArea;}
    	
        @Override
        public void _onTableLoaded()
        {
        	BusinessBuildingHireCostAreaMgr hireCostMgr = new BusinessBuildingHireCostAreaMgr();
        	for(int i = 0; i < getList().size(); i++)
        	{
        		RefBusinessBuildingHireCost ref = getList().get(i);
        		if(null == ref)
        			continue;
        		
        		hireCostMgr._initAddLevelData(ref);
        	}
        	_m_mgrHireCostArea = hireCostMgr;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBusinessBuildingHireCost newRef = (RefBusinessBuildingHireCost) _newRef;
        employee_num = newRef.employee_num;
        hire_cost_coeff_A = newRef.hire_cost_coeff_A;
        hire_cost_coeff_B = newRef.hire_cost_coeff_B;
        hire_cost_coeff_C = newRef.hire_cost_coeff_C;
        hire_cost_coeff_D = newRef.hire_cost_coeff_D;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return employee_num;
    }
    
    /***********
     * 构造区间数据管理
     */
	@Override
	public int getLevel() 
	{
		return employee_num;
	}

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /***
     备注说明：
     	雇佣第n个员工的消耗=A*(n^3)+B*(n^2)+C*n+D
     */
	
    public int employee_num;//员工人数
    public int hire_cost_coeff_A;//消耗公式系数A
    public int hire_cost_coeff_B;//消耗公式系数B
    public int hire_cost_coeff_C;//消耗公式系数C
    public int hire_cost_coeff_D;//消耗公式系数D
    
    @RefField(isIgnore = true)
    public boolean isMax;//已经是最大配置
    
    /**********
     * 计算消耗数值
     * 
     * @param _employeeNum
     * @param _hireCostMultiple
     * @return
     */
    public long calCostNum(int _employeeNum, int _hireCostMultiple)
    {
    	//基础数值
    	long costNum = (long) Math.ceil(
    											1.0f * hire_cost_coeff_A / 10000f * Math.pow(_employeeNum, 3) + 
    											1.0f * hire_cost_coeff_B / 10000f * Math.pow(_employeeNum, 2) + 
    											1.0f * hire_cost_coeff_C / 10000f * _employeeNum + 
    											1.0f * hire_cost_coeff_D / 10000f);
    	//倍率
    	costNum *= _hireCostMultiple;
    	
    	return costNum;
    }
}
