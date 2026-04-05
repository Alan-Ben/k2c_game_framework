package NPCommon.Property;

import ALBasicServer.ALTask._IALSynTask;

/***************
 * 属性容器的属性变化的处理任务
 */
public class SyncContainerPropertyChgTask<E extends Enum<E>> implements _IALSynTask
{
    private _ATNPBasicPropertyContainer _m_container;
    //变化的属性类型
    private E _m_eChgPropertyType;

    public SyncContainerPropertyChgTask(_ATNPBasicPropertyContainer _container, E _propertType)
    {
        _m_container = _container;
        _m_eChgPropertyType = _propertType;
    }

    //调用回调处理
    public void run()
    {
        //调用函数处理
        _m_container._onPropertyChg(_m_eChgPropertyType);
    }
}
