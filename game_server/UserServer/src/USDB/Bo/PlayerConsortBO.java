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
public class PlayerConsortBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_consortId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "家人ID")
    private long consortId;

    public static final int FIELD_fettersLvl =2;
    @DataBaseField(type = "int(11)", fieldname = "fettersLvl", comment = "羁绊等级")
    private int fettersLvl;

    public static final int FIELD_isHaloUnlock =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "isHaloUnlock", comment = "星辉是否解锁")
    private boolean isHaloUnlock;

    public static final int FIELD_haloLvl =4;
    @DataBaseField(type = "int(11)", fieldname = "haloLvl", comment = "星辉等级")
    private int haloLvl;

    public static final int FIELD_intimacy =5;
    @DataBaseField(type = "bigint(20)", fieldname = "intimacy", comment = "亲密度")
    private long intimacy;

    public static final int FIELD_initIntimacy =6;
    @DataBaseField(type = "bigint(20)", fieldname = "initIntimacy", comment = "初始亲密度")
    private long initIntimacy;

    public static final int FIELD_charm =7;
    @DataBaseField(type = "bigint(20)", fieldname = "charm", comment = "加护力")
    private long charm;

    public static final int FIELD_initCharm =8;
    @DataBaseField(type = "bigint(20)", fieldname = "initCharm", comment = "初始加护力")
    private long initCharm;

    public static final int FIELD_charmPoint =9;
    @DataBaseField(type = "bigint(20)", fieldname = "charmPoint", comment = "加护力点数")
    private long charmPoint;

    public static final int FIELD_curSkinId =10;
    @DataBaseField(type = "bigint(20)", fieldname = "curSkinId", comment = "当前皮肤ID")
    private long curSkinId;

    public static final int FIELD_triggeredCallStoryIdList =11;
    @DataBaseField(type = "blob", fieldname = "triggeredCallStoryIdList", comment = "已触发的邀约事件ID列表")
    private byte[] triggeredCallStoryIdList;

    public static final int FIELD_charmPointRecord =12;
    @DataBaseField(type = "bigint(20)", fieldname = "charmPointRecord", comment = "加护力点数记录")
    private long charmPointRecord;

    public static final int FIELD_has_add_chat_friend =13;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_add_chat_friend", comment = "是否已添加聊天好友")
    private boolean has_add_chat_friend;

    public PlayerConsortBO() {
        id = 0;
        cid = 0L;
        consortId = 0L;
        fettersLvl = 0;
        isHaloUnlock = false;
        haloLvl = 0;
        intimacy = 0L;
        initIntimacy = 0L;
        charm = 0L;
        initCharm = 0L;
        charmPoint = 0L;
        curSkinId = 0L;
        triggeredCallStoryIdList = null;
        charmPointRecord = 0L;
        has_add_chat_friend = false;
    }

    public PlayerConsortBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        consortId = rs.getLong(3);
        fettersLvl = rs.getInt(4);
        isHaloUnlock = rs.getBoolean(5);
        haloLvl = rs.getInt(6);
        intimacy = rs.getLong(7);
        initIntimacy = rs.getLong(8);
        charm = rs.getLong(9);
        initCharm = rs.getLong(10);
        charmPoint = rs.getLong(11);
        curSkinId = rs.getLong(12);
        triggeredCallStoryIdList = rs.getBytes(13);
        charmPointRecord = rs.getLong(14);
        has_add_chat_friend = rs.getBoolean(15);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerConsortBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `consortId`, `fettersLvl`, `isHaloUnlock`, `haloLvl`, `intimacy`, `initIntimacy`, `charm`, `initCharm`, `charmPoint`, `curSkinId`, `triggeredCallStoryIdList`, `charmPointRecord`, `has_add_chat_friend`";
    }

    @Override
    public String getTableName() {
        return "`player_consort`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(fettersLvl).append("', ");
        strBuf.append("'").append(isHaloUnlock ? 1 : 0).append("', ");
        strBuf.append("'").append(haloLvl).append("', ");
        strBuf.append("'").append(intimacy).append("', ");
        strBuf.append("'").append(initIntimacy).append("', ");
        strBuf.append("'").append(charm).append("', ");
        strBuf.append("'").append(initCharm).append("', ");
        strBuf.append("'").append(charmPoint).append("', ");
        strBuf.append("'").append(curSkinId).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(charmPointRecord).append("', ");
        strBuf.append("'").append(has_add_chat_friend ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(triggeredCallStoryIdList);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_triggeredCallStoryIdList)) ret.add(triggeredCallStoryIdList);         return ret;
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

    // 家人ID
    public long getConsortId() { return this.consortId; }
    public void setConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId; 
        markField(_bm, FIELD_consortId); 
    }
    public void saveConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId;
        saveField(_bm, "consortId", consortId);
    }

    // 羁绊等级
    public int getFettersLvl() { return this.fettersLvl; }
    public void setFettersLvl(BM _bm, int fettersLvl) {
        if(fettersLvl==this.fettersLvl) 
            return;
        this.fettersLvl = fettersLvl; 
        markField(_bm, FIELD_fettersLvl); 
    }
    public void saveFettersLvl(BM _bm, int fettersLvl) {
        if(fettersLvl==this.fettersLvl) 
            return;
        this.fettersLvl = fettersLvl;
        saveField(_bm, "fettersLvl", fettersLvl);
    }

    // 星辉是否解锁
    public boolean getIsHaloUnlock() { return this.isHaloUnlock; }
    public void setIsHaloUnlock(BM _bm, boolean isHaloUnlock) {
        if(isHaloUnlock==this.isHaloUnlock) 
            return;
        this.isHaloUnlock = isHaloUnlock; 
        markField(_bm, FIELD_isHaloUnlock); 
    }
    public void saveIsHaloUnlock(BM _bm, boolean isHaloUnlock) {
        if(isHaloUnlock==this.isHaloUnlock) 
            return;
        this.isHaloUnlock = isHaloUnlock;
        saveField(_bm, "isHaloUnlock", isHaloUnlock ? 1 : 0);
    }

    // 星辉等级
    public int getHaloLvl() { return this.haloLvl; }
    public void setHaloLvl(BM _bm, int haloLvl) {
        if(haloLvl==this.haloLvl) 
            return;
        this.haloLvl = haloLvl; 
        markField(_bm, FIELD_haloLvl); 
    }
    public void saveHaloLvl(BM _bm, int haloLvl) {
        if(haloLvl==this.haloLvl) 
            return;
        this.haloLvl = haloLvl;
        saveField(_bm, "haloLvl", haloLvl);
    }

    // 亲密度
    public long getIntimacy() { return this.intimacy; }
    public void setIntimacy(BM _bm, long intimacy) {
        if(intimacy==this.intimacy) 
            return;
        this.intimacy = intimacy; 
        markField(_bm, FIELD_intimacy); 
    }
    public void saveIntimacy(BM _bm, long intimacy) {
        if(intimacy==this.intimacy) 
            return;
        this.intimacy = intimacy;
        saveField(_bm, "intimacy", intimacy);
    }

    // 初始亲密度
    public long getInitIntimacy() { return this.initIntimacy; }
    public void setInitIntimacy(BM _bm, long initIntimacy) {
        if(initIntimacy==this.initIntimacy) 
            return;
        this.initIntimacy = initIntimacy; 
        markField(_bm, FIELD_initIntimacy); 
    }
    public void saveInitIntimacy(BM _bm, long initIntimacy) {
        if(initIntimacy==this.initIntimacy) 
            return;
        this.initIntimacy = initIntimacy;
        saveField(_bm, "initIntimacy", initIntimacy);
    }

    // 加护力
    public long getCharm() { return this.charm; }
    public void setCharm(BM _bm, long charm) {
        if(charm==this.charm) 
            return;
        this.charm = charm; 
        markField(_bm, FIELD_charm); 
    }
    public void saveCharm(BM _bm, long charm) {
        if(charm==this.charm) 
            return;
        this.charm = charm;
        saveField(_bm, "charm", charm);
    }

    // 初始加护力
    public long getInitCharm() { return this.initCharm; }
    public void setInitCharm(BM _bm, long initCharm) {
        if(initCharm==this.initCharm) 
            return;
        this.initCharm = initCharm; 
        markField(_bm, FIELD_initCharm); 
    }
    public void saveInitCharm(BM _bm, long initCharm) {
        if(initCharm==this.initCharm) 
            return;
        this.initCharm = initCharm;
        saveField(_bm, "initCharm", initCharm);
    }

    // 加护力点数
    public long getCharmPoint() { return this.charmPoint; }
    public void setCharmPoint(BM _bm, long charmPoint) {
        if(charmPoint==this.charmPoint) 
            return;
        this.charmPoint = charmPoint; 
        markField(_bm, FIELD_charmPoint); 
    }
    public void saveCharmPoint(BM _bm, long charmPoint) {
        if(charmPoint==this.charmPoint) 
            return;
        this.charmPoint = charmPoint;
        saveField(_bm, "charmPoint", charmPoint);
    }

    // 当前皮肤ID
    public long getCurSkinId() { return this.curSkinId; }
    public void setCurSkinId(BM _bm, long curSkinId) {
        if(curSkinId==this.curSkinId) 
            return;
        this.curSkinId = curSkinId; 
        markField(_bm, FIELD_curSkinId); 
    }
    public void saveCurSkinId(BM _bm, long curSkinId) {
        if(curSkinId==this.curSkinId) 
            return;
        this.curSkinId = curSkinId;
        saveField(_bm, "curSkinId", curSkinId);
    }

    // 已触发的邀约事件ID列表
    public byte[] getTriggeredCallStoryIdList() { return this.triggeredCallStoryIdList; }
    public void setTriggeredCallStoryIdList(BM _bm, byte[] triggeredCallStoryIdList) {
        if(triggeredCallStoryIdList==this.triggeredCallStoryIdList) 
            return;
        this.triggeredCallStoryIdList = triggeredCallStoryIdList; 
        markField(_bm, FIELD_triggeredCallStoryIdList); 
    }
    public void saveTriggeredCallStoryIdList(BM _bm, byte[] triggeredCallStoryIdList) {
        if(triggeredCallStoryIdList==this.triggeredCallStoryIdList) 
            return;
        this.triggeredCallStoryIdList = triggeredCallStoryIdList;
        saveFieldBytes(_bm, "triggeredCallStoryIdList", triggeredCallStoryIdList);
    }

    // 加护力点数记录
    public long getCharmPointRecord() { return this.charmPointRecord; }
    public void setCharmPointRecord(BM _bm, long charmPointRecord) {
        if(charmPointRecord==this.charmPointRecord) 
            return;
        this.charmPointRecord = charmPointRecord; 
        markField(_bm, FIELD_charmPointRecord); 
    }
    public void saveCharmPointRecord(BM _bm, long charmPointRecord) {
        if(charmPointRecord==this.charmPointRecord) 
            return;
        this.charmPointRecord = charmPointRecord;
        saveField(_bm, "charmPointRecord", charmPointRecord);
    }

    // 是否已添加聊天好友
    public boolean getHasAddChatFriend() { return this.has_add_chat_friend; }
    public void setHasAddChatFriend(BM _bm, boolean has_add_chat_friend) {
        if(has_add_chat_friend==this.has_add_chat_friend) 
            return;
        this.has_add_chat_friend = has_add_chat_friend; 
        markField(_bm, FIELD_has_add_chat_friend); 
    }
    public void saveHasAddChatFriend(BM _bm, boolean has_add_chat_friend) {
        if(has_add_chat_friend==this.has_add_chat_friend) 
            return;
        this.has_add_chat_friend = has_add_chat_friend;
        saveField(_bm, "has_add_chat_friend", has_add_chat_friend ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `fettersLvl` = '").append(fettersLvl).append("',");
        sBuilder.append(" `isHaloUnlock` = '").append(isHaloUnlock ? 1 : 0).append("',");
        sBuilder.append(" `haloLvl` = '").append(haloLvl).append("',");
        sBuilder.append(" `intimacy` = '").append(intimacy).append("',");
        sBuilder.append(" `initIntimacy` = '").append(initIntimacy).append("',");
        sBuilder.append(" `charm` = '").append(charm).append("',");
        sBuilder.append(" `initCharm` = '").append(initCharm).append("',");
        sBuilder.append(" `charmPoint` = '").append(charmPoint).append("',");
        sBuilder.append(" `curSkinId` = '").append(curSkinId).append("',");
        sBuilder.append(" `triggeredCallStoryIdList` = ?,");
        sBuilder.append(" `charmPointRecord` = '").append(charmPointRecord).append("',");
        sBuilder.append(" `has_add_chat_friend` = '").append(has_add_chat_friend ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_fettersLvl)) sBuilder.append(" `fettersLvl` = '").append(fettersLvl).append("',");
        if(isFieldMarked(FIELD_isHaloUnlock)) sBuilder.append(" `isHaloUnlock` = '").append(isHaloUnlock ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_haloLvl)) sBuilder.append(" `haloLvl` = '").append(haloLvl).append("',");
        if(isFieldMarked(FIELD_intimacy)) sBuilder.append(" `intimacy` = '").append(intimacy).append("',");
        if(isFieldMarked(FIELD_initIntimacy)) sBuilder.append(" `initIntimacy` = '").append(initIntimacy).append("',");
        if(isFieldMarked(FIELD_charm)) sBuilder.append(" `charm` = '").append(charm).append("',");
        if(isFieldMarked(FIELD_initCharm)) sBuilder.append(" `initCharm` = '").append(initCharm).append("',");
        if(isFieldMarked(FIELD_charmPoint)) sBuilder.append(" `charmPoint` = '").append(charmPoint).append("',");
        if(isFieldMarked(FIELD_curSkinId)) sBuilder.append(" `curSkinId` = '").append(curSkinId).append("',");
        if(isFieldMarked(FIELD_triggeredCallStoryIdList)) sBuilder.append(" `triggeredCallStoryIdList` = ?,");
        if(isFieldMarked(FIELD_charmPointRecord)) sBuilder.append(" `charmPointRecord` = '").append(charmPointRecord).append("',");
        if(isFieldMarked(FIELD_has_add_chat_friend)) sBuilder.append(" `has_add_chat_friend` = '").append(has_add_chat_friend ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_consort` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '家人ID',"
                + "`fettersLvl` int(11) NOT NULL DEFAULT '0' COMMENT '羁绊等级',"
                + "`isHaloUnlock` tinyint(1) NOT NULL DEFAULT '0' COMMENT '星辉是否解锁',"
                + "`haloLvl` int(11) NOT NULL DEFAULT '0' COMMENT '星辉等级',"
                + "`intimacy` bigint(20) NOT NULL DEFAULT '0' COMMENT '亲密度',"
                + "`initIntimacy` bigint(20) NOT NULL DEFAULT '0' COMMENT '初始亲密度',"
                + "`charm` bigint(20) NOT NULL DEFAULT '0' COMMENT '加护力',"
                + "`initCharm` bigint(20) NOT NULL DEFAULT '0' COMMENT '初始加护力',"
                + "`charmPoint` bigint(20) NOT NULL DEFAULT '0' COMMENT '加护力点数',"
                + "`curSkinId` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前皮肤ID',"
                + "`triggeredCallStoryIdList` blob NULL COMMENT '已触发的邀约事件ID列表',"
                + "`charmPointRecord` bigint(20) NOT NULL DEFAULT '0' COMMENT '加护力点数记录',"
                + "`has_add_chat_friend` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已添加聊天好友',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家家人数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//consortId
        _size+=4;//fettersLvl
        _size+=1;//isHaloUnlock
        _size+=4;//haloLvl
        _size+=8;//intimacy
        _size+=8;//initIntimacy
        _size+=8;//charm
        _size+=8;//initCharm
        _size+=8;//charmPoint
        _size+=8;//curSkinId
        _size+=2;_size+=triggeredCallStoryIdList.length;//triggeredCallStoryIdList
        _size+=8;//charmPointRecord
        _size+=1;//has_add_chat_friend
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(consortId);
        buff.putInt(fettersLvl);
        buff.put((byte)(isHaloUnlock?1:0));
        buff.putInt(haloLvl);
        buff.putLong(intimacy);
        buff.putLong(initIntimacy);
        buff.putLong(charm);
        buff.putLong(initCharm);
        buff.putLong(charmPoint);
        buff.putLong(curSkinId);
        buff.putShort((short)(triggeredCallStoryIdList == null ? 0 : triggeredCallStoryIdList.length));if(null != triggeredCallStoryIdList){buff.put(triggeredCallStoryIdList);}
        buff.putLong(charmPointRecord);
        buff.put((byte)(has_add_chat_friend?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        consortId=buff.getLong();
        fettersLvl=buff.getInt();
        isHaloUnlock=(buff.get()==1);
        haloLvl=buff.getInt();
        intimacy=buff.getLong();
        initIntimacy=buff.getLong();
        charm=buff.getLong();
        initCharm=buff.getLong();
        charmPoint=buff.getLong();
        curSkinId=buff.getLong();
        int triggeredCallStoryIdList_count = buff.getShort();if(triggeredCallStoryIdList_count>0){triggeredCallStoryIdList = new byte[triggeredCallStoryIdList_count];buff.get(triggeredCallStoryIdList);}
        charmPointRecord=buff.getLong();
        has_add_chat_friend=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
