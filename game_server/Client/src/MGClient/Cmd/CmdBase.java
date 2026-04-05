package MGClient.Cmd;

import MGClient.ClientPlayer.ClientPlayer;

public class CmdBase
{
    private ClientPlayer m_pPlayer;

    public void setOwner(ClientPlayer player)
    {
        m_pPlayer = player;

    }

    public ClientPlayer getOwner()
    {
        return m_pPlayer;
    }
}
