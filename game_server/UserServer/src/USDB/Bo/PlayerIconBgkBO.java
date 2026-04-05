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
public class PlayerIconBgkBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_iconBgkId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "iconBgkId", comment = "头像框ID")
    private long iconBgkId;

    public static final int FIELD_lvl =2;
    @DataBaseField(type = "int(11)", fieldname = "lvl", comment = "头像框等级")
    private int lvl;

    public static final int FIELD_expireTimeS =3;
    @DataBaseField(type = "int(11)", fieldname = "expireTimeS", comment = "超时时间戳，0一下表示永久")
    private int expireTimeS;

    public static final int FIELD_viewed =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "viewed", comment = "是否已查看")
    private boolean viewed;

    public static final int FIELD_need_check_send_mail =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "need_check_send_mail", comment = "用于检查是否需要发送过期提醒邮件")
    private boolean need_check_send_mail;

    public PlayerIconBgkBO() {
        id = 0;
        cid = 0L;
        iconBgkId = 0L;
        lvl = 0;
        expireTimeS = 0;
        viewed = false;
        need_check_send_mail = false;
    }

    public PlayerIconBgkBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        iconBgkId = rs.getLong(3);
        lvl = rs.getInt(4);
        expireTimeS = rs.getInt(5);
        viewed = rs.getBoolean(6);
        need_check_send_mail = rs.getBoolean(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerIconBgkBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `iconBgkId`, `lvl`, `expireTimeS`, `viewed`, `need_check_send_mail`";
    }

    @Override
    public String getTableName() {
        return "`player_icon_bgk`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(iconBgkId).append("', ");
        strBuf.append("'").append(lvl).append("', ");
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

    // 头像框ID
    public long getIconBgkId() { return this.iconBgkId; }
    public void setIconBgkId(BM _bm, long iconBgkId) {
        if(iconBgkId==this.iconBgkId) 
            return;
        this.iconBgkId = iconBgkId; 
        markField(_bm, FIELD_iconBgkId); 
    }
    public void saveIconBgkId(BM _bm, long iconBgkId) {
        if(iconBgkId==this.iconBgkId) 
            return;
        this.iconBgkId = iconBgkId;
        saveField(_bm, "iconBgkId", iconBgkId);
    }

    // 头像框等级
    public int getLvl() { return this.lvl; }
    public void setLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl; 
        markField(_bm, FIELD_lvl); 
    }
    public void saveLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl;
        saveField(_bm, "lvl", lvl);
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
        sBuilder.append(" `iconBgkId` = '").append(iconBgkId).append("',");
        sBuilder.append(" `lvl` = '").append(lvl).append("',");
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
        if(isFieldMarked(FIELD_iconBgkId)) sBuilder.append(" `iconBgkId` = '").append(iconBgkId).append("',");
        if(isFieldMarked(FIELD_lvl)) sBuilder.append(" `lvl` = '").append(lvl).append("',");
        if(isFieldMarked(FIELD_expireTimeS)) sBuilder.append(" `expireTimeS` = '").append(expireTimeS).append("',");
        if(isFieldMarked(FIELD_viewed)) sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_need_check_send_mail)) sBuilder.append(" `need_check_send_mail` = '").append(need_check_send_mail ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_icon_bgk` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`iconBgkId` bigint(20) NOT NULL DEFAULT '0' COMMENT '头像框ID',"
                + "`lvl` int(11) NOT NULL DEFAULT '0' COMMENT '头像框等级',"
                + "`expireTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '超时时间戳，0一下表示永久',"
                + "`viewed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已查看',"
                + "`need_check_send_mail` tinyint(1) NOT NULL DEFAULT '0' COMMENT '用于检查是否需要发送过期提醒邮件',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User Info 玩家头像框数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//iconBgkId
        _size+=4;//lvl
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
        buff.putLong(iconBgkId);
        buff.putInt(lvl);
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
        iconBgkId=buff.getLong();
        lvl=buff.getInt();
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
