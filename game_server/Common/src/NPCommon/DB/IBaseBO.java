package NPCommon.DB;

import ALMySqlCommon.ALMySqlCommonObj._IALMySqlBaseDBBO;
import NPCommon.Enum.NPCommonEnum;

import java.sql.ResultSet;
import java.util.List;

/**
 * 基本VO类，使用于JDBC访问数据库时的基本VO类
 * <p>
 * 主要实现了方法getFromResultSet 在PUBLICDAO中需要根据该方法实现冲JDBC中返回LIST
 * <p>
 * 作者：alzq
 */
public interface IBaseBO extends _IALMySqlBaseDBBO
{

    /**
     * 以上2项主要是提供给JDBC调用后可以返回正确的值，这样不需要用户去重复编写
     */
    /**
     * 本方法从ResultSet中提取相关信息，并添加到List中<br>
     * 主要用于JDBC的访问，一般根据自行设定好的顺序进行数据提取<br>
     * 也可以使用字段名，不过字段名消耗较多资源<br>
     * 需要在子类别中重定义<br>
     * @param rs
     * @param list
     * @throws java.lang.Exception
     */
    @SuppressWarnings("rawtypes")
    public void getFromResultSet(ResultSet rs, List list) throws Exception;

    /**
     * ************ 用于选取VO数据的数据项字符串
     * @return
     */
    public String getItemsName();

    /**
     * ************* 用于选取VO数据的表名集合
     * @return
     */
    public String getTbName();

    /**
     * ************* 用于获取插入数据时的值列表
     * @return
     */
    public String getItemsValue();

    /**
     * 用于设置每条记录的主键ID
     * @param iID
     */
    public void setId(long iID);

    /**
     * 用于获取每条记录的主键ID
     * @return
     */
    public long getId();


    public String getTableName();

    public NPCommonEnum.EDBTag getDBTag();
}
