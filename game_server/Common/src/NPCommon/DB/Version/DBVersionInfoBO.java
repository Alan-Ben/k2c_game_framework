package NPCommon.DB.Version;

import NPCommon.DB.BaseBO;
import NPCommon.Enum.NPCommonEnum;

import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

public class DBVersionInfoBO extends BaseBO
{
    private long m_id;
    private String m_version;
    private String m_info;
    private byte[] m_idBlob;

    public DBVersionInfoBO()
    {
        this.m_id = 0;
        this.m_version = "1.0.0.1";
        this.m_info = "";
        m_idBlob = null;
    }

    public DBVersionInfoBO(DBVersionInfoBO _bo)
    {
        this.m_id = _bo.m_id;
        this.m_version = _bo.m_version;
        this.m_info = _bo.m_info;
        m_idBlob = _bo.m_idBlob;
    }

    public DBVersionInfoBO(int id, String version, String info, byte[] _idBlob)
    {
        this.m_id = id;
        this.m_version = version;
        this.m_info = info;
        this.m_idBlob = _idBlob;
    }

    @SuppressWarnings({"unchecked", "rawtypes"})
    @Override
    public void getFromResultSet(ResultSet rs, List list) throws Exception
    {
        DBVersionInfoBO bo = new DBVersionInfoBO();

        bo.m_id = rs.getInt(1);
        bo.m_version = rs.getString(2);
        bo.m_info = rs.getString(3);
        bo.m_idBlob = rs.getBytes(4);

        list.add(bo);
    }

    public String createTableSql()
    {
        return String.format(
                "CREATE TABLE %s (%s int(11) NOT NULL AUTO_INCREMENT, %s varchar(64) NOT NULL, %s varchar(64), %s blob, PRIMARY KEY (%s));",
                this.getTbName(), "ID", "Version", "Info", "IdBlob", "ID");
    }

    @Override
    public String getItemsName()
    {
        return "`ID`, `Version`, `Info`, `IdBlob`";
    }

    @Override
    public String getItemsValue()
    {
        StringBuilder strBuf = new StringBuilder();

        strBuf.append(m_id).append(",'");
        strBuf.append(m_version).append("','");
        strBuf.append(m_info).append("', ");
        strBuf.append("?");//使用？可以通过编号使用getInsertValueByte函数返回数据通过stmt.setBytes插入对应Byte数据

        return strBuf.toString();
    }

    @Override
    public void setId(long iID)
    {
        m_id = iID;
    }

    @Override
    public long getId()
    {
        return m_id;
    }

    public void setVersion(String version)
    {
        m_version = version;
    }

    public String getVersion()
    {
        return m_version;
    }

    public void setInfo(String info)
    {
        m_info = info;
    }

    public String getInfo()
    {
        return m_info;
    }

    public void setIdBlob(byte[] _idBlob)
    {
        m_idBlob = _idBlob;
    }
    public byte[] getIdBlob()
    {
        return m_idBlob;
    }


    @Override
    protected String getUpdateKeyValue()
    {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `Version` = '").append(m_version).append("',");
        sBuilder.append(" `Info` = '").append(m_info).append("',");
        sBuilder.append(" `IdBlob` = ?");//使用？可以通过编号使用getInsertValueByte函数返回数据通过stmt.setBytes插入对应Byte数据
        return sBuilder.toString();
    }

    @Override
    protected ArrayList<byte[]> getInsertValueBytes()
    {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(m_idBlob);
        return ret;
    }

    @Override
    public String getTableName()
    {
        return "`table_version_info`";
    }

    @Override
    public NPCommonEnum.EDBTag getDBTag()
    {
        return NPCommonEnum.EDBTag.NONE;
    }

    @Override
    public ByteBuffer toByteBuffer()
    {
        return null;
    }

    @Override
    public void readFromByteBuffer(ByteBuffer _buffer)
    {

    }

    @Override
    protected ArrayList<byte[]> getMarkedValueBytes()
    {
        return null;
    }

    @Override
    public String getMarkedUpdateKeyValue()
    {

        return "";
    }
}
