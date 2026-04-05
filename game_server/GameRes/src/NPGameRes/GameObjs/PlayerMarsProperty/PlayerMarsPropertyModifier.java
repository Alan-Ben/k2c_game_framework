package NPGameRes.GameObjs.PlayerMarsProperty;

import Common.MarsEnum.EMarsPropertyType;
import NPCommon.Property._ATNPBasicPropertyModifier;

public class PlayerMarsPropertyModifier extends _ATNPBasicPropertyModifier<EMarsPropertyType, PlayerMarsPropertyModifier>
{
    public PlayerMarsPropertyModifier()
    {
        super(EMarsPropertyType.class);
    }

    /************
     * 创建一个编辑器对象
     * @return
     */
    @Override
    protected PlayerMarsPropertyModifier _createModifier()
    {
        return new PlayerMarsPropertyModifier();
    }
}