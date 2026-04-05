package NPCommon.DB.Version;

import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALBasicServer.ALTask._IALAsynCallBackTask;
import ALBasicServer.ALTask._IALAsynCallTask;
import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import ALServerLog.ALServerLog;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.DB.BM.BM;
import NPCommon.DB.BM.BMObj;
import NPCommon.DB.BaseBO;
import NPCommon.DB.SQLUtil;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._IRunCallBack;
import NPCommon.Util.CommClass;

import java.lang.reflect.Field;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.*;
import java.util.Map.Entry;
import java.util.concurrent.ConcurrentHashMap;

/**
 * The implementation class of DB version manager and automatic updates
 * @author Aaron
 */
public class WCGDBVersionChecker extends AbstractDBVersionChecker
{

    // true when be initialized
    private boolean m_bInitializeOk = false;
    // the newest db version
    private String m_newestVersion = "1.0.0.1";
    // the bo of version info's record
    private DBVersionInfoBO m_versionBO = new DBVersionInfoBO();

    private BM _m_bm;
    private WCGDBObj _m_dbObj;

    /**
     * constructor
     */
    public WCGDBVersionChecker(BM _bm, WCGDBObj _obj)
    {
        _m_bm = _bm;
        _m_dbObj = _obj;
    }

    @Override
    public WCGDBObj getDBObj()
    {
        return _m_dbObj;
    }

    @SuppressWarnings("unchecked")
    @Override
    protected boolean initCurrentVersion()
    {
        if (m_bInitializeOk)
        {
            return true;
        }

        if (getDBObj() == null)
        {
            this._onError("initialize: db connection info has not been assigned!!!");
            return false;
        }

        //ensure version table
        String sql = String.format("show tables like '%s'", m_versionBO.getTbName());
        sql = sql.replace("`", "");
        try
        {
            PreparedStatement pstm = getDBObj().getConn().prepareStatement(sql);
            ResultSet rs = pstm.executeQuery();
            if (rs.next())
            {

            } else
            {
                ALMySqlDBExcutor.execute(m_versionBO.createTableSql(), getDBObj());
            }

        } catch (Throwable e)
        {
            CommLog.error("Error in ensure version table: " + m_versionBO.getTbName());
            return false;
        }


        List<DBVersionInfoBO> resultList = null;
        sql = String.format("select * from  %s", m_versionBO.getTbName());

        resultList = ALMySqlDBExcutor.executeQuery(sql, getDBObj(), m_versionBO);
        if (resultList != null && resultList.size() > 0)
        {
            m_versionBO = resultList.get(0);
        } else
        {
            //默认设置Id为1，要不第一次开启会因为插入Id为0报错
            if (ALMySqlDBExcutor.InsertBO(getDBObj(), m_versionBO, m_versionBO.getInsertValueBytes()) == false)
            {
                this._onError("initialize: insert version bo failed!!!");
                return false;
            }
        }

        //初始化DB对象中的TbIdMgr对象
        _m_dbObj.initTbIdMgr(m_versionBO);

        //遍历每个关联数据表，初始化BM中归属本Tag的表信息

        this._clearLastError();
        m_bInitializeOk = true;
        return true;
    }

    @Override
    protected void _onError(String errorInfo)
    {
        this._setLastError(errorInfo);
        CommLog.error(errorInfo);
    }

    @Override
    protected void _onMsg(String msg)
    {
        CommLog.info(msg);
    }

    @Override
    public String getCurVersion()
    {
        return m_versionBO.getVersion();
    }

    @Override
    protected boolean _setCurVersion(String version)
    {
        ALMySqlDBConditionObj cnd = new ALMySqlDBConditionObj();
        cnd.setTablesName(m_versionBO.getTbName());
        cnd.addAndEquals("ID", m_versionBO.getId());
        int count = ALMySqlDBExcutor.updateByCondition(getDBObj(), cnd, "Version='" + version + "'");
        return 1 == count;
    }

    @Override
    public String getNewestVersion()
    {
        return m_newestVersion;
    }

