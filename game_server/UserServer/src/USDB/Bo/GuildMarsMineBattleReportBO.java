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
public class GuildMarsMineBattleReportBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟ID")
    private long guild_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "发起战斗的玩家CID")
    private long cid;

    public static final int FIELD_log_type =2;
    @DataBaseField(type = "int(11)", fieldname = "log_type", comment = "战报类型（EMarsExplorePVPLogType）")
    private int log_type;

    public static final int FIELD_created_at =3;
    @DataBaseField(type = "bigint(20)", fieldname = "created_at", comment = "创建时间（毫秒）")
    private long created_at;

    public static final int FIELD_log_data =4;
    @DataBaseField(type = "blob", fieldname = "log_data", comment = "战报数据")
    private byte[] log_data;

    public GuildMarsMineBattleReportBO() {
        id = 0;
        guild_id = 0L;
        cid = 0L;
        log_type = 0;
        created_at = 0L;
        log_data = null;
    }

    public GuildMarsMineBattleReportBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        cid = rs.getLong(3);
        log_type = rs.getInt(4);
        created_at = rs.getLong(5);
        log_data = rs.getBytes(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildMarsMineBattleReportBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `cid`, `log_type`, `created_at`, `log_data`";
    }

    @Override
    public String getTableName() {
        return "`guild_mars_mine_battle_report`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(log_type).append("', ");
        strBuf.append("'").append(created_at).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(log_data);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_log_data)) ret.add(log_data);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 联盟ID
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 发起战斗的玩家CID
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

    // 战报类型（EMarsExplorePVPLogType）
    public int getLogType() { return this.log_type; }
    public void setLogType(BM _bm, int log_type) {
        if(log_type==this.log_type) 
            return;
        this.log_type = log_type; 
        markField(_bm, FIELD_log_type); 
    }
    public void saveLogType(BM _bm, int log_type) {
        if(log_type==this.log_type) 
            return;
        this.log_type = log_type;
        saveField(_bm, "log_type", log_type);
    }

    // 创建时间（毫秒）
    public long getCreatedAt() { return this.created_at; }
    public void setCreatedAt(BM _bm, long created_at) {
        if(created_at==this.created_at) 
            return;
        this.created_at = created_at; 
        markField(_bm, FIELD_created_at); 
    }
    public void saveCreatedAt(BM _bm, long created_at) {
        if(created_at==this.created_at) 
            return;
        this.created_at = created_at;
        saveField(_bm, "created_at", created_at);
    }

    // 战报数据
    public byte[] getLogData() { return this.log_data; }
    public void setLogData(BM _bm, byte[] log_data) {
        if(log_data==this.log_data) 
            return;
        this.log_data = log_data; 
        markField(_bm, FIELD_log_data); 
    }
    public void saveLogData(BM _bm, byte[] log_data) {
        if(log_data==this.log_data) 
            return;
        this.log_data = log_data;
        saveFieldBytes(_bm, "log_data", log_data);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `log_type` = '").append(log_type).append("',");
        sBuilder.append(" `created_at` = '").append(created_at).append("',");
        sBuilder.append(" `log_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_log_type)) sBuilder.append(" `log_type` = '").append(log_type).append("',");
        if(isFieldMarked(FIELD_created_at)) sBuilder.append(" `created_at` = '").append(created_at).append("',");
        if(isFieldMarked(FIELD_log_data)) sBuilder.append(" `log_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_mars_mine_battle_report` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '发起战斗的玩家CID',"
                + "`log_type` int(11) NOT NULL DEFAULT '0' COMMENT '战报类型（EMarsExplorePVPLogType）',"
                + "`created_at` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间（毫秒）',"
                + "`log_data` blob NULL COMMENT '战报数据',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟火星矿战报数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guild_id
        _size+=8;//cid
        _size+=4;//log_type
        _size+=8;//created_at
        _size+=2;_size+=log_data.length;//log_data
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(cid);
        buff.putInt(log_type);
        buff.putLong(created_at);
        buff.putShort((short)(log_data == null ? 0 : log_data.length));if(null != log_data){buff.put(log_data);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        cid=buff.getLong();
        log_type=buff.getInt();
        created_at=buff.getLong();
        int log_data_count = buff.getShort();if(log_data_count>0){log_data = new byte[log_data_count];buff.get(log_data);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
