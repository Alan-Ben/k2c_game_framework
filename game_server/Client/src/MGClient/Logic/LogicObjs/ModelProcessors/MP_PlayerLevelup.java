package MGClient.Logic.LogicObjs.ModelProcessors;

import MGClient.ClientPlayer.ClientPlayer;

public class MP_PlayerLevelup extends ModelProcesorBase
{

    private int upCount = 0;

    @Override
    public void process(ClientPlayer _player, int tickCount)
    {
        if (upCount < 3)
        {
            _player.runCmd("player lvlup ");
            upCount++;
        } else
            setDone();

    }

    @Override
    public void onStart(ClientPlayer _player)
    {

    }

}