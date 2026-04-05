package NPCommon.SerialLoader;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;
import NPCommon.Util.Pair.WCGPair;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;

/********
 * 顺序异步加载管理器，用来在服务器启动的时候按顺序异步加载一些数据，可能是从数据库中读取，也可能是从其它服务器拉取
 */
public abstract class _AAsyncSerialLoadMgr
{
	//操作序列号，避免 PS 重启 引发重新加载引发计数_m_curIndex错误
	private long _m_lOpSerialize;
	
    private final List<_IAllLoadOverHandler> _m_calls = new ArrayList<>(); //缓存全部加载成功后处理的回调函数列表
    private List<_IAsyncSingleLoader> _m_lLoaders = new ArrayList<>(); //全部加载器列表
    private AtomicInteger _m_curIndex = new AtomicInteger(0); //当前加载器的索引
    
    //加载完成后安全的调用
    public void safeCall(final _IAllLoadOverHandler _handler)
    {
        if (!isAllLoad())
        {
            synchronized (_m_calls)
            {
                _m_calls.add(_handler);
            }
            return;
        } else
        {
            _handler.onAllLoadOver();
        }
    }

    /*******
     * 注册一个加载器
     * @param _loader
     */
    public void registLoader(_IAsyncSingleLoader _loader)
    {
        _m_lLoaders.add(_loader);
    }

    /*******
     * 开始加载
     */
    public synchronized void startAsyncLoad()
    {
        if (_m_lLoaders.isEmpty())
            return;
        
        //获取新操作序列号
        _m_lOpSerialize = ALSerializeMaker.makeNewSerialize();
        
        //重置标志位
        _m_curIndex.set(0);
        //开启加载
        _startSingleLoad(_m_lOpSerialize);
    }

    /*****
     * 开始加载单个加载器，失败后3秒重试
     */
    private void _startSingleLoad(long _opSerial)
    {	
        _IAsyncSingleLoader loader = _m_lLoaders.get(_m_curIndex.get());
        CommLog.info("--------start exec loading for loader:{} serial:{} ......", loader.getClass().getSimpleName(), _opSerial);

        //开启任务监控，3秒后检测Loader是否有回调，没有回调则表示被卡住了，输出日志报警
        final WCGPair<_IAsyncSingleLoader, Boolean> pairFlag = new WCGPair<>(loader, false);
        ALSynTaskManager.getInstance().regTask(() ->
        {
            if (!pairFlag.second)
            {
                CommLog.error("Loader:{} Serial:{} stuck .............", loader.getClass().getName(), _opSerial);
            }

        }, 3000);

        //开始加载
        loader.asyncLoad(isSuccess ->
        {
        	//检查当前序列号
        	if(_opSerial != _m_lOpSerialize)
        	{
        		CommLog.warn("-------- asyncLoad load loader:{} serial:{} curSerial:{} stop......", loader.getClass().getSimpleName(), _opSerial, _m_lOpSerialize);
        		return;
        	}
        	
            pairFlag.second = true;

            if (isSuccess)
            {//加载成功
                CommLog.info("--------load success for loader:{} serial:{}", loader.getClass().getSimpleName(), _opSerial);
                _m_curIndex.incrementAndGet();
                if (_m_curIndex.get() == _m_lLoaders.size())
                {//全部加装完毕了
                	CommLog.info("--------all loader over serial:{} size:{}", _opSerial, _m_lLoaders.size());
                    ALSynTaskManager.getInstance().regTask(() -> _onAllLoadOver());
                } else
                {//还有加载器未完成，继续下一个加载任务
                    _startSingleLoad(_opSerial);
                }
            } else
            {//加载失败

                CommLog.error("loader:{} serial:{} load failed ,try in next 3 sec!", loader.getClass().getName(), _opSerial);
                //3秒后重试
                ALSynTaskManager.getInstance().regTask(() -> _startSingleLoad(_opSerial), 3000);
            }
        });

    }


    /****
     * 是否全部加载完毕
     * @return
     */
    public boolean isAllLoad()
    {
        return _m_curIndex.get() == _m_lLoaders.size();
    }

    /*******
     * 全部加载后调用
     */
    private void _onAllLoadOver()
    {
        //全部成功后的回调函数
        List<_IAllLoadOverHandler> calls = null;
        synchronized (_m_calls)
        {
            if (!_m_calls.isEmpty())
            {
                calls = new ArrayList<>(_m_calls);
                _m_calls.clear();
            }
        }
        if (null != calls)
        {
            for (_IAllLoadOverHandler _handler : calls)
            {
                try
                {
                    _handler.onAllLoadOver();
                } catch (Throwable e)
                {
                    CommLog.error("server deal loadover handler faild! [e:{}]", e);
                }
            }

        }
        CommLog.info("======================All server loader done===================");
    }
}
