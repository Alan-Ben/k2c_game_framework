using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGC2GS_GuildBaseInfo : ALBasicProtocolPack._IALProtocolStructure {
private string guildName;
private string manifesto;
private long guildIcon;
private long national;
private long joinGrades;
private int joinLimit;


public WCGGC2GS_GuildBaseInfo() {
	guildName = "";
	manifesto = "";
	guildIcon = (long)0;
	national = (long)0;
	joinGrades = (long)0;
	joinLimit = 0;
}

public WCGGC2GS_GuildBaseInfo(
	string _guildName
	, string _manifesto
	, long _guildIcon
	, long _national
	, long _joinGrades
	, int _joinLimit
) {	guildName = _guildName;
	manifesto = _manifesto;
	guildIcon = _guildIcon;
	national = _national;
	joinGrades = _joinGrades;
	joinLimit = _joinLimit;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public string getGuildName() { return guildName; }
public void setGuildName(string _guildName) { guildName = _guildName; }
public string getManifesto() { return manifesto; }
public void setManifesto(string _manifesto) { manifesto = _manifesto; }
public long getGuildIcon() { return guildIcon; }
public void setGuildIcon(long _guildIcon) { guildIcon = _guildIcon; }
public long getNational() { return national; }
public void setNational(long _national) { national = _national; }
public long getJoinGrades() { return joinGrades; }
public void setJoinGrades(long _joinGrades) { joinGrades = _joinGrades; }
public int getJoinLimit() { return joinLimit; }
public void setJoinLimit(int _joinLimit) { joinLimit = _joinLimit; }


public int GetBufSize() {
	int _size = 28;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	manifesto = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	national = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinGrades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinLimit = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(guildName);
	_buf.putString(manifesto);
	_buf.putLong(guildIcon);
	_buf.putLong(national);
	_buf.putLong(joinGrades);
	_buf.putInt(joinLimit);
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
	builder.Append("guildName").Append(":").Append(guildName.ToString()).Append(", ");
	builder.Append("manifesto").Append(":").Append(manifesto.ToString()).Append(", ");
	builder.Append("guildIcon").Append(":").Append(guildIcon.ToString()).Append(", ");
	builder.Append("national").Append(":").Append(national.ToString()).Append(", ");
	builder.Append("joinGrades").Append(":").Append(joinGrades.ToString()).Append(", ");
	builder.Append("joinLimit").Append(":").Append(joinLimit.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

