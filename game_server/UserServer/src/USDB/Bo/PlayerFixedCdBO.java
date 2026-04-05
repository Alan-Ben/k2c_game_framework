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
public class PlayerFixedCdBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_cd_id =1;
    @DataBaseField(type = "int(11)", fieldname = "cd_id", comment = "CD类型ID")
    private int cd_id;

    public static final int FIELD_last_calc_time =2;
    @DataBaseField(type = "bigint(20)", fieldname = "last_calc_time", comment = "最近结算时间，ms")
    private long last_calc_time;

    public static final int FIELD_count =3;
    @DataBaseField(type = "int(11)", fieldname = "count", comment = "cd计数")
    private int count;

    public PlayerFixedCdBO() {
        id = 0;
        cid = 0L;
        cd_id = 0;
        last_calc_time = 0L;
        count = 0;
    }

    public PlayerFixedCdBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        cd_id = rs.getInt(3);
        last_calc_time = rs.getLong(4);
        count = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerFixedCdBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `cd_id`, `last_calc_time`, `count`";
    }

    @Override
    public String getTableName() {
        return "`player_fixed_cd`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(cd_id).append("', ");
        strBuf.append("'").append(last_calc_time).append("', ");
        strBuf.append("'").append(count).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
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

    // CD类型ID
    public int getCdId() { return this.cd_id; }
    public void setCdId(BM _bm, int cd_id) {
        if(cd_id==this.cd_id) 
            return;
        this.cd_id = cd_id; 
        markField(_bm, FIELD_cd_id); 
    }
    public void saveCdId(BM _bm, int cd_id) {
        if(cd_id==this.cd_id) 
            return;
        this.cd_id = cd_id;
        saveField(_bm, "cd_id", cd_id);
    }

    // 最近结算时间，ms
    public long getLastCalcTime() { return this.last_calc_time; }
    public void setLastCalcTime(BM _bm, long last_calc_time) {
        if(last_calc_time==this.last_calc_time) 
            return;
        this.last_calc_time = last_calc_time; 
        markField(_bm, FIELD_last_calc_time); 
    }
    public void saveLastCalcTime(BM _bm, long last_calc_time) {
        if(last_calc_time==this.last_calc_time) 
            return;
        this.last_calc_time = last_calc_time;
        saveField(_bm, "last_calc_time", last_calc_time);
    }

    // cd计数
    public int getCount() { return this.count; }
    public void setCount(BM _bm, int count) {
        if(count==this.count) 
            return;
        this.count = count; 
        markField(_bm, FIELD_count); 
    }
    public void saveCount(BM _bm, int count) {
        if(count==this.count) 
            return;
        this.count = count;
        saveField(_bm, "count", count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `cd_id` = '").append(cd_id).append("',");
        sBuilder.append(" `last_calc_time` = '").append(last_calc_time).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_cd_id)) sBuilder.append(" `cd_id` = '").append(cd_id).append("',");
        if(isFieldMarked(FIELD_last_calc_time)) sBuilder.append(" `last_calc_time` = '").append(last_calc_time).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_fixed_cd` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`cd_id` int(11) NOT NULL DEFAULT '0' COMMENT 'CD类型ID',"
                + "`last_calc_time` bigint(20) NOT NULL DEFAULT '0' COMMENT '最近结算时间，ms',"
                + "`count` int(11) NOT NULL DEFAULT '0' COMMENT 'cd计数',"
                + "KEY `cid` (`cid`),"
                + "KEY `cd_id` (`cd_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家固定时间刷新CD' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//cd_id
        _size+=8;//last_calc_time
        _size+=4;//count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(cd_id);
        buff.putLong(last_calc_time);
        buff.putInt(count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        cd_id=buff.getInt();
        last_calc_time=buff.getLong();
        count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
