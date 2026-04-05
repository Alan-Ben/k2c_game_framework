package NPCommon.CommonMsg;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.CallBack._ICallBackT;

import java.util.ArrayList;
import java.util.Comparator;

@SuppressWarnings("rawtypes")
public abstract class ComMsgMgrBase<T extends ComMsgBase, L extends ComMsgLoaderBase>
{

    private int _m_cacheMaxNum;
    private MutexAtom _m_lock;
    private ArrayList<T> _m_cacheList;

    public ComMsgMgrBase()
    {
        _m_lock = new MutexAtom();
        _m_cacheList = new ArrayList<T>(_m_cacheMaxNum);
        _m_cacheMaxNum = Math.min(setMaxNum(), 500);
    }

    public abstract int setMaxNum();

    public long getNewMsgInfoId()
    {
        return _m_cacheList.isEmpty() ? 0 : _m_cacheList.get(0).getMsgSerial();
    }

    public void init()
    {
        _lock();
        try
        {
            load(_m_cacheMaxNum, list ->
            {
                _m_cacheList.addAll(list);
            });
        } finally
        {
            _unlock();
        }
    }

    public void get(long _msgSerial, int _num, _ICallBackT<ArrayList<T>> _handle)
    {
        ArrayList<T> msgInfos = new ArrayList<>();
        _lock();
        try
        {
            //如果序列号为-1，从内存最新的id + 1开始处理
            if (_msgSerial <= 0)
            {
                _msgSerial = getNewMsgInfoId() + 1;
            }

            //先处理内存数据
            for (T msgInfo : _m_cacheList)
            {
                if (_num <= 0)
                {
                    break;
                }
                if (_msgSerial > msgInfo.getMsgSerial())
                {
                    msgInfos.add(msgInfo);

                    _num--;
                    _msgSerial = msgInfo.getMsgSerial();
                }
            }
            if (_num > 0 && _msgSerial > getOverId())
            {
                load(_msgSerial, _num, list ->
                {
                    msgInfos.addAll(list);
                    msgInfos.sort(sort());
                    _handle.onRunOver(msgInfos);
                });
            } else
            {
                _handle.onRunOver(msgInfos);
            }
        } finally
        {
            _unlock();
        }
    }

    public void add(T _t)
    {
        _lock();
        try
        {
            _m_cacheList.add(_t);

        } finally
        {
            _unlock();
        }
    }

    public void add1(T _t)
    {
        _lock();
        try
        {
            _m_cacheList.add(_t);
            addDb(_t);

        } finally
        {
            _unlock();
        }
    }

    public abstract void addDb(T _t);

    public abstract void load(long _msgSerial, int _num, _ICallBackT<ArrayList<T>> _handle);

    public abstract void load(int _num, _ICallBackT<ArrayList<T>> _handle);

    public abstract Comparator<? super T> sort();

    public abstract long getOverId();

    private void _lock()
    {
        _m_lock.lock();
    }

    private void _unlock()
    {
        _m_lock.unlock();
    }

    public void discard(long _msgSerial, int _num)
    {
        boolean isRemove = false;
        _lock();
        try
        {
            for (int i = 0; i < _m_cacheList.size() && _num > 0; i++)
            {
                if (_msgSerial == _m_cacheList.get(i).getMsgSerial())
                {
                    isRemove = true;
                }
                if (isRemove)
                {
                    _m_cacheList.remove(i);
                    _num--;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    public void discardAll()
    {
        _lock();
        try
        {
            _m_cacheList.clear();
        } finally
        {
            _unlock();
        }
    }

    public abstract void onDiscard();

    public abstract void onDiscardDb();

}
