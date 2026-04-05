package NPGameRes.GameObjs.PlayerMarsProperty;

import Common.MarsEnum.EMarsPropertyType;
import NPCommon.Property._ATNPBasicPropertyContainer;

/************************
 * 属性容器对象
 **/
public class PlayerMarsPropertyContainer extends _ATNPBasicPropertyContainer<EMarsPropertyType, PlayerMarsPropertyModifier, PlayerMarsPropertyContainer>
{
    /**
     * 标识名，不涉及业务，用于检查，汇总到父类后，父类能知道是哪一部分的数据
     */
    private String _m_containerName;

    public PlayerMarsPropertyContainer()
    {
        super(EMarsPropertyType.class);
    }

    public PlayerMarsPropertyContainer(String _containerName)
    {
        super(EMarsPropertyType.class);
        _m_containerName = _containerName;
    }

    /************
     * 创建一个容器对象
     * @return
     */
    @Override
    public PlayerMarsPropertyContainer _createContainer()
    {
        return new PlayerMarsPropertyContainer(_m_containerName + "_copy");
    }

    @Override
    public String getName()
    {
        return _m_containerName;
    }
}