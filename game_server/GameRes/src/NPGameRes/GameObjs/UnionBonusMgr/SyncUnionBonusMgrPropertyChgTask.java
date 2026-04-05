package NPGameRes.GameObjs.UnionBonusMgr;

import ALBasicServer.ALTask._IALSynTask;
import CommonEnum.EBonusPropertyType;

/***************
 * 属性容器的属性变化的处理任务
 */
public class SyncUnionBonusMgrPropertyChgTask implements _IALSynTask
{
    private UnionBonusMgr _m_mgr;
    //变化的属性类型
    private EBonusPropertyType _m_eChgPropertyType;

    public SyncUnionBonusMgrPropertyChgTask(UnionBonusMgr _mgr, EBonusPropertyType _propertType)
    {
        _m_mgr = _mgr;
        _m_eChgPropertyType = _propertType;
    }

    //调用回调处理
    public void run()
    {
        //调用函数处理
        _m_mgr._onPropertyChg(_m_eChgPropertyType);
    }
}
