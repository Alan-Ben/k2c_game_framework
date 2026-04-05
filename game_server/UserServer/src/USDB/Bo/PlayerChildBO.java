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

@RefBo(isIdAuto = false)
public class PlayerChildBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_consortId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "关联家人ID")
    private long consortId;

    public static final int FIELD_initIntimacy =2;
    @DataBaseField(type = "bigint(20)", fieldname = "initIntimacy", comment = "初始亲密度")
    private long initIntimacy;

    public static final int FIELD_initResId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "initResId", comment = "子嗣初始形象配置")
    private long initResId;

    public static final int FIELD_quality =4;
    @DataBaseField(type = "bigint(20)", fieldname = "quality", comment = "子嗣品质")
    private long quality;

    public static final int FIELD_attrType =5;
    @DataBaseField(type = "int(11)", fieldname = "attrType", comment = "子嗣相性")
    private int attrType;

    public static final int FIELD_career =6;
    @DataBaseField(type = "bigint(20)", fieldname = "career", comment = "子嗣职业")
    private long career;

    public static final int FIELD_seatId =7;
    @DataBaseField(type = "bigint(20)", fieldname = "seatId", comment = "训练房ID")
    private long seatId;

    public static final int FIELD_isGiftde =8;
    @DataBaseField(type = "tinyint(1)", fieldname = "isGiftde", comment = "是否卷王")
    private boolean isGiftde;

    public static final int FIELD_initStudyBonus =9;
    @DataBaseField(type = "int(11)", fieldname = "initStudyBonus", comment = "教学经验加成（万分比）")
    private int initStudyBonus;

    public static final int FIELD_name =10;
    @DataBaseField(type = "varchar(500)", fieldname = "name", comment = "子嗣名称")
    private String name;

    public static final int FIELD_lvl =11;
    @DataBaseField(type = "int(11)", fieldname = "lvl", comment = "子嗣等级")
    private int lvl;

    public static final int FIELD_baseBonus =12;
    @DataBaseField(type = "bigint(20)", fieldname = "baseBonus", comment = "子嗣基础收益")
    private long baseBonus;

    public static final int FIELD_trainBonus =13;
    @DataBaseField(type = "bigint(20)", fieldname = "trainBonus", comment = "子嗣上课收益")
    private long trainBonus;

    public static final int FIELD_createdAt =14;
    @DataBaseField(type = "int(11)", fieldname = "createdAt", comment = "创建时间（秒）")
    private int createdAt;

    public PlayerChildBO() {
        id = 0;
        cid = 0L;
        consortId = 0L;
        initIntimacy = 0L;
        initResId = 0L;
        quality = 0L;
        attrType = 0;
        career = 0L;
        seatId = 0L;
        isGiftde = false;
        initStudyBonus = 0;
        name = "";
        lvl = 0;
        baseBonus = 0L;
        trainBonus = 0L;
        createdAt = 0;
    }

    public PlayerChildBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        consortId = rs.getLong(3);
        initIntimacy = rs.getLong(4);
        initResId = rs.getLong(5);
        quality = rs.getLong(6);
        attrType = rs.getInt(7);
        career = rs.getLong(8);
        seatId = rs.getLong(9);
        isGiftde = rs.getBoolean(10);
        initStudyBonus = rs.getInt(11);
        name = rs.getString(12);
        lvl = rs.getInt(13);
        baseBonus = rs.getLong(14);
        trainBonus = rs.getLong(15);
        createdAt = rs.getInt(16);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerChildBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `consortId`, `initIntimacy`, `initResId`, `quality`, `attrType`, `career`, `seatId`, `isGiftde`, `initStudyBonus`, `name`, `lvl`, `baseBonus`, `trainBonus`, `createdAt`";
    }

    @Override
    public String getTableName() {
        return "`player_child`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(initIntimacy).append("', ");
        strBuf.append("'").append(initResId).append("', ");
        strBuf.append("'").append(quality).append("', ");
        strBuf.append("'").append(attrType).append("', ");
        strBuf.append("'").append(career).append("', ");
        strBuf.append("'").append(seatId).append("', ");
        strBuf.append("'").append(isGiftde ? 1 : 0).append("', ");
        strBuf.append("'").append(initStudyBonus).append("', ");
        strBuf.append("'").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(lvl).append("', ");
        strBuf.append("'").append(baseBonus).append("', ");
        strBuf.append("'").append(trainBonus).append("', ");
        strBuf.append("'").append(createdAt).append("', ");
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

    // 关联家人ID
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

    // 子嗣初始形象配置
    public long getInitResId() { return this.initResId; }
    public void setInitResId(BM _bm, long initResId) {
        if(initResId==this.initResId) 
            return;
        this.initResId = initResId; 
        markField(_bm, FIELD_initResId); 
    }
    public void saveInitResId(BM _bm, long initResId) {
        if(initResId==this.initResId) 
            return;
        this.initResId = initResId;
        saveField(_bm, "initResId", initResId);
    }

    // 子嗣品质
    public long getQuality() { return this.quality; }
    public void setQuality(BM _bm, long quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality; 
        markField(_bm, FIELD_quality); 
    }
    public void saveQuality(BM _bm, long quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality;
        saveField(_bm, "quality", quality);
    }

    // 子嗣相性
    public int getAttrType() { return this.attrType; }
    public void setAttrType(BM _bm, int attrType) {
        if(attrType==this.attrType) 
            return;
        this.attrType = attrType; 
        markField(_bm, FIELD_attrType); 
    }
    public void saveAttrType(BM _bm, int attrType) {
        if(attrType==this.attrType) 
            return;
        this.attrType = attrType;
        saveField(_bm, "attrType", attrType);
    }

    // 子嗣职业
    public long getCareer() { return this.career; }
    public void setCareer(BM _bm, long career) {
        if(career==this.career) 
            return;
        this.career = career; 
        markField(_bm, FIELD_career); 
    }
    public void saveCareer(BM _bm, long career) {
        if(career==this.career) 
            return;
        this.career = career;
        saveField(_bm, "career", career);
    }

    // 训练房ID
    public long getSeatId() { return this.seatId; }
    public void setSeatId(BM _bm, long seatId) {
        if(seatId==this.seatId) 
            return;
        this.seatId = seatId; 
        markField(_bm, FIELD_seatId); 
    }
    public void saveSeatId(BM _bm, long seatId) {
        if(seatId==this.seatId) 
            return;
        this.seatId = seatId;
        saveField(_bm, "seatId", seatId);
    }

    // 是否卷王
    public boolean getIsGiftde() { return this.isGiftde; }
    public void setIsGiftde(BM _bm, boolean isGiftde) {
        if(isGiftde==this.isGiftde) 
            return;
        this.isGiftde = isGiftde; 
        markField(_bm, FIELD_isGiftde); 
    }
    public void saveIsGiftde(BM _bm, boolean isGiftde) {
        if(isGiftde==this.isGiftde) 
            return;
        this.isGiftde = isGiftde;
        saveField(_bm, "isGiftde", isGiftde ? 1 : 0);
    }

    // 教学经验加成（万分比）
    public int getInitStudyBonus() { return this.initStudyBonus; }
    public void setInitStudyBonus(BM _bm, int initStudyBonus) {
        if(initStudyBonus==this.initStudyBonus) 
            return;
        this.initStudyBonus = initStudyBonus; 
        markField(_bm, FIELD_initStudyBonus); 
    }
    public void saveInitStudyBonus(BM _bm, int initStudyBonus) {
        if(initStudyBonus==this.initStudyBonus) 
            return;
        this.initStudyBonus = initStudyBonus;
        saveField(_bm, "initStudyBonus", initStudyBonus);
    }

    // 子嗣名称
    public String getName() { return this.name; }
    public void setName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name; 
        markField(_bm, FIELD_name); 
    }
    public void saveName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name;
        saveField(_bm, "name", name);
    }

    // 子嗣等级
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

    // 子嗣基础收益
    public long getBaseBonus() { return this.baseBonus; }
    public void setBaseBonus(BM _bm, long baseBonus) {
        if(baseBonus==this.baseBonus) 
            return;
        this.baseBonus = baseBonus; 
        markField(_bm, FIELD_baseBonus); 
    }
    public void saveBaseBonus(BM _bm, long baseBonus) {
        if(baseBonus==this.baseBonus) 
            return;
        this.baseBonus = baseBonus;
        saveField(_bm, "baseBonus", baseBonus);
    }

    // 子嗣上课收益
    public long getTrainBonus() { return this.trainBonus; }
    public void setTrainBonus(BM _bm, long trainBonus) {
        if(trainBonus==this.trainBonus) 
            return;
        this.trainBonus = trainBonus; 
        markField(_bm, FIELD_trainBonus); 
    }
    public void saveTrainBonus(BM _bm, long trainBonus) {
        if(trainBonus==this.trainBonus) 
            return;
        this.trainBonus = trainBonus;
        saveField(_bm, "trainBonus", trainBonus);
    }

    // 创建时间（秒）
    public int getCreatedAt() { return this.createdAt; }
    public void setCreatedAt(BM _bm, int createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt; 
        markField(_bm, FIELD_createdAt); 
    }
    public void saveCreatedAt(BM _bm, int createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt;
        saveField(_bm, "createdAt", createdAt);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `initIntimacy` = '").append(initIntimacy).append("',");
        sBuilder.append(" `initResId` = '").append(initResId).append("',");
        sBuilder.append(" `quality` = '").append(quality).append("',");
        sBuilder.append(" `attrType` = '").append(attrType).append("',");
        sBuilder.append(" `career` = '").append(career).append("',");
        sBuilder.append(" `seatId` = '").append(seatId).append("',");
        sBuilder.append(" `isGiftde` = '").append(isGiftde ? 1 : 0).append("',");
        sBuilder.append(" `initStudyBonus` = '").append(initStudyBonus).append("',");
        sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `lvl` = '").append(lvl).append("',");
        sBuilder.append(" `baseBonus` = '").append(baseBonus).append("',");
        sBuilder.append(" `trainBonus` = '").append(trainBonus).append("',");
        sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_initIntimacy)) sBuilder.append(" `initIntimacy` = '").append(initIntimacy).append("',");
        if(isFieldMarked(FIELD_initResId)) sBuilder.append(" `initResId` = '").append(initResId).append("',");
        if(isFieldMarked(FIELD_quality)) sBuilder.append(" `quality` = '").append(quality).append("',");
        if(isFieldMarked(FIELD_attrType)) sBuilder.append(" `attrType` = '").append(attrType).append("',");
        if(isFieldMarked(FIELD_career)) sBuilder.append(" `career` = '").append(career).append("',");
        if(isFieldMarked(FIELD_seatId)) sBuilder.append(" `seatId` = '").append(seatId).append("',");
        if(isFieldMarked(FIELD_isGiftde)) sBuilder.append(" `isGiftde` = '").append(isGiftde ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_initStudyBonus)) sBuilder.append(" `initStudyBonus` = '").append(initStudyBonus).append("',");
        if(isFieldMarked(FIELD_name)) sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_lvl)) sBuilder.append(" `lvl` = '").append(lvl).append("',");
        if(isFieldMarked(FIELD_baseBonus)) sBuilder.append(" `baseBonus` = '").append(baseBonus).append("',");
        if(isFieldMarked(FIELD_trainBonus)) sBuilder.append(" `trainBonus` = '").append(trainBonus).append("',");
        if(isFieldMarked(FIELD_createdAt)) sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_child` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '关联家人ID',"
                + "`initIntimacy` bigint(20) NOT NULL DEFAULT '0' COMMENT '初始亲密度',"
                + "`initResId` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣初始形象配置',"
                + "`quality` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣品质',"
                + "`attrType` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣相性',"
                + "`career` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣职业',"
                + "`seatId` bigint(20) NOT NULL DEFAULT '0' COMMENT '训练房ID',"
                + "`isGiftde` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否卷王',"
                + "`initStudyBonus` int(11) NOT NULL DEFAULT '0' COMMENT '教学经验加成（万分比）',"
                + "`name` varchar(500) NOT NULL DEFAULT '' COMMENT '子嗣名称',"
                + "`lvl` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣等级',"
                + "`baseBonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣基础收益',"
                + "`trainBonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣上课收益',"
                + "`createdAt` int(11) NOT NULL DEFAULT '0' COMMENT '创建时间（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家子嗣数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//initIntimacy
        _size+=8;//initResId
        _size+=8;//quality
        _size+=4;//attrType
        _size+=8;//career
        _size+=8;//seatId
        _size+=1;//isGiftde
        _size+=4;//initStudyBonus
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);//name
        _size+=4;//lvl
        _size+=8;//baseBonus
        _size+=8;//trainBonus
        _size+=4;//createdAt
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
        buff.putLong(initIntimacy);
        buff.putLong(initResId);
        buff.putLong(quality);
        buff.putInt(attrType);
        buff.putLong(career);
        buff.putLong(seatId);
        buff.put((byte)(isGiftde?1:0));
        buff.putInt(initStudyBonus);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, name);
        buff.putInt(lvl);
        buff.putLong(baseBonus);
        buff.putLong(trainBonus);
        buff.putInt(createdAt);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        consortId=buff.getLong();
        initIntimacy=buff.getLong();
        initResId=buff.getLong();
        quality=buff.getLong();
        attrType=buff.getInt();
        career=buff.getLong();
        seatId=buff.getLong();
        isGiftde=(buff.get()==1);
        initStudyBonus=buff.getInt();
        name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        lvl=buff.getInt();
        baseBonus=buff.getLong();
        trainBonus=buff.getLong();
        createdAt=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
