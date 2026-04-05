using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_RankInfo : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int rank;
private string playername;
private long icon;
private int serverId;
private long grades;
private int starhoner;
private long legendscore;
private long iconBgk;


public WCGGS2GC_RankInfo() {
	uid = (long)0;
	rank = 0;
	playername = "";
	icon = (long)0;
	serverId = 0;
	grades = (long)0;
	starhoner = 0;
	legendscore = (long)0;
	iconBgk = (long)0;
}

public WCGGS2GC_RankInfo(
	long _uid
	, int _rank
	, string _playername
	, long _icon
	, int _serverId
	, long _grades
	, int _starhoner
	, long _legendscore
	, long _iconBgk
) {	uid = _uid;
	rank = _rank;
	playername = _playername;
	icon = _icon;
	serverId = _serverId;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	iconBgk = _iconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getRank() { return rank; }
public void setRank(int _rank) { rank = _rank; }
public string getPlayername() { return playername; }
public void setPlayername(string _playername) { playername = _playername; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public int getServerId() { return serverId; }
public void setServerId(int _serverId) { serverId = _serverId; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playername);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playername);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playername = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putInt(rank);
	_buf.putString(playername);
	_buf.putLong(icon);
	_buf.putInt(serverId);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.putLong(iconBgk);
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
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("rank").Append(":").Append(rank.ToString()).Append(", ");
	builder.Append("playername").Append(":").Append(playername.ToString()).Append(", ");
	builder.Append("icon").Append(":").Append(icon.ToString()).Append(", ");
	builder.Append("serverId").Append(":").Append(serverId.ToString()).Append(", ");
	builder.Append("grades").Append(":").Append(grades.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("iconBgk").Append(":").Append(iconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

