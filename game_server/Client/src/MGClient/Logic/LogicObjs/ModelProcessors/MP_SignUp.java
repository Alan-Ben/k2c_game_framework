package MGClient.Logic.LogicObjs.ModelProcessors;

import MGClient.ClientPlayer.ClientPlayer;

public class MP_SignUp extends ModelProcesorBase
{

    private int _m_tickCount = 1;

    @Override
    public void process(ClientPlayer _player, int tickCount)
    {

        if (--_m_tickCount <= 0)
            setDone();

    }

    @Override
    public void onStart(ClientPlayer _player)
    {
        // Common_HeroInfo  hero =_player.getCompHero().randHero();
        // _player.runGm("war _1sign");
    }

}