package NPGameRes.Refs.Dinner;

import Common.DinnerEnum.EDinnerPermitType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "dinner_permit")
public class RefDinnerPermit extends RefBase
{
    private static RefDinnerSeatMgr _g_mgr = new RefDinnerSeatMgr();
    public static RefDinnerSeatMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefDinnerSeatMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefDinnerSeatMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDinnerPermit newRef = (RefDinnerPermit) _newRef;
        permit_type = newRef.permit_type;
        dinner_id = newRef.dinner_id;
        lifeTs = newRef.lifeTs;
    }
    
    public static class RefDinnerSeatMgr extends RefTableContainer<RefDinnerPermit>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
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
        return permit_type.ordinal();
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public EDinnerPermitType permit_type = EDinnerPermitType.NONE;//凭证类型（EDinnerPermitType）
    public long dinner_id;//宴会id
    public int lifeTs;//有效期
    
    @RefField(isIgnore = true)
    public RefDinnerType dinnerRef;
}