    /**
     * 设置当前DB最新版本. 使用 0.0.0 格式 注意,必须在执行版本检查之前设置最新版本号
     * @param ver
     */
    public void setNewestVersion(String ver)
    {
        m_newestVersion = ver;
    }

    /**
     * 设定update类路径并执行自动更新
     * @param packagePath 版本更新实现类所在包名称
     * @return 出错时返回false
     */
    public boolean runAutoVersionUpdate(String packagePath)
    {
        regAllUpdate(packagePath);
        return this.run();
    }

    /**
     * 将指定包里面的所有BO注册到数据库中
     * @param packagePath BO所在的包
     * @return 出错时返回false
     */
    @SuppressWarnings("unchecked")
    public boolean checkAndUpdateVersion(String packagePath)
    {
        ALServerLog.Sys("Start check DB for server: " + _m_dbObj.getDbTag());

        Connection conn = null;
        Statement stmt = null;
        ResultSet rs = null;
        List<Class<? extends BaseBO>> clazz = null;

        try
        {
            conn = this.getDBObj().getConn();
            stmt = conn.createStatement();

            CommLog.warn("DB:[{}] Tag:[{}]==========开始检测数据库版本==============", getDBObj().getDBName(), getDBObj().getDbTag());

            // 读取当前所有的表
            List<String> tables = new ArrayList<String>();
            rs = stmt.executeQuery("show TABLES;");
            while (rs.next())
            {
                tables.add(rs.getString(1).toLowerCase());
            }

            List<Class<BaseBO>> tableToAdd = new ArrayList<>();
            List<String> fieldsToAdd = new ArrayList<String>();
            List<String> fieldsToUpdate = new ArrayList<String>();

            clazz = CommClass.getAllClassByInterface(BaseBO.class, packagePath);

            for (Class<? extends BaseBO> cs : clazz)
            {
                BaseBO bo = (BaseBO) CommClass.forName(cs.getName()).newInstance();
                String tableName = bo.getTbName().replace("`", "");
                if (!tables.contains(tableName.toLowerCase()))
                {
                    tableToAdd.add((Class<BaseBO>) bo.getClass());
                    continue;
                }
                String sql = "desc " + bo.getTbName() + ";";
                rs = stmt.executeQuery(sql);
                // <field, type> - 数据库中的字段
                Map<String, String> fieldsInDB = new HashMap<String, String>();
                while (rs.next())
                {
                    String field = rs.getString(1);
                    String type = rs.getString(2);
                    fieldsInDB.put(field, type);
                }
                // <field, type> - 代码中的字段
                Map<String, DataBaseField> fieldsInCode = new HashMap<>();
                for (Field curField : cs.getDeclaredFields())
                {
                    DataBaseField dbinfo = curField.getAnnotation(DataBaseField.class);
                    if (dbinfo == null)
                    {
                        continue;
                    }
                    if (dbinfo.size() == 0)
                    { // 非列表类型
                        fieldsInCode.put(curField.getName(), dbinfo);
                    } else
                    {// 列表类型
                        for (int i = 0; i < dbinfo.size(); ++i)
                        {
                            fieldsInCode.put(curField.getName() + "_" + i, dbinfo);
                        }
                    }
                }
                // 记录需要升级的字段
                for (Entry<String, DataBaseField> fieldInCode : fieldsInCode.entrySet())
                {
                    String fieldName = fieldInCode.getKey();
                    DataBaseField fieldInfo = fieldInCode.getValue();
                    if (!fieldsInDB.containsKey(fieldName))
                    {
                        fieldsToAdd.add(String.format("ALTER TABLE `%s` ADD `%s` %s;", tableName, fieldName,
                                getFieldInfo(fieldInfo)));
                    } else if (checkFieldToUpdate(fieldInfo.type(), fieldsInDB.get(fieldName)))
                    {
                        fieldsToUpdate.add(String.format("ALTER TABLE `%s` MODIFY COLUMN `%s`  %s ;", tableName,
                                fieldName, getFieldInfo(fieldInfo)));
                    }
                }
            }
            // 添加数据库表
            for (Class<BaseBO> table : tableToAdd)
            {
                CommLog.info("添加新增数据库表 ：{}", table.getSimpleName().replace("BO", ""));
                this.addTable(table);
            }
            // 添加字段
            if (!fieldsToAdd.isEmpty())
            {
                for (String fieldSql : fieldsToAdd)
                {
                    CommLog.info("添加字段, SQL：" + fieldSql);
                    conn.createStatement().execute(fieldSql);
                }
            }
            // 升级字段
            if (!fieldsToUpdate.isEmpty())
            {
                for (String fieldSql : fieldsToUpdate)
                {
                    CommLog.info("升级字段, SQL：" + fieldSql);
                    conn.createStatement().execute(fieldSql);
                }
            }
        } catch (Exception e)
        {
            CommLog.error("升级数据库失败，请确认数据库连接配置，原因" + e.getMessage(), e);
            System.exit(-1);
        }finally
        {
            SQLUtil.close(rs, stmt, conn);
        }

        try
        {
            //逐个初始化ID
            for (Class<? extends BaseBO> cs : clazz) {
                //初始化BM对象
                BMObj<? extends BaseBO> bm = _m_bm.initCheck(_m_dbObj, cs);

                //处理检查
                if (!bm.s_checkDBId()) {
                    ALServerLog.Fatal("Init Check DB ID Table: [" + cs.getName() + "] fail!!");
                }
            }
        }catch (Throwable e)
        {
            CommLog.error("初始化数据库最大字段失败，请确认数据库连接配置，原因" + e.getMessage(), e);
            return  false;
        }

        CommLog.warn("DB:[{}] Tag:[{}]==========数据库版本检测完毕==============", getDBObj().getDBName(), getDBObj().getDbTag());
        return true;
    }

