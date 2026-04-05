package NPCommon.DB.BM;

import ALServerLog.ALServerLog;
import NPCommon.DB.IBaseBO;
import NPCommon.DB.WCGDBObj;
import NPCommon.DB._ISelectDBInterface;
import NPCommon.Enum.NPCommonEnum;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/***********
 * 本对象不再是单例，而是每个服务器需要各自声明的对象
 */
public class BM
{
    //服务器创建的时候会带入本对象用于对不同的DB进行操作
    private _ISelectDBInterface _m_sdiSelectDBInterface;

    /**********
     * 单例对象管理，根据类对对应的BM对象进行管理
     */
    private final Map<Class<? extends IBaseBO>, BMObj<?>> map = new ConcurrentHashMap<>();

    //构造带入处理对象
    public BM(_ISelectDBInterface _selectDBInterface)
    {
        _m_sdiSelectDBInterface = _selectDBInterface;
    }

    /**
     * 根据带入的类型获取真实的DBTag
     * @param _dbTag
     * @return
     */
    public NPCommonEnum.EDBTag getRealDBTag(NPCommonEnum.EDBTag _dbTag){return null == _m_sdiSelectDBInterface ? _dbTag : _m_sdiSelectDBInterface.switchDBTag(_dbTag);}

    /********
     * 初始化获取类处理对象的处理，这里会自动创建
     * @param classType
     * @return
     * @param <M>
     */
                                            @SuppressWarnings("unchecked")
    public <M extends IBaseBO> BMObj<M> initCheck(WCGDBObj _dbObj, Class<M> classType)
    {
        synchronized (map)
        {
            BMObj<M> boInfo = (BMObj<M>) map.get(classType);
            if (boInfo == null)
            {
                map.put(classType, boInfo = new BMObj<>(_dbObj, classType));
            }
            return boInfo;
        }
    }

    /********
     * 获取具体的处理对象
     * @param classType
     * @return
     * @param <M>
     */
    @SuppressWarnings("unchecked")
    public <M extends IBaseBO> BMObj<M> getBM(Class<M> classType)
    {
        synchronized (map)
        {
            BMObj<M> boInfo = (BMObj<M>) map.get(classType);
            if (boInfo == null)
            {
                //无数据对象，则日志输出错误
                ALServerLog.Error("BM.getBM BMObj is null, classType:" + classType.getName());
            }

            return boInfo;
        }
    }
}
