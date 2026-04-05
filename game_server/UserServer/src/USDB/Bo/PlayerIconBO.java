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
public class PlayerIconBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_iconId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "iconId", comment = "头像ID")
    private long iconId;

    public static final int FIELD_expireTimeS =2;
    @DataBaseField(type = "int(11)", fieldname = "expireTimeS", comment = "超时时间戳，0一下表示永久")
    private int expireTimeS;

    public static final int FIELD_viewed =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "viewed", comment = "是否已查看")
    private boolean viewed;

    public static final int FIELD_need_check_send_mail =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "need_check_send_mail", comment = "用于检查是否需要发送过期提醒邮件")
    private boolean need_check_send_mail;

    public PlayerIconBO() {
        id = 0;
        cid = 0L;
        iconId = 0L;
        expireTimeS = 0;
        viewed = false;
        need_check_send_mail = false;
    }

    public PlayerIconBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        iconId = rs.getLong(3);
        expireTimeS = rs.getInt(4);
        viewed = rs.getBoolean(5);
        need_check_send_mail = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerIconBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `iconId`, `expireTimeS`, `viewed`, `need_check_send_mail`";
    }

    @Override
    public String getTableName() {
        return "`player_icon`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(iconId).append("', ");
        strBuf.append("'").append(expireTimeS).append("', ");
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
    public long getIconId() { return this.iconId; }
    public void setIconId(BM _bm, long iconId) {
        if(iconId==this.iconId) 
            return;
        this.iconId = iconId; 
        markField(_bm, FIELD_iconId); 
    }
    public void saveIconId(BM _bm, long iconId) {
        if(iconId==this.iconId) 
            return;
        this.iconId = iconId;
        saveField(_bm, "iconId", iconId);
    }

    // 超时时间戳，0一下表示永久
    public int getExpireTimeS() { return this.expireTimeS; }
    public void setExpireTimeS(BM _bm, int expireTimeS) {
        if(expireTimeS==this.expireTimeS) 
            return;
        this.expireTimeS = expireTimeS; 
        markField(_bm, FIELD_expireTimeS); 
    }
    public void saveExpireTimeS(BM _bm, int expireTimeS) {
        if(expireTimeS==this.expireTimeS) 
            return;
        this.expireTimeS = expireTimeS;
        saveField(_bm, "expireTimeS", expireTimeS);
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
        sBuilder.append(" `iconId` = '").append(iconId).append("',");
        sBuilder.append(" `expireTimeS` = '").append(expireTimeS).append("',");
        sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        sBuilder.append(" `need_check_send_mail` = '").append(need_check_send_mail ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_iconId)) sBuilder.append(" `iconId` = '").append(iconId).append("',");
        if(isFieldMarked(FIELD_expireTimeS)) sBuilder.append(" `expireTimeS` = '").append(expireTimeS).append("',");
        if(isFieldMarked(FIELD_viewed)) sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_need_check_send_mail)) sBuilder.append(" `need_check_send_mail` = '").append(need_check_send_mail ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_icon` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`iconId` bigint(20) NOT NULL DEFAULT '0' COMMENT '头像ID',"
                + "`expireTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '超时时间戳，0一下表示永久',"
                + "`viewed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已查看',"
                + "`need_check_send_mail` tinyint(1) NOT NULL DEFAULT '0' COMMENT '用于检查是否需要发送过期提醒邮件',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User Info 玩家头像数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//iconId
        _size+=4;//expireTimeS
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
        buff.putLong(iconId);
        buff.putInt(expireTimeS);
        buff.put((byte)(viewed?1:0));
        buff.put((byte)(need_check_send_mail?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        iconId=buff.getLong();
        expireTimeS=buff.getInt();
        viewed=(buff.get()==1);
        need_check_send_mail=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
