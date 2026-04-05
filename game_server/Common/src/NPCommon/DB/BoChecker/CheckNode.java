package NPCommon.DB.BoChecker;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;

public class CheckNode
{
    public BM BMObj;
    public BaseBO bo; //数据库Bo对象
    public long serial;//事务序列号
    public long addTime; //添加时间
}
