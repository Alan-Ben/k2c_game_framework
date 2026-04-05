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
public class PlayerEmoteGroupBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_ref_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "头像ID")
    private long ref_id;

    public static final int FIELD_expire_time_sec =2;
    @DataBaseField(type = "int(11)", fieldname = "expire_time_sec", comment = "超时时间戳，0一下表示永久")
    private int expire_time_sec;

    public static final int FIELD_viewed =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "viewed", comment = "是否已查看")
    private boolean viewed;

    public static final int FIELD_need_check_send_mail =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "need_check_send_mail", comment = "用于检查是否需要发送过期提醒邮件")
    private boolean need_check_send_mail;

    public PlayerEmoteGroupBO() {
        id = 0;
        cid = 0L;
        ref_id = 0L;
        expire_time_sec = 0;
        viewed = false;
        need_check_send_mail = false;
    }

    public PlayerEmoteGroupBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        ref_id = rs.getLong(3);
        expire_time_sec = rs.getInt(4);
        viewed = rs.getBoolean(5);
        need_check_send_mail = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerEmoteGroupBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `ref_id`, `expire_time_sec`, `viewed`, `need_check_send_mail`";
    }

    @Override
    public String getTableName() {
        return "`player_emote_group`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(expire_time_sec).append("', ");
        strBuf.append("'").append(viewed ? 1 : 0).append("', ");
        strBuf.append("'").append(need_check_send_mail ? 1 : 0).append("', ");
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

    // 头像ID
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
    }

    // 超时时间戳，0一下表示永久
    public int getExpireTimeSec() { return this.expire_time_sec; }
    public void setExpireTimeSec(BM _bm, int expire_time_sec) {
        if(expire_time_sec==this.expire_time_sec) 
            return;
        this.expire_time_sec = expire_time_sec; 
        markField(_bm, FIELD_expire_time_sec); 
    }
    public void saveExpireTimeSec(BM _bm, int expire_time_sec) {
        if(expire_time_sec==this.expire_time_sec) 
            return;
        this.expire_time_sec = expire_time_sec;
        saveField(_bm, "expire_time_sec", expire_time_sec);
    }

    // 是否已查看
    public boolean getViewed() { return this.viewed; }
    public void setViewed(BM _bm, boolean viewed) {
        if(viewed==this.viewed) 
            return;
        this.viewed = viewed; 
        markField(_bm, FIELD_viewed); 
    }
    public void saveViewed(BM _bm, boolean viewed) {
        if(viewed==this.viewed) 
            return;
        this.viewed = viewed;
        saveField(_bm, "viewed", viewed ? 1 : 0);
    }

    // 用于检查是否需要发送过期提醒邮件
    public boolean getNeedCheckSendMail() { return this.need_check_send_mail; }
    public void setNeedCheckSendMail(BM _bm, boolean need_check_send_mail) {
        if(need_check_send_mail==this.need_check_send_mail) 
            return;
        this.need_check_send_mail = need_check_send_mail; 
        markField(_bm, FIELD_need_check_send_mail); 
    }
    public void saveNeedCheckSendMail(BM _bm, boolean need_check_send_mail) {
        if(need_check_send_mail==this.need_check_send_mail) 
            return;
        this.need_check_send_mail = need_check_send_mail;
        saveField(_bm, "need_check_send_mail", need_check_send_mail ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `expire_time_sec` = '").append(expire_time_sec).append("',");
        sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        sBuilder.append(" `need_check_send_mail` = '").append(need_check_send_mail ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_expire_time_sec)) sBuilder.append(" `expire_time_sec` = '").append(expire_time_sec).append("',");
        if(isFieldMarked(FIELD_viewed)) sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_need_check_send_mail)) sBuilder.append(" `need_check_send_mail` = '").append(need_check_send_mail ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_emote_group` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '头像ID',"
                + "`expire_time_sec` int(11) NOT NULL DEFAULT '0' COMMENT '超时时间戳，0一下表示永久',"
                + "`viewed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已查看',"
                + "`need_check_send_mail` tinyint(1) NOT NULL DEFAULT '0' COMMENT '用于检查是否需要发送过期提醒邮件',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家表情包数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//ref_id
        _size+=4;//expire_time_sec
        _size+=1;//viewed
        _size+=1;//need_check_send_mail
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(ref_id);
        buff.putInt(expire_time_sec);
        buff.put((byte)(viewed?1:0));
        buff.put((byte)(need_check_send_mail?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        ref_id=buff.getLong();
        expire_time_sec=buff.getInt();
        viewed=(buff.get()==1);
        need_check_send_mail=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