    private String getFieldInfo(DataBaseField dbinfo)
    {
        Object defaultValue = "";
        String type = dbinfo.type();
        if (type.startsWith("blob") || type.startsWith("text"))
        {
            return String.format("%s NULL COMMENT '%s'", type, dbinfo.comment());
        } else if (type.startsWith("timestamp"))
        {
            return String.format("%s NULL DEFAULT NULL COMMENT '%s'", type, dbinfo.comment());
        } else if (type.startsWith("int") || type.startsWith("smallint") || type.startsWith("tinyint") || type.startsWith("bigint"))
        {
            defaultValue = 0;
        } else if (type.startsWith("float"))
        {
            type = "float";
            defaultValue = 0;

        } else if (type.startsWith("double"))
        {
            type = "double";
            defaultValue = 0;

        } else if (type.startsWith("varchar"))
        {
            defaultValue = "";
        } else
        {
            defaultValue = "";
        }
        return String.format("%s NOT NULL DEFAULT '%s' COMMENT '%s'", type, defaultValue, dbinfo.comment());
    }

    private boolean checkFieldToUpdate(String fieldInCode, String fieldInDB)
    {
        if (fieldInCode.equals(fieldInDB))
        {
            return false;
        }

        // 检测Int类型
        List<String> intfields = Arrays.asList("tinyint(1)", "tinyint(2)", "tinyint(4)", "smallint(6)", "mediumint(9)",
                "int(11)", "bigint(20)");
        int idxIntCode = intfields.indexOf(fieldInCode);
        int idxIntDB = intfields.indexOf(fieldInDB);
        if (idxIntCode >= 0 && idxIntDB >= 0)
        {
            return idxIntCode > idxIntDB;
        }

        // 检测文本类型
        String sCodeTxtType = fieldInCode.startsWith("varchar") ? "varchar" : fieldInCode;
        String sDBTxtType = fieldInDB.startsWith("varchar") ? "varchar" : fieldInDB;
        // 检测varchar
        if (sDBTxtType.equals("varchar") && sCodeTxtType.equals("varchar"))
        {
            return Integer.valueOf(fieldInCode.split("\\W", 3)[1]) > Integer.valueOf(fieldInDB.split("\\W", 3)[1]);
        }
        List<String> textFields = Arrays.asList("varchar", "text", "mediumtext", "longtext");
        int idxTxtCode = textFields.indexOf(sCodeTxtType);
        int idxTxtDB = textFields.indexOf(sDBTxtType);
        if (idxTxtCode >= 0 && idxTxtDB >= 0)
        {
            return idxTxtCode > idxTxtDB;
        }
        return false;
    }

