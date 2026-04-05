package NPGameRes.GameObjs.PlayerBonusProperty;

import CommonEnum.EBonusPropertyType;
import NPCommon.Property._ATNPBasicPropertyContainer;

/************************
 * 属性容器对象
 **/
public class PlayerBonusPropertyContainer extends _ATNPBasicPropertyContainer<EBonusPropertyType, PlayerBonusPropertyModifier, PlayerBonusPropertyContainer>
{
    /**
     * 标识名，不涉及业务，用于检查，汇总到父类后，父类能知道是哪一部分的数据
     */
    private String _m_containerName;

    public PlayerBonusPropertyContainer()
    {
        super(EBonusPropertyType.class);
    }

    public PlayerBonusPropertyContainer(String _containerName)
    {
        super(EBonusPropertyType.class);
        _m_containerName = _containerName;
    }

    /************
     * 创建一个容器对象
     * @return
     */
    @Override
    public PlayerBonusPropertyContainer _createContainer()
    {
        return new PlayerBonusPropertyContainer(_m_containerName + "_copy");
    }

    @Override
    public String getName()
    {
        return _m_containerName;
    }
}