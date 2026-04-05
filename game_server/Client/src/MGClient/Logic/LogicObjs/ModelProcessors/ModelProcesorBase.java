package MGClient.Logic.LogicObjs.ModelProcessors;

import MGClient.ClientPlayer.ClientPlayer;

public abstract class ModelProcesorBase
{

    private boolean _m_bIsDone = false;

    public abstract void process(ClientPlayer _player, int tickCount);

    public abstract void onStart(ClientPlayer _player);

    public boolean isDone()
    {
        return _m_bIsDone;
    }

    public void setDone()
    {
        _m_bIsDone = true;
    }


}
