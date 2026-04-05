package NPCommon.DB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;

import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;


public class WCGDBFactory
{

    private static List<_TALMySqlSafeOpDBObj<WCGDBObj>> _g_DBList = new CopyOnWriteArrayList<>();

    public static boolean regDBObj(WCGDBObj _dbObj)
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = null;
        for (int i = 0; i < _g_DBList.size(); i++)
        {
            tmpObj = _g_DBList.get(i);
            if (null == tmpObj)
                continue;

            if (_dbObj == tmpObj.getDB())
            {
                CommLog.fatal("duplciate reg db obj of tag:{}", tmpObj.getDB().getDbTag());
                System.exit(1);
            }

            if (tmpObj.getDB().getDbTag() == _dbObj.getDbTag())
            {
                CommLog.fatal("duplciate reg2 db obj of tag:{}", tmpObj.getDB().getDbTag());
                System.exit(1);
            }
        }

        //构建新对象
        _TALMySqlSafeOpDBObj<WCGDBObj> safeDBObj = new _TALMySqlSafeOpDBObj<WCGDBObj>(_dbObj, _dbObj.getTaskIndex(), _dbObj.getSafeOpSavePath());
        //尝试初始化数据库保存机制
        if (!safeDBObj.init())
        {
            ALServerLog.Error("Init Safe OP DB: " + _dbObj.getDBName() + " Fail!!");
            return false;
        }

        //加入队列
        _g_DBList.add(safeDBObj);

        return true;
    }

    public static _TALMySqlSafeOpDBObj<WCGDBObj> getDbObj(NPCommonEnum.EDBTag _sDBTag)
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = null;
        for (int i = 0; i < _g_DBList.size(); i++)
        {
            tmpObj = _g_DBList.get(i);
            if (null == tmpObj)
                continue;

            if (tmpObj.getDB().getDbTag() == _sDBTag)
            {
                return tmpObj;
            }
        }
        CommLog.error("Can not find db obj of tag: " + _sDBTag);
        return null;
    }

    public static List<_TALMySqlSafeOpDBObj<WCGDBObj>> getDBObjects()
    {
        return _g_DBList;
    }
}
