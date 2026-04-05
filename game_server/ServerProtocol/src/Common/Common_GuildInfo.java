package Common;

import java.nio.ByteBuffer;
public class Common_GuildInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long guildId;
private String guildName;
private String manifesto;
private int level;
private int exp;
private int createTime;
private long guildIcon;
private long national;
private long joinGrades;
private int joinLimit;
private int memberCnt;


public Common_GuildInfo() {
	guildId = (long)0;
	guildName = "";
	manifesto = "";
	level = 0;
	exp = 0;
	createTime = 0;
	guildIcon = (long)0;
	national = (long)0;
	joinGrades = (long)0;
	joinLimit = 0;
	memberCnt = 0;
}

public Common_GuildInfo(
	 long _guildId
	, String _guildName
	, String _manifesto
	, int _level
	, int _exp
	, int _createTime
	, long _guildIcon
	, long _national
	, long _joinGrades
	, int _joinLimit
	, int _memberCnt
) {	guildId = _guildId;
	guildName = _guildName;
	manifesto = _manifesto;
	level = _level;
	exp = _exp;
	createTime = _createTime;
	guildIcon = _guildIcon;
	national = _national;
	joinGrades = _joinGrades;
	joinLimit = _joinLimit;
	memberCnt = _memberCnt;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public String getGuildName() { return guildName; }
public void setGuildName(String _guildName) { guildName = _guildName; }
public String getManifesto() { return manifesto; }
public void setManifesto(String _manifesto) { manifesto = _manifesto; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getExp() { return exp; }
public void setExp(int _exp) { exp = _exp; }
public int getCreateTime() { return createTime; }
public void setCreateTime(int _createTime) { createTime = _createTime; }
public long getGuildIcon() { return guildIcon; }
public void setGuildIcon(long _guildIcon) { guildIcon = _guildIcon; }
public long getNational() { return national; }
public void setNational(long _national) { national = _national; }
public long getJoinGrades() { return joinGrades; }
public void setJoinGrades(long _joinGrades) { joinGrades = _joinGrades; }
public int getJoinLimit() { return joinLimit; }
public void setJoinLimit(int _joinLimit) { joinLimit = _joinLimit; }
public int getMemberCnt() { return memberCnt; }
public void setMemberCnt(int _memberCnt) { memberCnt = _memberCnt; }


public final int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) manifesto = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exp = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) national = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinGrades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) memberCnt = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guildId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, manifesto);
	_buf.putInt(level);
	_buf.putInt(exp);
	_buf.putInt(createTime);
	_buf.putLong(guildIcon);
	_buf.putLong(national);
	_buf.putLong(joinGrades);
	_buf.putInt(joinLimit);
	_buf.putInt(memberCnt);
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

