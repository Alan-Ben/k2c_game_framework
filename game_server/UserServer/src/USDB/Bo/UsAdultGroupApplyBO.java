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
public class UsAdultGroupApplyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_applyAdultId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "applyAdultId", comment = "请求子嗣实例ID")
    private long applyAdultId;

    public static final int FIELD_applyCid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "applyCid", comment = "请求玩家CID")
    private long applyCid;

    public static final int FIELD_applyCname =2;
    @DataBaseField(type = "varchar(500)", fieldname = "applyCname", comment = "请求玩家昵称")
    private String applyCname;

    public static final int FIELD_initResId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "initResId", comment = "请求子嗣初始形象配置，用于确认子嗣形象列表")
    private long initResId;

    public static final int FIELD_quality =4;
    @DataBaseField(type = "bigint(20)", fieldname = "quality", comment = "请求子嗣品质")
    private long quality;

    public static final int FIELD_attrType =5;
    @DataBaseField(type = "int(11)", fieldname = "attrType", comment = "请求子嗣相性")
    private int attrType;

    public static final int FIELD_career =6;
    @DataBaseField(type = "bigint(20)", fieldname = "career", comment = "请求子嗣职业")
    private long career;

    public static final int FIELD_isGiftde =7;
    @DataBaseField(type = "tinyint(1)", fieldname = "isGiftde", comment = "是否卷王")
    private boolean isGiftde;

    public static final int FIELD_name =8;
    @DataBaseField(type = "varchar(500)", fieldname = "name", comment = "请求子嗣名称")
    private String name;

    public static final int FIELD_bonus =9;
    @DataBaseField(type = "bigint(20)", fieldname = "bonus", comment = "子嗣收益")
    private long bonus;

    public static final int FIELD_marriedItem =10;
    @DataBaseField(type = "blob", fieldname = "marriedItem", comment = "联姻奖励")
    private byte[] marriedItem;

    public static final int FIELD_applyExpiredTs =11;
    @DataBaseField(type = "int(11)", fieldname = "applyExpiredTs", comment = "发起请求截至时间戳（秒）")
    private int applyExpiredTs;

    public static final int FIELD_minBonus =12;
    @DataBaseField(type = "bigint(20)", fieldname = "minBonus", comment = "对方子嗣允许的最小收益")
    private long minBonus;

    public UsAdultGroupApplyBO() {
        id = 0;
        applyAdultId = 0L;
        applyCid = 0L;
        applyCname = "";
        initResId = 0L;
        quality = 0L;
        attrType = 0;
        career = 0L;
        isGiftde = false;
        name = "";
        bonus = 0L;
        marriedItem = null;
        applyExpiredTs = 0;
        minBonus = 0L;
    }

    public UsAdultGroupApplyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        applyAdultId = rs.getLong(2);
        applyCid = rs.getLong(3);
        applyCname = rs.getString(4);
        initResId = rs.getLong(5);
        quality = rs.getLong(6);
        attrType = rs.getInt(7);
        career = rs.getLong(8);
        isGiftde = rs.getBoolean(9);
        name = rs.getString(10);
        bonus = rs.getLong(11);
        marriedItem = rs.getBytes(12);
        applyExpiredTs = rs.getInt(13);
        minBonus = rs.getLong(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsAdultGroupApplyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `applyAdultId`, `applyCid`, `applyCname`, `initResId`, `quality`, `attrType`, `career`, `isGiftde`, `name`, `bonus`, `marriedItem`, `applyExpiredTs`, `minBonus`";
    }

    @Override
    public String getTableName() {
        return "`us_adult_group_apply`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(applyAdultId).append("', ");
        strBuf.append("'").append(applyCid).append("', ");
        strBuf.append("'").append(applyCname == null ? null : applyCname.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(initResId).append("', ");
        strBuf.append("'").append(quality).append("', ");
        strBuf.append("'").append(attrType).append("', ");
        strBuf.append("'").append(career).append("', ");
        strBuf.append("'").append(isGiftde ? 1 : 0).append("', ");
        strBuf.append("'").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(bonus).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(applyExpiredTs).append("', ");
        strBuf.append("'").append(minBonus).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(marriedItem);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_marriedItem)) ret.add(marriedItem);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 请求子嗣实例ID
    public long getApplyAdultId() { return this.applyAdultId; }
    public void setApplyAdultId(BM _bm, long applyAdultId) {
        if(applyAdultId==this.applyAdultId) 
            return;
        this.applyAdultId = applyAdultId; 
        markField(_bm, FIELD_applyAdultId); 
    }
    public void saveApplyAdultId(BM _bm, long applyAdultId) {
        if(applyAdultId==this.applyAdultId) 
            return;
        this.applyAdultId = applyAdultId;
        saveField(_bm, "applyAdultId", applyAdultId);
    }

    // 请求玩家CID
    public long getApplyCid() { return this.applyCid; }
    public void setApplyCid(BM _bm, long applyCid) {
        if(applyCid==this.applyCid) 
            return;
        this.applyCid = applyCid; 
        markField(_bm, FIELD_applyCid); 
    }
    public void saveApplyCid(BM _bm, long applyCid) {
        if(applyCid==this.applyCid) 
            return;
        this.applyCid = applyCid;
        saveField(_bm, "applyCid", applyCid);
    }

    // 请求玩家昵称
    public String getApplyCname() { return this.applyCname; }
    public void setApplyCname(BM _bm, String applyCname) {
        if(applyCname.equals(this.applyCname)) 
            return;
        this.applyCname = applyCname; 
        markField(_bm, FIELD_applyCname); 
    }
    public void saveApplyCname(BM _bm, String applyCname) {
        if(applyCname.equals(this.applyCname)) 
            return;
        this.applyCname = applyCname;
        saveField(_bm, "applyCname", applyCname);
    }

    // 请求子嗣初始形象配置，用于确认子嗣形象列表
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

    // 请求子嗣品质
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

    // 请求子嗣相性
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

    // 请求子嗣职业
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

    // 请求子嗣名称
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

    // 子嗣收益
    public long getBonus() { return this.bonus; }
    public void setBonus(BM _bm, long bonus) {
        if(bonus==this.bonus) 
            return;
        this.bonus = bonus; 
        markField(_bm, FIELD_bonus); 
    }
    public void saveBonus(BM _bm, long bonus) {
        if(bonus==this.bonus) 
            return;
        this.bonus = bonus;
        saveField(_bm, "bonus", bonus);
    }

    // 联姻奖励
    public byte[] getMarriedItem() { return this.marriedItem; }
    public void setMarriedItem(BM _bm, byte[] marriedItem) {
        if(marriedItem==this.marriedItem) 
            return;
        this.marriedItem = marriedItem; 
        markField(_bm, FIELD_marriedItem); 
    }
    public void saveMarriedItem(BM _bm, byte[] marriedItem) {
        if(marriedItem==this.marriedItem) 
            return;
        this.marriedItem = marriedItem;
        saveFieldBytes(_bm, "marriedItem", marriedItem);
    }

    // 发起请求截至时间戳（秒）
    public int getApplyExpiredTs() { return this.applyExpiredTs; }
    public void setApplyExpiredTs(BM _bm, int applyExpiredTs) {
        if(applyExpiredTs==this.applyExpiredTs) 
            return;
        this.applyExpiredTs = applyExpiredTs; 
        markField(_bm, FIELD_applyExpiredTs); 
    }
    public void saveApplyExpiredTs(BM _bm, int applyExpiredTs) {
        if(applyExpiredTs==this.applyExpiredTs) 
            return;
        this.applyExpiredTs = applyExpiredTs;
        saveField(_bm, "applyExpiredTs", applyExpiredTs);
    }

    // 对方子嗣允许的最小收益
    public long getMinBonus() { return this.minBonus; }
    public void setMinBonus(BM _bm, long minBonus) {
        if(minBonus==this.minBonus) 
            return;
        this.minBonus = minBonus; 
        markField(_bm, FIELD_minBonus); 
    }
    public void saveMinBonus(BM _bm, long minBonus) {
        if(minBonus==this.minBonus) 
            return;
        this.minBonus = minBonus;
        saveField(_bm, "minBonus", minBonus);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `applyAdultId` = '").append(applyAdultId).append("',");
        sBuilder.append(" `applyCid` = '").append(applyCid).append("',");
        sBuilder.append(" `applyCname` = '").append(applyCname == null ? null : applyCname.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `initResId` = '").append(initResId).append("',");
        sBuilder.append(" `quality` = '").append(quality).append("',");
        sBuilder.append(" `attrType` = '").append(attrType).append("',");
        sBuilder.append(" `career` = '").append(career).append("',");
        sBuilder.append(" `isGiftde` = '").append(isGiftde ? 1 : 0).append("',");
        sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `bonus` = '").append(bonus).append("',");
        sBuilder.append(" `marriedItem` = ?,");
        sBuilder.append(" `applyExpiredTs` = '").append(applyExpiredTs).append("',");
        sBuilder.append(" `minBonus` = '").append(minBonus).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_applyAdultId)) sBuilder.append(" `applyAdultId` = '").append(applyAdultId).append("',");
        if(isFieldMarked(FIELD_applyCid)) sBuilder.append(" `applyCid` = '").append(applyCid).append("',");
        if(isFieldMarked(FIELD_applyCname)) sBuilder.append(" `applyCname` = '").append(applyCname == null ? null : applyCname.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_initResId)) sBuilder.append(" `initResId` = '").append(initResId).append("',");
        if(isFieldMarked(FIELD_quality)) sBuilder.append(" `quality` = '").append(quality).append("',");
        if(isFieldMarked(FIELD_attrType)) sBuilder.append(" `attrType` = '").append(attrType).append("',");
        if(isFieldMarked(FIELD_career)) sBuilder.append(" `career` = '").append(career).append("',");
        if(isFieldMarked(FIELD_isGiftde)) sBuilder.append(" `isGiftde` = '").append(isGiftde ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_name)) sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_bonus)) sBuilder.append(" `bonus` = '").append(bonus).append("',");
        if(isFieldMarked(FIELD_marriedItem)) sBuilder.append(" `marriedItem` = ?,");
        if(isFieldMarked(FIELD_applyExpiredTs)) sBuilder.append(" `applyExpiredTs` = '").append(applyExpiredTs).append("',");
        if(isFieldMarked(FIELD_minBonus)) sBuilder.append(" `minBonus` = '").append(minBonus).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_adult_group_apply` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`applyAdultId` bigint(20) NOT NULL DEFAULT '0' COMMENT '请求子嗣实例ID',"
                + "`applyCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '请求玩家CID',"
                + "`applyCname` varchar(500) NOT NULL DEFAULT '' COMMENT '请求玩家昵称',"
                + "`initResId` bigint(20) NOT NULL DEFAULT '0' COMMENT '请求子嗣初始形象配置，用于确认子嗣形象列表',"
                + "`quality` bigint(20) NOT NULL DEFAULT '0' COMMENT '请求子嗣品质',"
                + "`attrType` int(11) NOT NULL DEFAULT '0' COMMENT '请求子嗣相性',"
                + "`career` bigint(20) NOT NULL DEFAULT '0' COMMENT '请求子嗣职业',"
                + "`isGiftde` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否卷王',"
                + "`name` varchar(500) NOT NULL DEFAULT '' COMMENT '请求子嗣名称',"
                + "`bonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣收益',"
                + "`marriedItem` blob NULL COMMENT '联姻奖励',"
                + "`applyExpiredTs` int(11) NOT NULL DEFAULT '0' COMMENT '发起请求截至时间戳（秒）',"
                + "`minBonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '对方子嗣允许的最小收益',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联姻池请求数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//applyAdultId
        _size+=8;//applyCid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(applyCname);//applyCname
        _size+=8;//initResId
        _size+=8;//quality
        _size+=4;//attrType
        _size+=8;//career
        _size+=1;//isGiftde
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);//name
        _size+=8;//bonus
        _size+=2;_size+=marriedItem.length;//marriedItem
        _size+=4;//applyExpiredTs
        _size+=8;//minBonus
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(applyAdultId);
        buff.putLong(applyCid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, applyCname);
        buff.putLong(initResId);
        buff.putLong(quality);
        buff.putInt(attrType);
        buff.putLong(career);
        buff.put((byte)(isGiftde?1:0));
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, name);
        buff.putLong(bonus);
        buff.putShort((short)(marriedItem == null ? 0 : marriedItem.length));if(null != marriedItem){buff.put(marriedItem);}
        buff.putInt(applyExpiredTs);
        buff.putLong(minBonus);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        applyAdultId=buff.getLong();
        applyCid=buff.getLong();
        applyCname=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        initResId=buff.getLong();
        quality=buff.getLong();
        attrType=buff.getInt();
        career=buff.getLong();
        isGiftde=(buff.get()==1);
        name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        bonus=buff.getLong();
        int marriedItem_count = buff.getShort();if(marriedItem_count>0){marriedItem = new byte[marriedItem_count];buff.get(marriedItem);}
        applyExpiredTs=buff.getInt();
        minBonus=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
