package MGClient.Logic.LogicObjs.ModelProcessors;

import MGClient.ClientPlayer.ClientPlayer;

public class MP_sendHelpConscript extends ModelProcesorBase
{
    private int _m_tickCount = 20;

    @Override
    public void process(ClientPlayer _player, int tickCount)
    {

        if (--_m_tickCount <= 0)
            setDone();
    }

    @Override
    public void onStart(ClientPlayer _player)
    {
        _player.runCmd("Conscript sendHelp");

    }
}
