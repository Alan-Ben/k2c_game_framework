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
public class PlayerConsortBusinessSkillBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_consortId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "妃子ID")
    private long consortId;

    public static final int FIELD_skillId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "skillId", comment = "技能ID")
    private long skillId;

    public static final int FIELD_proAdd =3;
    @DataBaseField(type = "int(11)", fieldname = "proAdd", comment = "属性加成")
    private int proAdd;

    public static final int FIELD_normalOpCount =4;
    @DataBaseField(type = "int(11)", fieldname = "normalOpCount", comment = "普通领悟次数")
    private int normalOpCount;

    public static final int FIELD_advanceOpCount =5;
    @DataBaseField(type = "int(11)", fieldname = "advanceOpCount", comment = "高级领悟次数")
    private int advanceOpCount;

    public PlayerConsortBusinessSkillBO() {
        id = 0;
        cid = 0L;
        consortId = 0L;
        skillId = 0L;
        proAdd = 0;
        normalOpCount = 0;
        advanceOpCount = 0;
    }

    public PlayerConsortBusinessSkillBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        consortId = rs.getLong(3);
        skillId = rs.getLong(4);
        proAdd = rs.getInt(5);
        normalOpCount = rs.getInt(6);
        advanceOpCount = rs.getInt(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerConsortBusinessSkillBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `consortId`, `skillId`, `proAdd`, `normalOpCount`, `advanceOpCount`";
    }

    @Override
    public String getTableName() {
        return "`player_consort_business_skill`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(skillId).append("', ");
        strBuf.append("'").append(proAdd).append("', ");
        strBuf.append("'").append(normalOpCount).append("', ");
        strBuf.append("'").append(advanceOpCount).append("', ");
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

    // 妃子ID
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

    // 技能ID
    public long getSkillId() { return this.skillId; }
    public void setSkillId(BM _bm, long skillId) {
        if(skillId==this.skillId) 
            return;
        this.skillId = skillId; 
        markField(_bm, FIELD_skillId); 
    }
    public void saveSkillId(BM _bm, long skillId) {
        if(skillId==this.skillId) 
            return;
        this.skillId = skillId;
        saveField(_bm, "skillId", skillId);
    }

    // 属性加成
    public int getProAdd() { return this.proAdd; }
    public void setProAdd(BM _bm, int proAdd) {
        if(proAdd==this.proAdd) 
            return;
        this.proAdd = proAdd; 
        markField(_bm, FIELD_proAdd); 
    }
    public void saveProAdd(BM _bm, int proAdd) {
        if(proAdd==this.proAdd) 
            return;
        this.proAdd = proAdd;
        saveField(_bm, "proAdd", proAdd);
    }

    // 普通领悟次数
    public int getNormalOpCount() { return this.normalOpCount; }
    public void setNormalOpCount(BM _bm, int normalOpCount) {
        if(normalOpCount==this.normalOpCount) 
            return;
        this.normalOpCount = normalOpCount; 
        markField(_bm, FIELD_normalOpCount); 
    }
    public void saveNormalOpCount(BM _bm, int normalOpCount) {
        if(normalOpCount==this.normalOpCount) 
            return;
        this.normalOpCount = normalOpCount;
        saveField(_bm, "normalOpCount", normalOpCount);
    }

    // 高级领悟次数
    public int getAdvanceOpCount() { return this.advanceOpCount; }
    public void setAdvanceOpCount(BM _bm, int advanceOpCount) {
        if(advanceOpCount==this.advanceOpCount) 
            return;
        this.advanceOpCount = advanceOpCount; 
        markField(_bm, FIELD_advanceOpCount); 
    }
    public void saveAdvanceOpCount(BM _bm, int advanceOpCount) {
        if(advanceOpCount==this.advanceOpCount) 
            return;
        this.advanceOpCount = advanceOpCount;
        saveField(_bm, "advanceOpCount", advanceOpCount);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `skillId` = '").append(skillId).append("',");
        sBuilder.append(" `proAdd` = '").append(proAdd).append("',");
        sBuilder.append(" `normalOpCount` = '").append(normalOpCount).append("',");
        sBuilder.append(" `advanceOpCount` = '").append(advanceOpCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_skillId)) sBuilder.append(" `skillId` = '").append(skillId).append("',");
        if(isFieldMarked(FIELD_proAdd)) sBuilder.append(" `proAdd` = '").append(proAdd).append("',");
        if(isFieldMarked(FIELD_normalOpCount)) sBuilder.append(" `normalOpCount` = '").append(normalOpCount).append("',");
        if(isFieldMarked(FIELD_advanceOpCount)) sBuilder.append(" `advanceOpCount` = '").append(advanceOpCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_consort_business_skill` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '妃子ID',"
                + "`skillId` bigint(20) NOT NULL DEFAULT '0' COMMENT '技能ID',"
                + "`proAdd` int(11) NOT NULL DEFAULT '0' COMMENT '属性加成',"
                + "`normalOpCount` int(11) NOT NULL DEFAULT '0' COMMENT '普通领悟次数',"
                + "`advanceOpCount` int(11) NOT NULL DEFAULT '0' COMMENT '高级领悟次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家家人经营技能数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//skillId
        _size+=4;//proAdd
        _size+=4;//normalOpCount
        _size+=4;//advanceOpCount
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
        buff.putLong(skillId);
        buff.putInt(proAdd);
        buff.putInt(normalOpCount);
        buff.putInt(advanceOpCount);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        consortId=buff.getLong();
        skillId=buff.getLong();
        proAdd=buff.getInt();
        normalOpCount=buff.getInt();
        advanceOpCount=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
