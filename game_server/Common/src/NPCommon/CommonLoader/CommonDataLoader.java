package NPCommon.CommonLoader;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;

public class CommonDataLoader<T>
{
    private boolean _m_bInited = false;
    private T _m_data =null;
    private CommonLoader<T> _m_loader = new CommonLoader<>();
    private MutexAtom _m_locker = new MutexAtom();

    public T getData(){ return _m_data;}
    public boolean isInited(){ return _m_bInited;}

    public void loadData(_ILoader<T> _loader, HandlerOne<Result>  _handler)
    {
        HandlerOne<Result> immHandler =null;
        _m_locker.lock();
        try
        {
            if(_m_bInited)
            {
                immHandler = _handler;
            }
        }finally
        {
            _m_locker.unlock();
        } 
        if (null != immHandler)
        {
            immHandler.handle(Result.SUCC);
            return;
        }

        _m_loader.asyncLoad(_loader, new HandlerTwo<Result, T>() {
            @Override
            public void handle(Result result,T _boList)
            {
                _m_locker.lock();
                try
                {
                    if (result.isSucc())
                    {
                        _m_bInited =true;
                        _m_data = _boList;
                    }
                }finally
                {
                    _m_locker.unlock();
                }
                _handler.handle(result);
            }
        });
            
    
    }
}
