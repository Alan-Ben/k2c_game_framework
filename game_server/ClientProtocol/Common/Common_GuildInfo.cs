using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_GuildInfo : ALBasicProtocolPack._IALProtocolStructure {
private long guildId;
private string guildName;
private string manifesto;
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
	, string _guildName
	, string _manifesto
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public string getGuildName() { return guildName; }
public void setGuildName(string _guildName) { guildName = _guildName; }
public string getManifesto() { return manifesto; }
public void setManifesto(string _manifesto) { manifesto = _manifesto; }
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


public int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	manifesto = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exp = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	national = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinGrades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	memberCnt = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(guildId);
	_buf.putString(guildName);
	_buf.putString(manifesto);
	_buf.putInt(level);
	_buf.putInt(exp);
	_buf.putInt(createTime);
	_buf.putLong(guildIcon);
	_buf.putLong(national);
	_buf.putLong(joinGrades);
	_buf.putInt(joinLimit);
	_buf.putInt(memberCnt);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("guildId").Append(":").Append(guildId.ToString()).Append(", ");
	builder.Append("guildName").Append(":").Append(guildName.ToString()).Append(", ");
	builder.Append("manifesto").Append(":").Append(manifesto.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("exp").Append(":").Append(exp.ToString()).Append(", ");
	builder.Append("createTime").Append(":").Append(createTime.ToString()).Append(", ");
	builder.Append("guildIcon").Append(":").Append(guildIcon.ToString()).Append(", ");
	builder.Append("national").Append(":").Append(national.ToString()).Append(", ");
	builder.Append("joinGrades").Append(":").Append(joinGrades.ToString()).Append(", ");
	builder.Append("joinLimit").Append(":").Append(joinLimit.ToString()).Append(", ");
	builder.Append("memberCnt").Append(":").Append(memberCnt.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

