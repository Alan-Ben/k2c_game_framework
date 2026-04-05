using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_GuildApplyInfo : ALBasicProtocolPack._IALProtocolStructure {
private long sId;
private long uid;
private string playerName;
private int playerLevel;
private long playerIcon;
private long grades;
private int starhoner;
private long legendscore;
private long playerIconBgk;


public Common_GuildApplyInfo() {
	sId = (long)0;
	uid = (long)0;
	playerName = "";
	playerLevel = 0;
	playerIcon = (long)0;
	grades = (long)0;
	starhoner = 0;
	legendscore = (long)0;
	playerIconBgk = (long)0;
}

public Common_GuildApplyInfo(
	long _sId
	, long _uid
	, string _playerName
	, int _playerLevel
	, long _playerIcon
	, long _grades
	, int _starhoner
	, long _legendscore
	, long _playerIconBgk
) {	sId = _sId;
	uid = _uid;
	playerName = _playerName;
	playerLevel = _playerLevel;
	playerIcon = _playerIcon;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	playerIconBgk = _playerIconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getSId() { return sId; }
public void setSId(long _sId) { sId = _sId; }
public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public string getPlayerName() { return playerName; }
public void setPlayerName(string _playerName) { playerName = _playerName; }
public int getPlayerLevel() { return playerLevel; }
public void setPlayerLevel(int _playerLevel) { playerLevel = _playerLevel; }
public long getPlayerIcon() { return playerIcon; }
public void setPlayerIcon(long _playerIcon) { playerIcon = _playerIcon; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public long getPlayerIconBgk() { return playerIconBgk; }
public void setPlayerIconBgk(long _playerIconBgk) { playerIconBgk = _playerIconBgk; }


public int GetBufSize() {
	int _size = 56;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 58;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(sId);
	_buf.putLong(uid);
	_buf.putString(playerName);
	_buf.putInt(playerLevel);
	_buf.putLong(playerIcon);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.putLong(playerIconBgk);
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
	builder.Append("sId").Append(":").Append(sId.ToString()).Append(", ");
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("playerLevel").Append(":").Append(playerLevel.ToString()).Append(", ");
	builder.Append("playerIcon").Append(":").Append(playerIcon.ToString()).Append(", ");
	builder.Append("grades").Append(":").Append(grades.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("playerIconBgk").Append(":").Append(playerIconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

