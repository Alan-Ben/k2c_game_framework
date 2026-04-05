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
public class GuildMarsHelpBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟id")
    private long guild_id;

    public static final int FIELD_sender_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "sender_cid", comment = "发送者cid")
    private long sender_cid;

    public static final int FIELD_obj_type =2;
    @DataBaseField(type = "int(11)", fieldname = "obj_type", comment = "互助目标实体类型")
    private int obj_type;

    public static final int FIELD_obj_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "obj_id", comment = "互助目标实体ID")
    private long obj_id;

    public static final int FIELD_deal_limit =4;
    @DataBaseField(type = "int(11)", fieldname = "deal_limit", comment = "求助允许处理的次数上限")
    private int deal_limit;

    public static final int FIELD_deal_secs =5;
    @DataBaseField(type = "int(11)", fieldname = "deal_secs", comment = "求助扣除的时长（秒）")
    private int deal_secs;

    public static final int FIELD_helper_cid_list =6;
    @DataBaseField(type = "blob", fieldname = "helper_cid_list", comment = "帮助者玩家CID列表")
    private byte[] helper_cid_list;

    public static final int FIELD_ext =7;
    @DataBaseField(type = "blob", fieldname = "ext", comment = "目标实体额外数据")
    private byte[] ext;

    public GuildMarsHelpBO() {
        id = 0;
        guild_id = 0L;
        sender_cid = 0L;
        obj_type = 0;
        obj_id = 0L;
        deal_limit = 0;
        deal_secs = 0;
        helper_cid_list = null;
        ext = null;
    }

    public GuildMarsHelpBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        sender_cid = rs.getLong(3);
        obj_type = rs.getInt(4);
        obj_id = rs.getLong(5);
        deal_limit = rs.getInt(6);
        deal_secs = rs.getInt(7);
        helper_cid_list = rs.getBytes(8);
        ext = rs.getBytes(9);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildMarsHelpBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `sender_cid`, `obj_type`, `obj_id`, `deal_limit`, `deal_secs`, `helper_cid_list`, `ext`";
    }

    @Override
    public String getTableName() {
        return "`guild_mars_help`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(sender_cid).append("', ");
        strBuf.append("'").append(obj_type).append("', ");
        strBuf.append("'").append(obj_id).append("', ");
        strBuf.append("'").append(deal_limit).append("', ");
        strBuf.append("'").append(deal_secs).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(helper_cid_list); 
        ret.add(ext);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_helper_cid_list)) ret.add(helper_cid_list); 
        if(isFieldMarked(FIELD_ext)) ret.add(ext);         return ret;
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

    // 发送者cid
    public long getSenderCid() { return this.sender_cid; }
    public void setSenderCid(BM _bm, long sender_cid) {
        if(sender_cid==this.sender_cid) 
            return;
        this.sender_cid = sender_cid; 
        markField(_bm, FIELD_sender_cid); 
    }
    public void saveSenderCid(BM _bm, long sender_cid) {
        if(sender_cid==this.sender_cid) 
            return;
        this.sender_cid = sender_cid;
        saveField(_bm, "sender_cid", sender_cid);
    }

    // 互助目标实体类型
    public int getObjType() { return this.obj_type; }
    public void setObjType(BM _bm, int obj_type) {
        if(obj_type==this.obj_type) 
            return;
        this.obj_type = obj_type; 
        markField(_bm, FIELD_obj_type); 
    }
    public void saveObjType(BM _bm, int obj_type) {
        if(obj_type==this.obj_type) 
            return;
        this.obj_type = obj_type;
        saveField(_bm, "obj_type", obj_type);
    }

    // 互助目标实体ID
    public long getObjId() { return this.obj_id; }
    public void setObjId(BM _bm, long obj_id) {
        if(obj_id==this.obj_id) 
            return;
        this.obj_id = obj_id; 
        markField(_bm, FIELD_obj_id); 
    }
    public void saveObjId(BM _bm, long obj_id) {
        if(obj_id==this.obj_id) 
            return;
        this.obj_id = obj_id;
        saveField(_bm, "obj_id", obj_id);
    }

    // 求助允许处理的次数上限
    public int getDealLimit() { return this.deal_limit; }
    public void setDealLimit(BM _bm, int deal_limit) {
        if(deal_limit==this.deal_limit) 
            return;
        this.deal_limit = deal_limit; 
        markField(_bm, FIELD_deal_limit); 
    }
    public void saveDealLimit(BM _bm, int deal_limit) {
        if(deal_limit==this.deal_limit) 
            return;
        this.deal_limit = deal_limit;
        saveField(_bm, "deal_limit", deal_limit);
    }

    // 求助扣除的时长（秒）
    public int getDealSecs() { return this.deal_secs; }
    public void setDealSecs(BM _bm, int deal_secs) {
        if(deal_secs==this.deal_secs) 
            return;
        this.deal_secs = deal_secs; 
        markField(_bm, FIELD_deal_secs); 
    }
    public void saveDealSecs(BM _bm, int deal_secs) {
        if(deal_secs==this.deal_secs) 
            return;
        this.deal_secs = deal_secs;
        saveField(_bm, "deal_secs", deal_secs);
    }

    // 帮助者玩家CID列表
    public byte[] getHelperCidList() { return this.helper_cid_list; }
    public void setHelperCidList(BM _bm, byte[] helper_cid_list) {
        if(helper_cid_list==this.helper_cid_list) 
            return;
        this.helper_cid_list = helper_cid_list; 
        markField(_bm, FIELD_helper_cid_list); 
    }
    public void saveHelperCidList(BM _bm, byte[] helper_cid_list) {
        if(helper_cid_list==this.helper_cid_list) 
            return;
        this.helper_cid_list = helper_cid_list;
        saveFieldBytes(_bm, "helper_cid_list", helper_cid_list);
    }

    // 目标实体额外数据
    public byte[] getExt() { return this.ext; }
    public void setExt(BM _bm, byte[] ext) {
        if(ext==this.ext) 
            return;
        this.ext = ext; 
        markField(_bm, FIELD_ext); 
    }
    public void saveExt(BM _bm, byte[] ext) {
        if(ext==this.ext) 
            return;
        this.ext = ext;
        saveFieldBytes(_bm, "ext", ext);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `sender_cid` = '").append(sender_cid).append("',");
        sBuilder.append(" `obj_type` = '").append(obj_type).append("',");
        sBuilder.append(" `obj_id` = '").append(obj_id).append("',");
        sBuilder.append(" `deal_limit` = '").append(deal_limit).append("',");
        sBuilder.append(" `deal_secs` = '").append(deal_secs).append("',");
        sBuilder.append(" `helper_cid_list` = ?,");
        sBuilder.append(" `ext` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_sender_cid)) sBuilder.append(" `sender_cid` = '").append(sender_cid).append("',");
        if(isFieldMarked(FIELD_obj_type)) sBuilder.append(" `obj_type` = '").append(obj_type).append("',");
        if(isFieldMarked(FIELD_obj_id)) sBuilder.append(" `obj_id` = '").append(obj_id).append("',");
        if(isFieldMarked(FIELD_deal_limit)) sBuilder.append(" `deal_limit` = '").append(deal_limit).append("',");
        if(isFieldMarked(FIELD_deal_secs)) sBuilder.append(" `deal_secs` = '").append(deal_secs).append("',");
        if(isFieldMarked(FIELD_helper_cid_list)) sBuilder.append(" `helper_cid_list` = ?,");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_mars_help` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`sender_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '发送者cid',"
                + "`obj_type` int(11) NOT NULL DEFAULT '0' COMMENT '互助目标实体类型',"
                + "`obj_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '互助目标实体ID',"
                + "`deal_limit` int(11) NOT NULL DEFAULT '0' COMMENT '求助允许处理的次数上限',"
                + "`deal_secs` int(11) NOT NULL DEFAULT '0' COMMENT '求助扣除的时长（秒）',"
                + "`helper_cid_list` blob NULL COMMENT '帮助者玩家CID列表',"
                + "`ext` blob NULL COMMENT '目标实体额外数据',"
                + "KEY `guild_id` (`guild_id`),"
                + "KEY `sender_cid` (`sender_cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟成员在火星系统的互助' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//sender_cid
        _size+=4;//obj_type
        _size+=8;//obj_id
        _size+=4;//deal_limit
        _size+=4;//deal_secs
        _size+=2;_size+=helper_cid_list.length;//helper_cid_list
        _size+=2;_size+=ext.length;//ext
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(sender_cid);
        buff.putInt(obj_type);
        buff.putLong(obj_id);
        buff.putInt(deal_limit);
        buff.putInt(deal_secs);
        buff.putShort((short)(helper_cid_list == null ? 0 : helper_cid_list.length));if(null != helper_cid_list){buff.put(helper_cid_list);}
        buff.putShort((short)(ext == null ? 0 : ext.length));if(null != ext){buff.put(ext);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        sender_cid=buff.getLong();
        obj_type=buff.getInt();
        obj_id=buff.getLong();
        deal_limit=buff.getInt();
        deal_secs=buff.getInt();
        int helper_cid_list_count = buff.getShort();if(helper_cid_list_count>0){helper_cid_list = new byte[helper_cid_list_count];buff.get(helper_cid_list);}
        int ext_count = buff.getShort();if(ext_count>0){ext = new byte[ext_count];buff.get(ext);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
