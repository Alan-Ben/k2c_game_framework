package MGClient.ClientPlayer.PlayerComp.Comps;


import CommonEnum.ECurrency;
import MGClient.ClientPlayer.ClientPlayer;
import MGClient.ClientPlayer.PlayerComp._AClientComponentBase;
import NPCommon.Enum.NPCommonEnum;

public class ClientCurrencyComp extends _AClientComponentBase
{

    private Long[] _m_alCurrencyList = new Long[ECurrency.ECurrency_Length];

    public ClientCurrencyComp(ClientPlayer _player)
    {
        super(_player, NPCommonEnum.ENPPlayerCompType.CURRENCY_COMP);
    }


    public long getCurrency(int _iCurrency)
    {
        return _m_alCurrencyList[_iCurrency] == null ? 0 : _m_alCurrencyList[_iCurrency];
    }

}
