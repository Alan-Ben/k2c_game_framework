package MGClient.ClientPlayer.PlayerComp;

import MGClient.ClientPlayer.ClientPlayer;
import NPCommon.Enum.NPCommonEnum;


public class _AClientComponentBase
{
    protected ClientPlayer _m_owner;
    //是否已经开始加载

    private NPCommonEnum.ENPPlayerCompType _m_eCompType;

    protected _AClientComponentBase(ClientPlayer _player, NPCommonEnum.ENPPlayerCompType _eComponent)
    {
        _m_eCompType = _eComponent;
        _m_owner = _player;
        _m_owner.registComp(this);
    }

    NPCommonEnum.ENPPlayerCompType getCompType()
    {
        return _m_eCompType;
    }
}