    protected void regAllUpdate(String path)
    {
        List<Class<?>> dealers = CommClass.getAllClassByInterface(IUpdateDBVersion.class, path);

        for (Class<?> cs : dealers)
        {
            IUpdateDBVersion dealer = null;
            try
            {
                dealer = (IUpdateDBVersion) CommClass.forName(cs.getName()).newInstance();
            } catch (InstantiationException | IllegalAccessException | ClassNotFoundException e)
            {
            }

            if (null == dealer)
            {
                continue;
            }

            regVersionUpdate(dealer);
        }
    }

    // =============== 通用的修改表结构接口 ===================
    public <T extends BaseBO> boolean addTable(Class<T> clz)
    {

        String tableName;
        String sql;

        try
        {
            T bo = clz.newInstance();
            tableName = (String) clz.getMethod("getTableName").invoke(bo);
            sql = (String) clz.getMethod("getSql_TableCreate").invoke(null);
        } catch (Throwable ex)
        {
            //
            return false;
        }

        if (!ALMySqlDBExcutor.execute(sql, getDBObj()))
        {
            CommLog.error(tableName + " Create Fail!");
            return false;
        }

        return true;
    }

    public boolean addIndex(String tableName, String index)
    {
        // info: INT(11) NOT NULL DEFAULT '0' COMMENT '注释'
        String sql = String.format("ALTER TABLE `%s` ADD INDEX `%s` (`%s`) USING BTREE ", tableName, index, index);
        if (!ALMySqlDBExcutor.execute(sql, getDBObj()))
        {
            CommLog.error("addIndex {}.{} Fail!{}sql:{}", tableName, index, System.lineSeparator(), sql);
            return false;
        }

        return true;
    }

