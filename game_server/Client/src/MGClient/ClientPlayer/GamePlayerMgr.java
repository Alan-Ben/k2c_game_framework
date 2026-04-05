package MGClient.ClientPlayer;

import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.ADelegateOne;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;

import java.util.ArrayList;
import java.util.concurrent.locks.ReentrantLock;

public class GamePlayerMgr implements _IHandlerHolder
{
    private static GamePlayerMgr _instance = new GamePlayerMgr();

    public ADelegateOne<ClientPlayer> onPlayerLoginGame = new ADelegateOne<>(this);
    public ADelegateOne<ClientPlayer> onPlayerLogouted = new ADelegateOne<>(this);
    private ReentrantLock _m_mutex = null;

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    private GamePlayerMgr()
    {
        _m_mutex = new ReentrantLock();
    }

    public static GamePlayerMgr getInstance()
    {
        return _instance;
    }

    private ArrayList<ClientPlayer> mPlayers = new ArrayList<ClientPlayer>();

    public void addLoginedPlayer(ClientPlayer _clientPlayer)
    {
        _lock();
        try
        {
            mPlayers.add(_clientPlayer);
            onPlayerLoginGame.onEvent(_clientPlayer);
            _clientPlayer.OnPlayerOffLine.addHandler(this, new HandlerOne<ClientPlayer>()
            {
                @Override
                public void handle(ClientPlayer _player)
                {
                    onPlayerLogouted.onEvent(_player);
                    mPlayers.remove(_player);
                    CommLog.info("player count:{}", mPlayers.size());

                }
            });
        } finally
        {
            _unlock();
        }

    }

    public ClientPlayer lookupPlayerById(long uid)
    {
        _lock();
        try
        {
            for (int i = 0; i < mPlayers.size(); i++)
            {
                if (mPlayers.get(i).getCid() == uid)
                    return mPlayers.get(i);
            }
        } finally
        {
            _unlock();
        }
        return null;

    }

    public ClientPlayer lookupPlayerByName(String name)
    {
        _lock();
        try
        {
            for (int i = 0; i < mPlayers.size(); i++)
            {
                if (mPlayers.get(i).getName().compareToIgnoreCase(name) == 0)
                    return mPlayers.get(i);
            }
        } finally
        {
            _unlock();
        }
        return null;
    }

    public void changeLogic(String logicName)
    {
        _lock();
        try
        {
            for (int i = 0; i < mPlayers.size(); i++)
            {
                mPlayers.get(i).changeLogic(logicName);
            }
        } finally
        {
            _unlock();
        }
    }

    public void runGM(String gmCommand)
    {
        _lock();
        try
        {
            for (int i = 0; i < mPlayers.size(); i++)
            {
                mPlayers.get(i).runGm(gmCommand);
            }
        } finally
        {
            _unlock();
        }
    }

    public void runCMD(String gmCommand)
    {
        _lock();
        try
        {
            for (int i = 0; i < mPlayers.size(); i++)
            {
                mPlayers.get(i).runCmd(gmCommand);
            }
        } finally
        {
            _unlock();
        }
    }

    public void getAllPlayers(ArrayList<ClientPlayer> allPlayers)
    {
        _lock();
        try
        {
            allPlayers.addAll(mPlayers);
        } finally
        {
            _unlock();
        }
    }

    public void clear()
    {
        _lock();
        try
        {
            mPlayers.clear();
        } finally
        {
            _unlock();
        }
    }

    public int getPlayerNum()
    {
        _lock();
        try
        {
            return mPlayers.size();
        } finally
        {
            _unlock();
        }
    }

}
