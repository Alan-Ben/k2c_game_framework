package NPGameRes.GameObjs.PlayerAttrProperty;

import CommonEnum.EBasicAttrType;
import NPCommon.Property._ATNPBasicPropertyContainer;

/************************
 * 属性容器对象
 **/
public class PlayerAttrPropertyContainer extends _ATNPBasicPropertyContainer<EBasicAttrType, PlayerAttrPropertyModifier, PlayerAttrPropertyContainer>
{
    /**
     * 标识名，不涉及业务，用于检查，汇总到父类后，父类能知道是哪一部分的数据
     */
    private String _m_containerName;

    public PlayerAttrPropertyContainer()
    {
        super(EBasicAttrType.class);
    }

    public PlayerAttrPropertyContainer(String _containerName)
    {
        super(EBasicAttrType.class);
        _m_containerName = _containerName;
    }

    /************
     * 创建一个容器对象
     * @return
     */
    @Override
    public PlayerAttrPropertyContainer _createContainer()
    {
        return new PlayerAttrPropertyContainer(_m_containerName + "_copy");
    }

    @Override
    public String getName()
    {
        return _m_containerName;
    }
    
    /**
     * 获取玩家属性总和
     * 
     * 只计算如下属性总和
     		STR, //1 ==== 武力
     		INT, //2 ==== 智力
     		POL, //3 ==== 政治
     		LEAD, //4 ==== 统帅
     * 
     * @return
     */
    public long getPlayerAtrrSum()
    {
        return getValue(EBasicAttrType.POWER);
    }
}