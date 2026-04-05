package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟展示信息
 **/
public class Guild_ShowInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long guidId;
private long flagId;
private String name;
private String simpleName;
/** 宣言 */
private String declaration;
private long leaderId;
private int level;
private long exp;
/** 成员总赚速 */
private long totalEarnings;
/** 加入类型 */
private Common.GuildEnum.EGuildJoinType joinType;
/** 加入限制信息 */
private java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> joinLimitInfo;


public Guild_ShowInfo() {
	guidId = (long)0;
	flagId = (long)0;
	name = "";
	simpleName = "";
	declaration = "";
	leaderId = (long)0;
	level = 0;
	exp = (long)0;
	totalEarnings = (long)0;
	joinType = Common.GuildEnum.EGuildJoinType.values()[0];
	joinLimitInfo = new java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo>();
}

public Guild_ShowInfo(
	 long _guidId
	, long _flagId
	, String _name
	, String _simpleName
	, String _declaration
	, long _leaderId
	, int _level
	, long _exp
	, long _totalEarnings
	, Common.GuildEnum.EGuildJoinType _joinType
	, java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> _joinLimitInfo
) {	guidId = _guidId;
	flagId = _flagId;
	name = _name;
	simpleName = _simpleName;
	declaration = _declaration;
	leaderId = _leaderId;
	level = _level;
	exp = _exp;
	totalEarnings = _totalEarnings;
	joinType = _joinType;
	joinLimitInfo = _joinLimitInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGuidId() { return guidId; }
public void setGuidId(long _guidId) { guidId = _guidId; }
public long getFlagId() { return flagId; }
public void setFlagId(long _flagId) { flagId = _flagId; }
public String getName() { return name; }
public void setName(String _name) { name = _name; }
public String getSimpleName() { return simpleName; }
public void setSimpleName(String _simpleName) { simpleName = _simpleName; }
/** 宣言 */
public String getDeclaration() { return declaration; }
/** 宣言 */
public void setDeclaration(String _declaration) { declaration = _declaration; }
public long getLeaderId() { return leaderId; }
public void setLeaderId(long _leaderId) { leaderId = _leaderId; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public long getExp() { return exp; }
public void setExp(long _exp) { exp = _exp; }
/** 成员总赚速 */
public long getTotalEarnings() { return totalEarnings; }
/** 成员总赚速 */
public void setTotalEarnings(long _totalEarnings) { totalEarnings = _totalEarnings; }
/** 加入类型 */
public Common.GuildEnum.EGuildJoinType getJoinType() { return joinType; }
/** 加入类型 */
public void setJoinType(Common.GuildEnum.EGuildJoinType _joinType) { joinType = _joinType; }
/** 加入限制信息 */
public java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> getJoinLimitInfo() { return joinLimitInfo; }
/** 加入限制信息 */
public void addJoinLimitInfo(Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo) { joinLimitInfo.add(_joinLimitInfo); }


public final int GetBufSize() {
	int _size = 48;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);
	_size += 2 + (joinLimitInfo.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);
	_size += 2 + (joinLimitInfo.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guidId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) flagId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) simpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) declaration = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leaderId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalEarnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinType = Common.GuildEnum.EGuildJoinType.EGuildJoinType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinLimitInfoCount = _buf.getShort();
	for(int _i = 0; _i < _joinLimitInfoCount; _i++) { 
		Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo = new Common.GuildObj.Guild_JoinLimitInfo();
		if(_buf.remaining() <= 0) return;
	int __joinLimitInfoCustLen = _buf.getInt();
	int __joinLimitInfoCurPos = _buf.position();
	_joinLimitInfo.ReadUnzipBuf(_buf, __joinLimitInfoCurPos + __joinLimitInfoCustLen);
	_buf.position(__joinLimitInfoCurPos + __joinLimitInfoCustLen);

		joinLimitInfo.add(_joinLimitInfo);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guidId);
	_buf.putLong(flagId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, simpleName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, declaration);
	_buf.putLong(leaderId);
	_buf.putInt(level);
	_buf.putLong(exp);
	_buf.putLong(totalEarnings);
	_buf.putInt(joinType.ordinal());

	_buf.putShort((short)joinLimitInfo.size());
	for(int _i = 0; _i < joinLimitInfo.size(); _i++) { 
		_buf.putInt(joinLimitInfo.get(_i).GetBufSize());
	joinLimitInfo.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

