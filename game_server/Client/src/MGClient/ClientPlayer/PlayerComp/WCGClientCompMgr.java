package MGClient.ClientPlayer.PlayerComp;


import NPCommon.Enum.NPCommonEnum;

public class WCGClientCompMgr
{

    private _AClientComponentBase[] _m_comps = new _AClientComponentBase[NPCommonEnum.ENPPlayerCompType.values().length];

    public void registComp(_AClientComponentBase _clientComponentBase)
    {
        _m_comps[_clientComponentBase.getCompType().ordinal()] = _clientComponentBase;
    }

    _AClientComponentBase getComp(NPCommonEnum.ENPPlayerCompType eComp)
    {
        return _m_comps[eComp.ordinal()];
    }
}
