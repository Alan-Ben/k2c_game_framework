package MGClient.ClientPlayer.PlayerComp.Comps;


import MGClient.ClientPlayer.ClientPlayer;
import MGClient.ClientPlayer.PlayerComp._AClientComponentBase;
import NPCommon.Enum.NPCommonEnum;

public class ClientBagComp extends _AClientComponentBase
{


    public ClientBagComp(ClientPlayer _player)
    {
        super(_player, NPCommonEnum.ENPPlayerCompType.BAG_ITEM);
    }


}
