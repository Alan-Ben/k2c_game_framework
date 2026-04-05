package USDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class PlayerLevyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_levy_type =1;
    @DataBaseField(type = "int(11)", fieldname = "levy_type", comment = "征收类型")
    private int levy_type;

    public static final int FIELD_levy_info =2;
    @DataBaseField(type = "blob", fieldname = "levy_info", comment = "征收数据")
    private byte[] levy_info;

    public static final int FIELD_levy_sum =3;
    @DataBaseField(type = "bigint(20)", fieldname = "levy_sum", comment = "征收总和")
    private long levy_sum;

    public static final int FIELD_crit_sum =4;
    @DataBaseField(type = "int(11)", fieldname = "crit_sum", comment = "暴击次数总和")
    private int crit_sum;

    public PlayerLevyBO() {
        id = 0;
        cid = 0L;
        levy_type = 0;
        levy_info = null;
        levy_sum = 0L;
        crit_sum = 0;
    }

    public PlayerLevyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        levy_type = rs.getInt(3);
        levy_info = rs.getBytes(4);
        levy_sum = rs.getLong(5);
        crit_sum = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerLevyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `levy_type`, `levy_info`, `levy_sum`, `crit_sum`";
    }

    @Override
    public String getTableName() {
        return "`player_levy`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(levy_type).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(levy_sum).append("', ");
        strBuf.append("'").append(crit_sum).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(levy_info);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_levy_info)) ret.add(levy_info);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 玩家CID
    public long getCid() { return this.cid; }
    public void setCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid; 
        markField(_bm, FIELD_cid); 
    }
    public void saveCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid;
        saveField(_bm, "cid", cid);
    }

    // 征收类型
    public int getLevyType() { return this.levy_type; }
    public void setLevyType(BM _bm, int levy_type) {
        if(levy_type==this.levy_type) 
            return;
        this.levy_type = levy_type; 
        markField(_bm, FIELD_levy_type); 
    }
    public void saveLevyType(BM _bm, int levy_type) {
        if(levy_type==this.levy_type) 
            return;
        this.levy_type = levy_type;
        saveField(_bm, "levy_type", levy_type);
    }

    // 征收数据
    public byte[] getLevyInfo() { return this.levy_info; }
    public void setLevyInfo(BM _bm, byte[] levy_info) {
        if(levy_info==this.levy_info) 
            return;
        this.levy_info = levy_info; 
        markField(_bm, FIELD_levy_info); 
    }
    public void saveLevyInfo(BM _bm, byte[] levy_info) {
        if(levy_info==this.levy_info) 
            return;
        this.levy_info = levy_info;
        saveFieldBytes(_bm, "levy_info", levy_info);
    }

    // 征收总和
    public long getLevySum() { return this.levy_sum; }
    public void setLevySum(BM _bm, long levy_sum) {
        if(levy_sum==this.levy_sum) 
            return;
        this.levy_sum = levy_sum; 
        markField(_bm, FIELD_levy_sum); 
    }
    public void saveLevySum(BM _bm, long levy_sum) {
        if(levy_sum==this.levy_sum) 
            return;
        this.levy_sum = levy_sum;
        saveField(_bm, "levy_sum", levy_sum);
    }

    // 暴击次数总和
    public int getCritSum() { return this.crit_sum; }
    public void setCritSum(BM _bm, int crit_sum) {
        if(crit_sum==this.crit_sum) 
            return;
        this.crit_sum = crit_sum; 
        markField(_bm, FIELD_crit_sum); 
    }
    public void saveCritSum(BM _bm, int crit_sum) {
        if(crit_sum==this.crit_sum) 
            return;
        this.crit_sum = crit_sum;
        saveField(_bm, "crit_sum", crit_sum);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `levy_type` = '").append(levy_type).append("',");
        sBuilder.append(" `levy_info` = ?,");
        sBuilder.append(" `levy_sum` = '").append(levy_sum).append("',");
        sBuilder.append(" `crit_sum` = '").append(crit_sum).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_levy_type)) sBuilder.append(" `levy_type` = '").append(levy_type).append("',");
        if(isFieldMarked(FIELD_levy_info)) sBuilder.append(" `levy_info` = ?,");
        if(isFieldMarked(FIELD_levy_sum)) sBuilder.append(" `levy_sum` = '").append(levy_sum).append("',");
        if(isFieldMarked(FIELD_crit_sum)) sBuilder.append(" `crit_sum` = '").append(crit_sum).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_levy` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`levy_type` int(11) NOT NULL DEFAULT '0' COMMENT '征收类型',"
                + "`levy_info` blob NULL COMMENT '征收数据',"
                + "`levy_sum` bigint(20) NOT NULL DEFAULT '0' COMMENT '征收总和',"
                + "`crit_sum` int(11) NOT NULL DEFAULT '0' COMMENT '暴击次数总和',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家征收数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//cid
        _size+=4;//levy_type
        _size+=2;_size+=levy_info.length;//levy_info
        _size+=8;//levy_sum
        _size+=4;//crit_sum
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(levy_type);
        buff.putShort((short)(levy_info == null ? 0 : levy_info.length));if(null != levy_info){buff.put(levy_info);}
        buff.putLong(levy_sum);
        buff.putInt(crit_sum);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        levy_type=buff.getInt();
        int levy_info_count = buff.getShort();if(levy_info_count>0){levy_info = new byte[levy_info_count];buff.get(levy_info);}
        levy_sum=buff.getLong();
        crit_sum=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
