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
public class GuildMarsMineShareBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟ID")
    private long guild_id;

    public static final int FIELD_finder_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "finder_cid", comment = "发现者CID")
    private long finder_cid;

    public static final int FIELD_mine_instance_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "mine_instance_id", comment = "矿实例ID")
    private long mine_instance_id;

    public static final int FIELD_mine_end_show_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "mine_end_show_ms", comment = "矿有效期毫秒时间戳")
    private long mine_end_show_ms;

    public static final int FIELD_pos_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "pos_id", comment = "矿位置ID")
    private long pos_id;

    public GuildMarsMineShareBO() {
        id = 0;
        guild_id = 0L;
        finder_cid = 0L;
        mine_instance_id = 0L;
        mine_end_show_ms = 0L;
        pos_id = 0L;
    }

    public GuildMarsMineShareBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        finder_cid = rs.getLong(3);
        mine_instance_id = rs.getLong(4);
        mine_end_show_ms = rs.getLong(5);
        pos_id = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildMarsMineShareBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `finder_cid`, `mine_instance_id`, `mine_end_show_ms`, `pos_id`";
    }

    @Override
    public String getTableName() {
        return "`guild_mars_mine_share`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(finder_cid).append("', ");
        strBuf.append("'").append(mine_instance_id).append("', ");
        strBuf.append("'").append(mine_end_show_ms).append("', ");
        strBuf.append("'").append(pos_id).append("', ");
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

    // 发现者CID
    public long getFinderCid() { return this.finder_cid; }
    public void setFinderCid(BM _bm, long finder_cid) {
        if(finder_cid==this.finder_cid) 
            return;
        this.finder_cid = finder_cid; 
        markField(_bm, FIELD_finder_cid); 
    }
    public void saveFinderCid(BM _bm, long finder_cid) {
        if(finder_cid==this.finder_cid) 
            return;
        this.finder_cid = finder_cid;
        saveField(_bm, "finder_cid", finder_cid);
    }

    // 矿实例ID
    public long getMineInstanceId() { return this.mine_instance_id; }
    public void setMineInstanceId(BM _bm, long mine_instance_id) {
        if(mine_instance_id==this.mine_instance_id) 
            return;
        this.mine_instance_id = mine_instance_id; 
        markField(_bm, FIELD_mine_instance_id); 
    }
    public void saveMineInstanceId(BM _bm, long mine_instance_id) {
        if(mine_instance_id==this.mine_instance_id) 
            return;
        this.mine_instance_id = mine_instance_id;
        saveField(_bm, "mine_instance_id", mine_instance_id);
    }

    // 矿有效期毫秒时间戳
    public long getMineEndShowMs() { return this.mine_end_show_ms; }
    public void setMineEndShowMs(BM _bm, long mine_end_show_ms) {
        if(mine_end_show_ms==this.mine_end_show_ms) 
            return;
        this.mine_end_show_ms = mine_end_show_ms; 
        markField(_bm, FIELD_mine_end_show_ms); 
    }
    public void saveMineEndShowMs(BM _bm, long mine_end_show_ms) {
        if(mine_end_show_ms==this.mine_end_show_ms) 
            return;
        this.mine_end_show_ms = mine_end_show_ms;
        saveField(_bm, "mine_end_show_ms", mine_end_show_ms);
    }

    // 矿位置ID
    public long getPosId() { return this.pos_id; }
    public void setPosId(BM _bm, long pos_id) {
        if(pos_id==this.pos_id) 
            return;
        this.pos_id = pos_id; 
        markField(_bm, FIELD_pos_id); 
    }
    public void savePosId(BM _bm, long pos_id) {
        if(pos_id==this.pos_id) 
            return;
        this.pos_id = pos_id;
        saveField(_bm, "pos_id", pos_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `finder_cid` = '").append(finder_cid).append("',");
        sBuilder.append(" `mine_instance_id` = '").append(mine_instance_id).append("',");
        sBuilder.append(" `mine_end_show_ms` = '").append(mine_end_show_ms).append("',");
        sBuilder.append(" `pos_id` = '").append(pos_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_finder_cid)) sBuilder.append(" `finder_cid` = '").append(finder_cid).append("',");
        if(isFieldMarked(FIELD_mine_instance_id)) sBuilder.append(" `mine_instance_id` = '").append(mine_instance_id).append("',");
        if(isFieldMarked(FIELD_mine_end_show_ms)) sBuilder.append(" `mine_end_show_ms` = '").append(mine_end_show_ms).append("',");
        if(isFieldMarked(FIELD_pos_id)) sBuilder.append(" `pos_id` = '").append(pos_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_mars_mine_share` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`finder_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '发现者CID',"
                + "`mine_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '矿实例ID',"
                + "`mine_end_show_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '矿有效期毫秒时间戳',"
                + "`pos_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '矿位置ID',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟分享矿数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//finder_cid
        _size+=8;//mine_instance_id
        _size+=8;//mine_end_show_ms
        _size+=8;//pos_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(finder_cid);
        buff.putLong(mine_instance_id);
        buff.putLong(mine_end_show_ms);
        buff.putLong(pos_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        finder_cid=buff.getLong();
        mine_instance_id=buff.getLong();
        mine_end_show_ms=buff.getLong();
        pos_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
