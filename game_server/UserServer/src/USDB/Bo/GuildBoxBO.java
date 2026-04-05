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
public class GuildBoxBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟id")
    private long guild_id;

    public static final int FIELD_box_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "box_id", comment = "宝箱配置ID")
    private long box_id;

    public static final int FIELD_share_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "share_cid", comment = "分享玩家CID")
    private long share_cid;

    public static final int FIELD_end_time =3;
    @DataBaseField(type = "bigint(20)", fieldname = "end_time", comment = "截至时间（毫秒）")
    private long end_time;

    public static final int FIELD_is_anonymous =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_anonymous", comment = "是否匿名")
    private boolean is_anonymous;

    public GuildBoxBO() {
        id = 0;
        guild_id = 0L;
        box_id = 0L;
        share_cid = 0L;
        end_time = 0L;
        is_anonymous = false;
    }

    public GuildBoxBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        box_id = rs.getLong(3);
        share_cid = rs.getLong(4);
        end_time = rs.getLong(5);
        is_anonymous = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildBoxBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `box_id`, `share_cid`, `end_time`, `is_anonymous`";
    }

    @Override
    public String getTableName() {
        return "`guild_box`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(box_id).append("', ");
        strBuf.append("'").append(share_cid).append("', ");
        strBuf.append("'").append(end_time).append("', ");
        strBuf.append("'").append(is_anonymous ? 1 : 0).append("', ");
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

    // 联盟id
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

    // 宝箱配置ID
    public long getBoxId() { return this.box_id; }
    public void setBoxId(BM _bm, long box_id) {
        if(box_id==this.box_id) 
            return;
        this.box_id = box_id; 
        markField(_bm, FIELD_box_id); 
    }
    public void saveBoxId(BM _bm, long box_id) {
        if(box_id==this.box_id) 
            return;
        this.box_id = box_id;
        saveField(_bm, "box_id", box_id);
    }

    // 分享玩家CID
    public long getShareCid() { return this.share_cid; }
    public void setShareCid(BM _bm, long share_cid) {
        if(share_cid==this.share_cid) 
            return;
        this.share_cid = share_cid; 
        markField(_bm, FIELD_share_cid); 
    }
    public void saveShareCid(BM _bm, long share_cid) {
        if(share_cid==this.share_cid) 
            return;
        this.share_cid = share_cid;
        saveField(_bm, "share_cid", share_cid);
    }

    // 截至时间（毫秒）
    public long getEndTime() { return this.end_time; }
    public void setEndTime(BM _bm, long end_time) {
        if(end_time==this.end_time) 
            return;
        this.end_time = end_time; 
        markField(_bm, FIELD_end_time); 
    }
    public void saveEndTime(BM _bm, long end_time) {
        if(end_time==this.end_time) 
            return;
        this.end_time = end_time;
        saveField(_bm, "end_time", end_time);
    }

    // 是否匿名
    public boolean getIsAnonymous() { return this.is_anonymous; }
    public void setIsAnonymous(BM _bm, boolean is_anonymous) {
        if(is_anonymous==this.is_anonymous) 
            return;
        this.is_anonymous = is_anonymous; 
        markField(_bm, FIELD_is_anonymous); 
    }
    public void saveIsAnonymous(BM _bm, boolean is_anonymous) {
        if(is_anonymous==this.is_anonymous) 
            return;
        this.is_anonymous = is_anonymous;
        saveField(_bm, "is_anonymous", is_anonymous ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `box_id` = '").append(box_id).append("',");
        sBuilder.append(" `share_cid` = '").append(share_cid).append("',");
        sBuilder.append(" `end_time` = '").append(end_time).append("',");
        sBuilder.append(" `is_anonymous` = '").append(is_anonymous ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_box_id)) sBuilder.append(" `box_id` = '").append(box_id).append("',");
        if(isFieldMarked(FIELD_share_cid)) sBuilder.append(" `share_cid` = '").append(share_cid).append("',");
        if(isFieldMarked(FIELD_end_time)) sBuilder.append(" `end_time` = '").append(end_time).append("',");
        if(isFieldMarked(FIELD_is_anonymous)) sBuilder.append(" `is_anonymous` = '").append(is_anonymous ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_box` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`box_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '宝箱配置ID',"
                + "`share_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '分享玩家CID',"
                + "`end_time` bigint(20) NOT NULL DEFAULT '0' COMMENT '截至时间（毫秒）',"
                + "`is_anonymous` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否匿名',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟宝箱数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//box_id
        _size+=8;//share_cid
        _size+=8;//end_time
        _size+=1;//is_anonymous
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(box_id);
        buff.putLong(share_cid);
        buff.putLong(end_time);
        buff.put((byte)(is_anonymous?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        box_id=buff.getLong();
        share_cid=buff.getLong();
        end_time=buff.getLong();
        is_anonymous=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
