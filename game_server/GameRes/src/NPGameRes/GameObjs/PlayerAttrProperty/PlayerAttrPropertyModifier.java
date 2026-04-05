package NPGameRes.GameObjs.PlayerAttrProperty;

import CommonEnum.EBasicAttrType;
import NPCommon.Property._ATNPBasicPropertyModifier;

public class PlayerAttrPropertyModifier extends _ATNPBasicPropertyModifier<EBasicAttrType, PlayerAttrPropertyModifier>
{
    public PlayerAttrPropertyModifier()
    {
        super(EBasicAttrType.class);
    }

    /************
     * 创建一个编辑器对象
     * @return
     */
    @Override
    protected PlayerAttrPropertyModifier _createModifier()
    {
        return new PlayerAttrPropertyModifier();
    }
}