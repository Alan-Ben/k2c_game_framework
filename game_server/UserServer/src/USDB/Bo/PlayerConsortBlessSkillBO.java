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
public class PlayerConsortBlessSkillBO extends BaseBO {
    
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

    public static final int FIELD_skillLvl =3;
    @DataBaseField(type = "int(11)", fieldname = "skillLvl", comment = "技能等级")
    private int skillLvl;

    public PlayerConsortBlessSkillBO() {
        id = 0;
        cid = 0L;
        consortId = 0L;
        skillId = 0L;
        skillLvl = 0;
    }

    public PlayerConsortBlessSkillBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        consortId = rs.getLong(3);
        skillId = rs.getLong(4);
        skillLvl = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerConsortBlessSkillBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `consortId`, `skillId`, `skillLvl`";
    }

    @Override
    public String getTableName() {
        return "`player_consort_bless_skill`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(skillId).append("', ");
        strBuf.append("'").append(skillLvl).append("', ");
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

    // 技能等级
    public int getSkillLvl() { return this.skillLvl; }
    public void setSkillLvl(BM _bm, int skillLvl) {
        if(skillLvl==this.skillLvl) 
            return;
        this.skillLvl = skillLvl; 
        markField(_bm, FIELD_skillLvl); 
    }
    public void saveSkillLvl(BM _bm, int skillLvl) {
        if(skillLvl==this.skillLvl) 
            return;
        this.skillLvl = skillLvl;
        saveField(_bm, "skillLvl", skillLvl);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `skillId` = '").append(skillId).append("',");
        sBuilder.append(" `skillLvl` = '").append(skillLvl).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_skillId)) sBuilder.append(" `skillId` = '").append(skillId).append("',");
        if(isFieldMarked(FIELD_skillLvl)) sBuilder.append(" `skillLvl` = '").append(skillLvl).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_consort_bless_skill` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '妃子ID',"
                + "`skillId` bigint(20) NOT NULL DEFAULT '0' COMMENT '技能ID',"
                + "`skillLvl` int(11) NOT NULL DEFAULT '0' COMMENT '技能等级',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家家人加护技能数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//skillLvl
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
        buff.putInt(skillLvl);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        consortId=buff.getLong();
        skillId=buff.getLong();
        skillLvl=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
