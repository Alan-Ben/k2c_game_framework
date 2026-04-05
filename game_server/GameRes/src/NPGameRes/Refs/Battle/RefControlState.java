package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer._ARefIndexContainer;
import WCGCommon.Enum.NPEnum.EWCGControlState;

import java.util.ArrayList;

/**
 * @author scott
 */
@RefTable(tableName = "control_state")
public class RefControlState extends RefBase
{
    private static RefControlStateMgr _g_mgr = new RefControlStateMgr();

    public static RefControlStateMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefControlStateMgr extends _ARefIndexContainer<RefControlState>
    {
        @Override
        protected int _getIndexCount()
        {
            return EWCGControlState.values().length;
        }

    }

    @Override
    public RefControlStateMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefControlStateMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefControlState newRef = (RefControlState) _newRef;
        control_state = newRef.control_state;
        add_buf_list = newRef.add_buf_list;
        state_sfx_list = newRef.state_sfx_list;
        end_sfx_list = newRef.end_sfx_list;
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
        return control_state.ordinal();
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public EWCGControlState control_state;
    public ArrayList<Long> add_buf_list;
    public ArrayList<Long> state_sfx_list;
    public ArrayList<Long> end_sfx_list;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
