package NPGameRes.GameObjs.NPPlayerProperty;

import NPCommon.Property._ATNPBasicPropertyContainer;
import NPEnum.ENPPlayerPropertyType;

/************************
 * 属性容器对象
 **/
public class NPPlayerPropertyContainer extends _ATNPBasicPropertyContainer<ENPPlayerPropertyType, NPPlayerPropertyModifier, NPPlayerPropertyContainer>
{
    /**
     * 标识名，不涉及业务，用于检查，汇总到父类后，父类能知道是哪一部分的数据
     */
    private String _m_containerName;

    public NPPlayerPropertyContainer()
    {
        super(ENPPlayerPropertyType.class);
    }

    public NPPlayerPropertyContainer(String _containerName)
    {
        super(ENPPlayerPropertyType.class);
        _m_containerName = _containerName;
    }

    /************
     * 创建一个容器对象
     * @return
     */
    @Override
    public NPPlayerPropertyContainer _createContainer()
    {
        return new NPPlayerPropertyContainer(_m_containerName + "_copy");
    }

    @Override
    public String getName()
    {
        return _m_containerName;
    }
}