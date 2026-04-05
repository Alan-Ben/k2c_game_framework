package CSDB.Bo;
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
public class SeasonRankDumpBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_seasonId =0;
    @DataBaseField(type = "int(11)", fieldname = "seasonId", comment = "赛季ID")
    private int seasonId;

    public static final int FIELD_uid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "uid", comment = "玩家uid")
    private long uid;

    public static final int FIELD_rank =2;
    @DataBaseField(type = "int(11)", fieldname = "rank", comment = "玩家排名")
    private int rank;

    public static final int FIELD_serverId =3;
    @DataBaseField(type = "int(11)", fieldname = "serverId", comment = "服务器ID")
    private int serverId;

    public static final int FIELD_playername =4;
    @DataBaseField(type = "varchar(500)", fieldname = "playername", comment = "玩家名字")
    private String playername;

    public static final int FIELD_icon =5;
    @DataBaseField(type = "bigint(20)", fieldname = "icon", comment = "玩家头像")
    private long icon;

    public static final int FIELD_iconBgk =6;
    @DataBaseField(type = "bigint(20)", fieldname = "iconBgk", comment = "玩家头像框")
    private long iconBgk;

    public static final int FIELD_grades =7;
    @DataBaseField(type = "int(11)", fieldname = "grades", comment = "玩家段位")
    private int grades;

    public static final int FIELD_starhoner =8;
    @DataBaseField(type = "int(11)", fieldname = "starhoner", comment = "段位星耀值")
    private int starhoner;

    public static final int FIELD_legendscore =9;
    @DataBaseField(type = "bigint(20)", fieldname = "legendscore", comment = "传说积分")
    private long legendscore;

    public SeasonRankDumpBO() {
        id = 0;
        seasonId = 0;
        uid = 0L;
        rank = 0;
        serverId = 0;
        playername = "";
        icon = 0L;
        iconBgk = 0L;
        grades = 0;
        starhoner = 0;
        legendscore = 0L;
    }

    public SeasonRankDumpBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        seasonId = rs.getInt(2);
        uid = rs.getLong(3);
        rank = rs.getInt(4);
        serverId = rs.getInt(5);
        playername = rs.getString(6);
        icon = rs.getLong(7);
        iconBgk = rs.getLong(8);
        grades = rs.getInt(9);
        starhoner = rs.getInt(10);
        legendscore = rs.getLong(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new SeasonRankDumpBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `seasonId`, `uid`, `rank`, `serverId`, `playername`, `icon`, `iconBgk`, `grades`, `starhoner`, `legendscore`";
    }

    @Override
    public String getTableName() {
        return "`seasonRankDump`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(seasonId).append("', ");
        strBuf.append("'").append(uid).append("', ");
        strBuf.append("'").append(rank).append("', ");
        strBuf.append("'").append(serverId).append("', ");
        strBuf.append("'").append(playername == null ? null : playername.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(icon).append("', ");
        strBuf.append("'").append(iconBgk).append("', ");
        strBuf.append("'").append(grades).append("', ");
        strBuf.append("'").append(starhoner).append("', ");
        strBuf.append("'").append(legendscore).append("', ");
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

    // 赛季ID
    public int getSeasonId() { return this.seasonId; }
    public void setSeasonId(BM _bm, int seasonId) {
        if(seasonId==this.seasonId) 
            return;
        this.seasonId = seasonId; 
        markField(_bm, FIELD_seasonId); 
    }
    public void saveSeasonId(BM _bm, int seasonId) {
        if(seasonId==this.seasonId) 
            return;
        this.seasonId = seasonId;
        saveField(_bm, "seasonId", seasonId);
    }

    // 玩家uid
    public long getUid() { return this.uid; }
    public void setUid(BM _bm, long uid) {
        if(uid==this.uid) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, long uid) {
        if(uid==this.uid) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // 玩家排名
    public int getRank() { return this.rank; }
    public void setRank(BM _bm, int rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank; 
        markField(_bm, FIELD_rank); 
    }
    public void saveRank(BM _bm, int rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank;
        saveField(_bm, "rank", rank);
    }

    // 服务器ID
    public int getServerId() { return this.serverId; }
    public void setServerId(BM _bm, int serverId) {
        if(serverId==this.serverId) 
            return;
        this.serverId = serverId; 
        markField(_bm, FIELD_serverId); 
    }
    public void saveServerId(BM _bm, int serverId) {
        if(serverId==this.serverId) 
            return;
        this.serverId = serverId;
        saveField(_bm, "serverId", serverId);
    }

    // 玩家名字
    public String getPlayername() { return this.playername; }
    public void setPlayername(BM _bm, String playername) {
        if(playername.equals(this.playername)) 
            return;
        this.playername = playername; 
        markField(_bm, FIELD_playername); 
    }
    public void savePlayername(BM _bm, String playername) {
        if(playername.equals(this.playername)) 
            return;
        this.playername = playername;
        saveField(_bm, "playername", playername);
    }

    // 玩家头像
    public long getIcon() { return this.icon; }
    public void setIcon(BM _bm, long icon) {
        if(icon==this.icon) 
            return;
        this.icon = icon; 
        markField(_bm, FIELD_icon); 
    }
    public void saveIcon(BM _bm, long icon) {
        if(icon==this.icon) 
            return;
        this.icon = icon;
        saveField(_bm, "icon", icon);
    }

    // 玩家头像框
    public long getIconBgk() { return this.iconBgk; }
    public void setIconBgk(BM _bm, long iconBgk) {
        if(iconBgk==this.iconBgk) 
            return;
        this.iconBgk = iconBgk; 
        markField(_bm, FIELD_iconBgk); 
    }
    public void saveIconBgk(BM _bm, long iconBgk) {
        if(iconBgk==this.iconBgk) 
            return;
        this.iconBgk = iconBgk;
        saveField(_bm, "iconBgk", iconBgk);
    }

    // 玩家段位
    public int getGrades() { return this.grades; }
    public void setGrades(BM _bm, int grades) {
        if(grades==this.grades) 
            return;
        this.grades = grades; 
        markField(_bm, FIELD_grades); 
    }
    public void saveGrades(BM _bm, int grades) {
        if(grades==this.grades) 
            return;
        this.grades = grades;
        saveField(_bm, "grades", grades);
    }

    // 段位星耀值
    public int getStarhoner() { return this.starhoner; }
    public void setStarhoner(BM _bm, int starhoner) {
        if(starhoner==this.starhoner) 
            return;
        this.starhoner = starhoner; 
        markField(_bm, FIELD_starhoner); 
    }
    public void saveStarhoner(BM _bm, int starhoner) {
        if(starhoner==this.starhoner) 
            return;
        this.starhoner = starhoner;
        saveField(_bm, "starhoner", starhoner);
    }

    // 传说积分
    public long getLegendscore() { return this.legendscore; }
    public void setLegendscore(BM _bm, long legendscore) {
        if(legendscore==this.legendscore) 
            return;
        this.legendscore = legendscore; 
        markField(_bm, FIELD_legendscore); 
    }
    public void saveLegendscore(BM _bm, long legendscore) {
        if(legendscore==this.legendscore) 
            return;
        this.legendscore = legendscore;
        saveField(_bm, "legendscore", legendscore);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `seasonId` = '").append(seasonId).append("',");
        sBuilder.append(" `uid` = '").append(uid).append("',");
        sBuilder.append(" `rank` = '").append(rank).append("',");
        sBuilder.append(" `serverId` = '").append(serverId).append("',");
        sBuilder.append(" `playername` = '").append(playername == null ? null : playername.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `icon` = '").append(icon).append("',");
        sBuilder.append(" `iconBgk` = '").append(iconBgk).append("',");
        sBuilder.append(" `grades` = '").append(grades).append("',");
        sBuilder.append(" `starhoner` = '").append(starhoner).append("',");
        sBuilder.append(" `legendscore` = '").append(legendscore).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_seasonId)) sBuilder.append(" `seasonId` = '").append(seasonId).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid).append("',");
        if(isFieldMarked(FIELD_rank)) sBuilder.append(" `rank` = '").append(rank).append("',");
        if(isFieldMarked(FIELD_serverId)) sBuilder.append(" `serverId` = '").append(serverId).append("',");
        if(isFieldMarked(FIELD_playername)) sBuilder.append(" `playername` = '").append(playername == null ? null : playername.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_icon)) sBuilder.append(" `icon` = '").append(icon).append("',");
        if(isFieldMarked(FIELD_iconBgk)) sBuilder.append(" `iconBgk` = '").append(iconBgk).append("',");
        if(isFieldMarked(FIELD_grades)) sBuilder.append(" `grades` = '").append(grades).append("',");
        if(isFieldMarked(FIELD_starhoner)) sBuilder.append(" `starhoner` = '").append(starhoner).append("',");
        if(isFieldMarked(FIELD_legendscore)) sBuilder.append(" `legendscore` = '").append(legendscore).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `seasonRankDump` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`seasonId` int(11) NOT NULL DEFAULT '0' COMMENT '赛季ID',"
                + "`uid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家uid',"
                + "`rank` int(11) NOT NULL DEFAULT '0' COMMENT '玩家排名',"
                + "`serverId` int(11) NOT NULL DEFAULT '0' COMMENT '服务器ID',"
                + "`playername` varchar(500) NOT NULL DEFAULT '' COMMENT '玩家名字',"
                + "`icon` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家头像',"
                + "`iconBgk` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家头像框',"
                + "`grades` int(11) NOT NULL DEFAULT '0' COMMENT '玩家段位',"
                + "`starhoner` int(11) NOT NULL DEFAULT '0' COMMENT '段位星耀值',"
                + "`legendscore` bigint(20) NOT NULL DEFAULT '0' COMMENT '传说积分',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='赛季排行榜前1000名备份' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//seasonId
        _size+=8;//uid
        _size+=4;//rank
        _size+=4;//serverId
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playername);//playername
        _size+=8;//icon
        _size+=8;//iconBgk
        _size+=4;//grades
        _size+=4;//starhoner
        _size+=8;//legendscore
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(seasonId);
        buff.putLong(uid);
        buff.putInt(rank);
        buff.putInt(serverId);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, playername);
        buff.putLong(icon);
        buff.putLong(iconBgk);
        buff.putInt(grades);
        buff.putInt(starhoner);
        buff.putLong(legendscore);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        seasonId=buff.getInt();
        uid=buff.getLong();
        rank=buff.getInt();
        serverId=buff.getInt();
        playername=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        icon=buff.getLong();
        iconBgk=buff.getLong();
        grades=buff.getInt();
        starhoner=buff.getInt();
        legendscore=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
