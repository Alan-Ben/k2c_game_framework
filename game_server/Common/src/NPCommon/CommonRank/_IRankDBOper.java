package NPCommon.CommonRank;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import NPCommon.DB.WCGDBObj;

/*******************
 * 排行数据库处理对象的接口类
 * @author mj
 *
 */
public interface _IRankDBOper
{

    /************
     * 获取数据库操作对象
     * @return
     */
    public abstract int getDBTaskThreadIndex();//获取数据库操作的线程索引

    public abstract _TALMySqlSafeOpDBObj<WCGDBObj> getDBObj();
    
    //数据库相关操作名称
    //rank主要信息表数据
    public abstract String getRankInfoTableName();

    //rank内一级数据相关信息
    public abstract String getRankObjTableName();
    public abstract String getRankObjScoreSourceIdName();
    public abstract String getRankObjScoreName();
    public abstract String getRankObjUpdatedMsName();

    //rank内二级数据相关信息
    public abstract String getRankSubObjTableName();
    public abstract String getRankSubObjScoreSourceIdName();
    public abstract String getRankSubObjScoreName();
    public abstract String getRankSubObjUpdatedMsName();
}
