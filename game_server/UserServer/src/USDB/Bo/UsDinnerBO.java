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
public class UsDinnerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_dinner_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "dinner_id", comment = "宴会配置ID")
    private long dinner_id;

    public static final int FIELD_owner_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "owner_cid", comment = "开宴玩家CID")
    private long owner_cid;

    public static final int FIELD_dinner_type =2;
    @DataBaseField(type = "int(11)", fieldname = "dinner_type", comment = "宴会类型")
    private int dinner_type;

    public static final int FIELD_start_ts =3;
    @DataBaseField(type = "int(11)", fieldname = "start_ts", comment = "开始时间戳（秒）")
    private int start_ts;

    public static final int FIELD_end_ts =4;
    @DataBaseField(type = "int(11)", fieldname = "end_ts", comment = "结束时间（秒）")
    private int end_ts;

    public static final int FIELD_permit_type =5;
    @DataBaseField(type = "int(11)", fieldname = "permit_type", comment = "凭证类型")
    private int permit_type;

    public static final int FIELD_permit_type_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "permit_type_id", comment = "凭证额外类型ID")
    private long permit_type_id;

    public static final int FIELD_score_add_per =7;
    @DataBaseField(type = "bigint(20)", fieldname = "score_add_per", comment = "人气值加成")
    private long score_add_per;

    public static final int FIELD_hero_id_list =8;
    @DataBaseField(type = "blob", fieldname = "hero_id_list", comment = "开宴玩家拥有的大臣ID列表")
    private byte[] hero_id_list;

    public UsDinnerBO() {
        id = 0;
        dinner_id = 0L;
        owner_cid = 0L;
        dinner_type = 0;
        start_ts = 0;
        end_ts = 0;
        permit_type = 0;
        permit_type_id = 0L;
        score_add_per = 0L;
        hero_id_list = null;
    }

    public UsDinnerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        dinner_id = rs.getLong(2);
        owner_cid = rs.getLong(3);
        dinner_type = rs.getInt(4);
        start_ts = rs.getInt(5);
        end_ts = rs.getInt(6);
        permit_type = rs.getInt(7);
        permit_type_id = rs.getLong(8);
        score_add_per = rs.getLong(9);
        hero_id_list = rs.getBytes(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsDinnerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `dinner_id`, `owner_cid`, `dinner_type`, `start_ts`, `end_ts`, `permit_type`, `permit_type_id`, `score_add_per`, `hero_id_list`";
    }

    @Override
    public String getTableName() {
        return "`us_dinner`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(dinner_id).append("', ");
        strBuf.append("'").append(owner_cid).append("', ");
        strBuf.append("'").append(dinner_type).append("', ");
        strBuf.append("'").append(start_ts).append("', ");
        strBuf.append("'").append(end_ts).append("', ");
        strBuf.append("'").append(permit_type).append("', ");
        strBuf.append("'").append(permit_type_id).append("', ");
        strBuf.append("'").append(score_add_per).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(hero_id_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_hero_id_list)) ret.add(hero_id_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 宴会配置ID
    public long getDinnerId() { return this.dinner_id; }
    public void setDinnerId(BM _bm, long dinner_id) {
        if(dinner_id==this.dinner_id) 
            return;
        this.dinner_id = dinner_id; 
        markField(_bm, FIELD_dinner_id); 
    }
    public void saveDinnerId(BM _bm, long dinner_id) {
        if(dinner_id==this.dinner_id) 
            return;
        this.dinner_id = dinner_id;
        saveField(_bm, "dinner_id", dinner_id);
    }

    // 开宴玩家CID
    public long getOwnerCid() { return this.owner_cid; }
    public void setOwnerCid(BM _bm, long owner_cid) {
        if(owner_cid==this.owner_cid) 
            return;
        this.owner_cid = owner_cid; 
        markField(_bm, FIELD_owner_cid); 
    }
    public void saveOwnerCid(BM _bm, long owner_cid) {
        if(owner_cid==this.owner_cid) 
            return;
        this.owner_cid = owner_cid;
        saveField(_bm, "owner_cid", owner_cid);
    }

    // 宴会类型
    public int getDinnerType() { return this.dinner_type; }
    public void setDinnerType(BM _bm, int dinner_type) {
        if(dinner_type==this.dinner_type) 
            return;
        this.dinner_type = dinner_type; 
        markField(_bm, FIELD_dinner_type); 
    }
    public void saveDinnerType(BM _bm, int dinner_type) {
        if(dinner_type==this.dinner_type) 
            return;
        this.dinner_type = dinner_type;
        saveField(_bm, "dinner_type", dinner_type);
    }

    // 开始时间戳（秒）
    public int getStartTs() { return this.start_ts; }
    public void setStartTs(BM _bm, int start_ts) {
        if(start_ts==this.start_ts) 
            return;
        this.start_ts = start_ts; 
        markField(_bm, FIELD_start_ts); 
    }
    public void saveStartTs(BM _bm, int start_ts) {
        if(start_ts==this.start_ts) 
            return;
        this.start_ts = start_ts;
        saveField(_bm, "start_ts", start_ts);
    }

    // 结束时间（秒）
    public int getEndTs() { return this.end_ts; }
    public void setEndTs(BM _bm, int end_ts) {
        if(end_ts==this.end_ts) 
            return;
        this.end_ts = end_ts; 
        markField(_bm, FIELD_end_ts); 
    }
    public void saveEndTs(BM _bm, int end_ts) {
        if(end_ts==this.end_ts) 
            return;
        this.end_ts = end_ts;
        saveField(_bm, "end_ts", end_ts);
    }

    // 凭证类型
    public int getPermitType() { return this.permit_type; }
    public void setPermitType(BM _bm, int permit_type) {
        if(permit_type==this.permit_type) 
            return;
        this.permit_type = permit_type; 
        markField(_bm, FIELD_permit_type); 
    }
    public void savePermitType(BM _bm, int permit_type) {
        if(permit_type==this.permit_type) 
            return;
        this.permit_type = permit_type;
        saveField(_bm, "permit_type", permit_type);
    }

    // 凭证额外类型ID
    public long getPermitTypeId() { return this.permit_type_id; }
    public void setPermitTypeId(BM _bm, long permit_type_id) {
        if(permit_type_id==this.permit_type_id) 
            return;
        this.permit_type_id = permit_type_id; 
        markField(_bm, FIELD_permit_type_id); 
    }
    public void savePermitTypeId(BM _bm, long permit_type_id) {
        if(permit_type_id==this.permit_type_id) 
            return;
        this.permit_type_id = permit_type_id;
        saveField(_bm, "permit_type_id", permit_type_id);
    }

    // 人气值加成
    public long getScoreAddPer() { return this.score_add_per; }
    public void setScoreAddPer(BM _bm, long score_add_per) {
        if(score_add_per==this.score_add_per) 
            return;
        this.score_add_per = score_add_per; 
        markField(_bm, FIELD_score_add_per); 
    }
    public void saveScoreAddPer(BM _bm, long score_add_per) {
        if(score_add_per==this.score_add_per) 
            return;
        this.score_add_per = score_add_per;
        saveField(_bm, "score_add_per", score_add_per);
    }

    // 开宴玩家拥有的大臣ID列表
    public byte[] getHeroIdList() { return this.hero_id_list; }
    public void setHeroIdList(BM _bm, byte[] hero_id_list) {
        if(hero_id_list==this.hero_id_list) 
            return;
        this.hero_id_list = hero_id_list; 
        markField(_bm, FIELD_hero_id_list); 
    }
    public void saveHeroIdList(BM _bm, byte[] hero_id_list) {
        if(hero_id_list==this.hero_id_list) 
            return;
        this.hero_id_list = hero_id_list;
        saveFieldBytes(_bm, "hero_id_list", hero_id_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `dinner_id` = '").append(dinner_id).append("',");
        sBuilder.append(" `owner_cid` = '").append(owner_cid).append("',");
        sBuilder.append(" `dinner_type` = '").append(dinner_type).append("',");
        sBuilder.append(" `start_ts` = '").append(start_ts).append("',");
        sBuilder.append(" `end_ts` = '").append(end_ts).append("',");
        sBuilder.append(" `permit_type` = '").append(permit_type).append("',");
        sBuilder.append(" `permit_type_id` = '").append(permit_type_id).append("',");
        sBuilder.append(" `score_add_per` = '").append(score_add_per).append("',");
        sBuilder.append(" `hero_id_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_dinner_id)) sBuilder.append(" `dinner_id` = '").append(dinner_id).append("',");
        if(isFieldMarked(FIELD_owner_cid)) sBuilder.append(" `owner_cid` = '").append(owner_cid).append("',");
        if(isFieldMarked(FIELD_dinner_type)) sBuilder.append(" `dinner_type` = '").append(dinner_type).append("',");
        if(isFieldMarked(FIELD_start_ts)) sBuilder.append(" `start_ts` = '").append(start_ts).append("',");
        if(isFieldMarked(FIELD_end_ts)) sBuilder.append(" `end_ts` = '").append(end_ts).append("',");
        if(isFieldMarked(FIELD_permit_type)) sBuilder.append(" `permit_type` = '").append(permit_type).append("',");
        if(isFieldMarked(FIELD_permit_type_id)) sBuilder.append(" `permit_type_id` = '").append(permit_type_id).append("',");
        if(isFieldMarked(FIELD_score_add_per)) sBuilder.append(" `score_add_per` = '").append(score_add_per).append("',");
        if(isFieldMarked(FIELD_hero_id_list)) sBuilder.append(" `hero_id_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_dinner` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`dinner_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '宴会配置ID',"
                + "`owner_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '开宴玩家CID',"
                + "`dinner_type` int(11) NOT NULL DEFAULT '0' COMMENT '宴会类型',"
                + "`start_ts` int(11) NOT NULL DEFAULT '0' COMMENT '开始时间戳（秒）',"
                + "`end_ts` int(11) NOT NULL DEFAULT '0' COMMENT '结束时间（秒）',"
                + "`permit_type` int(11) NOT NULL DEFAULT '0' COMMENT '凭证类型',"
                + "`permit_type_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '凭证额外类型ID',"
                + "`score_add_per` bigint(20) NOT NULL DEFAULT '0' COMMENT '人气值加成',"
                + "`hero_id_list` blob NULL COMMENT '开宴玩家拥有的大臣ID列表',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='宴会数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//dinner_id
        _size+=8;//owner_cid
        _size+=4;//dinner_type
        _size+=4;//start_ts
        _size+=4;//end_ts
        _size+=4;//permit_type
        _size+=8;//permit_type_id
        _size+=8;//score_add_per
        _size+=2;_size+=hero_id_list.length;//hero_id_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(dinner_id);
        buff.putLong(owner_cid);
        buff.putInt(dinner_type);
        buff.putInt(start_ts);
        buff.putInt(end_ts);
        buff.putInt(permit_type);
        buff.putLong(permit_type_id);
        buff.putLong(score_add_per);
        buff.putShort((short)(hero_id_list == null ? 0 : hero_id_list.length));if(null != hero_id_list){buff.put(hero_id_list);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        dinner_id=buff.getLong();
        owner_cid=buff.getLong();
        dinner_type=buff.getInt();
        start_ts=buff.getInt();
        end_ts=buff.getInt();
        permit_type=buff.getInt();
        permit_type_id=buff.getLong();
        score_add_per=buff.getLong();
        int hero_id_list_count = buff.getShort();if(hero_id_list_count>0){hero_id_list = new byte[hero_id_list_count];buff.get(hero_id_list);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