    /*******
     * 异步检测数据库表版本
     * @param
     */
    public void checkAndUpdateVersionAsync(BM _bm, List<Class<? extends BaseBO>> _clazzList, _IRunCallBack _handler){
        ALAsynTaskManager.getInstance().regTask(getDBObj().getTaskIndex(), new _IALAsynCallTask<Integer>() {
            @Override
            public Integer dealAsynTask()
            {
                if(!checkAndUpdateVersion(_bm, _clazzList))
                    return null;
                return 1;
            }
        }, new _IALAsynCallBackTask<Integer>(){
            @Override
            public void dealSuc(Integer integer)
            {
                _handler.onRunOver(true,"ok");
            }

            @Override
            public void dealFail()
            {
                _handler.onRunOver(false,"checkAndUpdateVersion failed!");
            }
        });
    }
    /**********
     * 同步检测数据库表版本
     * @param _clazzList
     * @return
     */
    public boolean checkAndUpdateVersion(BM _bm, List<Class<? extends BaseBO>> _clazzList) {

        Connection conn = null;
        Statement stmt = null;
        ResultSet rs = null;

        try
        {
            CommLog.info("DB:[{}] Tag:[{}]==========开始检测数据库版本==============",getDBObj().getDBName(),getDBObj().getDbTag());

            conn = this.getDBObj().getConn();
            stmt = conn.createStatement();

            // 读取当前所有的表
            List<String> tables = new ArrayList<String>();
            rs = stmt.executeQuery("show TABLES;");
            while (rs.next())
            {
                tables.add(rs.getString(1).toLowerCase());
            }

            List<Class<? extends BaseBO>> tableToAdd = new ArrayList<>();
            List<String> fieldsToAdd = new ArrayList<String>();
            List<String> fieldsToUpdate = new ArrayList<String>();

            for (Class<? extends BaseBO> cs : _clazzList)
            {
                BaseBO bo = cs.newInstance();
                String tableName = bo.getTbName().replace("`", "");
                if (!tables.contains(tableName.toLowerCase()))
                {
                    tableToAdd.add(cs);
                    continue;
                }
                String sql = "desc " + bo.getTbName() + ";";
                rs = stmt.executeQuery(sql);
                // <field, type> - 数据库中的字段
                Map<String, String> fieldsInDB = new ConcurrentHashMap<String, String>();
                while (rs.next())
                {
                    String field = rs.getString(1);
                    String type = rs.getString(2);
                    fieldsInDB.put(field, type);
                }
                // <field, type> - 代码中的字段
                Map<String, DataBaseField> fieldsInCode = new ConcurrentHashMap<>();
                for (Field curField : cs.getDeclaredFields())
                {
                    DataBaseField dbinfo = curField.getAnnotation(DataBaseField.class);
                    if (dbinfo == null)
                    {
                        continue;
                    }
                    if (dbinfo.size() == 0)
                    { // 非列表类型
                        fieldsInCode.put(curField.getName(), dbinfo);
                    } else
                    {// 列表类型
                        for (int i = 0; i < dbinfo.size(); ++i)
                        {
                            fieldsInCode.put(curField.getName() + "_" + i, dbinfo);
                        }
                    }
                }
                // 记录需要升级的字段
                for (Entry<String, DataBaseField> fieldInCode : fieldsInCode.entrySet())
                {
                    String fieldName = fieldInCode.getKey();
                    DataBaseField fieldInfo = fieldInCode.getValue();
                    if (!fieldsInDB.containsKey(fieldName))
                    {
                        fieldsToAdd.add(String.format("ALTER TABLE `%s` ADD `%s` %s;", tableName, fieldName,
                                getFieldInfo(fieldInfo)));
                    } else if (checkFieldToUpdate(fieldInfo.type(), fieldsInDB.get(fieldName)))
                    {
                        fieldsToUpdate.add(String.format("ALTER TABLE `%s` MODIFY COLUMN `%s`  %s ;", tableName,
                                fieldName, getFieldInfo(fieldInfo)));
                    }
                }
            }
            // 添加数据库表
            for (Class<? extends BaseBO> table : tableToAdd)
            {
                CommLog.info("添加新增数据库表 ：{}", table.getSimpleName().replace("BO", ""));
                this.addTable(table);
            }
            // 添加字段
            if (!fieldsToAdd.isEmpty())
            {
                for (String fieldSql : fieldsToAdd)
                {
                    CommLog.info("添加字段, SQL：" + fieldSql);
                    conn.createStatement().execute(fieldSql);
                }
            }
            // 升级字段
            if (!fieldsToUpdate.isEmpty())
            {
                for (String fieldSql : fieldsToUpdate)
                {
                    CommLog.info("升级字段, SQL：" + fieldSql);
                    conn.createStatement().execute(fieldSql);
                }
            }

        } catch (Throwable e)
        {
            CommLog.error("升级数据库失败，请确认数据库连接配置，原因" + e.getMessage(), e);
            return false;
        }finally
        {
            SQLUtil.close(rs, stmt, conn);
        }

        try
        {
            CommLog.info("开始初始化数据库最大字段，共{}个表...",_clazzList.size());
            for (Class<? extends BaseBO> cs : _clazzList)
            {
                BMObj<? extends BaseBO> bm = _bm.initCheck(_m_dbObj, cs);

                //处理检查
                if(!bm.s_checkDBId())
                {
                    ALServerLog.Fatal("Check DB ID Table: [" + cs.getName() + "] fail!!");
                }
            }
        }catch (Throwable e)
        {
            CommLog.error("初始化数据库最大字段失败，请确认数据库连接配置，原因" + e.getMessage(), e);
            return  false;
        }

        CommLog.info("DB:[{}] Tag:[{}]==========数据库版本检测完毕==============",getDBObj().getDBName(),getDBObj().getDbTag());
        return true;
    }
}
