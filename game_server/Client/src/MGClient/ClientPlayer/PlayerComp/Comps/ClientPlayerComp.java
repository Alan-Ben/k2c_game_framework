package MGClient.ClientPlayer.PlayerComp.Comps;

import MGClient.ClientPlayer.ClientPlayer;
import MGClient.ClientPlayer.PlayerComp._AClientComponentBase;
import NPCommon.Enum.NPCommonEnum;

public class ClientPlayerComp extends _AClientComponentBase
{
    private String cname;

    public ClientPlayerComp(ClientPlayer _player)
    {
        super(_player, NPCommonEnum.ENPPlayerCompType.PLAYER_COMP);
    }

    public String getName()
    {
        return cname;
    }

}
