package NPGameRes.GameObjs.NPActorProperty;

import NPCommon.Enum.NPCommonEnum.ENPPropertyType;
import NPCommon.Property._ATNPBasicPropertyContainer;


/************************
 * 属性容器对象
 **/
public class NPPropertyContainer extends _ATNPBasicPropertyContainer<ENPPropertyType, NPPropertyModifier, NPPropertyContainer>
{
    /**
     * 标识名，不涉及业务，用于检查，汇总到父类后，父类能知道是哪一部分的数据
     */
    private String _m_containerName = "";

    public NPPropertyContainer()
    {
        super(ENPPropertyType.class);
    }

    public NPPropertyContainer(String _containerName)
    {
        super(ENPPropertyType.class);
        _m_containerName = _containerName;
    }

    /************
     * 创建一个容器对象
     * @return
     */
    @Override
    public NPPropertyContainer _createContainer()
    {
        return new NPPropertyContainer(_m_containerName + "copy");
    }

    @Override
    public String getName()
    {
        return _m_containerName;
    }
}