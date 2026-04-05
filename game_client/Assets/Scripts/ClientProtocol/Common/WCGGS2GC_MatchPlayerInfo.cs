using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_MatchPlayerInfo : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private string playerName;
private int playerLvl;
private bool isReady;
private int playerIcon;
private Common.Common_Lineup lineup;
private long grades;
private int starhoner;
private long legendscore;
private int goupId;
private int campId;
private Common.Common_VoiceSetting voiceSetting;
private long playerIconBgk;


public WCGGS2GC_MatchPlayerInfo() {
	uid = (long)0;
	playerName = "";
	playerLvl = 0;
	isReady = false;
	playerIcon = 0;
	lineup = new Common.Common_Lineup();
	grades = (long)0;
	starhoner = 0;
	legendscore = (long)0;
	goupId = 0;
	campId = 0;
	voiceSetting = new Common.Common_VoiceSetting();
	playerIconBgk = (long)0;
}

public WCGGS2GC_MatchPlayerInfo(
	long _uid
	, string _playerName
	, int _playerLvl
	, bool _isReady
	, int _playerIcon
	, Common.Common_Lineup _lineup
	, long _grades
	, int _starhoner
	, long _legendscore
	, int _goupId
	, int _campId
	, Common.Common_VoiceSetting _voiceSetting
	, long _playerIconBgk
) {	uid = _uid;
	playerName = _playerName;
	playerLvl = _playerLvl;
	isReady = _isReady;
	playerIcon = _playerIcon;
	lineup = _lineup;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	goupId = _goupId;
	campId = _campId;
	voiceSetting = _voiceSetting;
	playerIconBgk = _playerIconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public string getPlayerName() { return playerName; }
public void setPlayerName(string _playerName) { playerName = _playerName; }
public int getPlayerLvl() { return playerLvl; }
public void setPlayerLvl(int _playerLvl) { playerLvl = _playerLvl; }
public bool getIsReady() { return isReady; }
public void setIsReady(bool _isReady) { isReady = _isReady; }
public int getPlayerIcon() { return playerIcon; }
public void setPlayerIcon(int _playerIcon) { playerIcon = _playerIcon; }
public Common.Common_Lineup getLineup() { return lineup; }
public void setLineup(Common.Common_Lineup _lineup) { lineup = _lineup; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public int getGoupId() { return goupId; }
public void setGoupId(int _goupId) { goupId = _goupId; }
public int getCampId() { return campId; }
public void setCampId(int _campId) { campId = _campId; }
public Common.Common_VoiceSetting getVoiceSetting() { return voiceSetting; }
public void setVoiceSetting(Common.Common_VoiceSetting _voiceSetting) { voiceSetting = _voiceSetting; }
public long getPlayerIconBgk() { return playerIconBgk; }
public void setPlayerIconBgk(long _playerIconBgk) { playerIconBgk = _playerIconBgk; }


public int GetBufSize() {
	int _size = 59;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 61;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIcon = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.getCurPos();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.setPosition(_lineupCurPos + _lineupCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	goupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	campId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _voiceSettingCustLen = _buf.getInt();
	int _voiceSettingCurPos = _buf.getCurPos();
	voiceSetting.ReadUnzipBuf(_buf, _voiceSettingCurPos + _voiceSettingCustLen);
	_buf.setPosition(_voiceSettingCurPos + _voiceSettingCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerIconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putString(playerName);
	_buf.putInt(playerLvl);
	_buf.put(isReady?(byte)1:(byte)0);
	_buf.putInt(playerIcon);
	_buf.putInt(lineup.GetBufSize());
	lineup.PutUnzipBuf(_buf);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.putInt(goupId);
	_buf.putInt(campId);
	_buf.putInt(voiceSetting.GetBufSize());
	voiceSetting.PutUnzipBuf(_buf);
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
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("playerLvl").Append(":").Append(playerLvl.ToString()).Append(", ");
	builder.Append("isReady").Append(":").Append(isReady.ToString()).Append(", ");
	builder.Append("playerIcon").Append(":").Append(playerIcon.ToString()).Append(", ");
	builder.Append("lineup").Append(":").Append(lineup == null ? "null" : lineup.ToString()).Append(", ");
	builder.Append("grades").Append(":").Append(grades.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("goupId").Append(":").Append(goupId.ToString()).Append(", ");
	builder.Append("campId").Append(":").Append(campId.ToString()).Append(", ");
	builder.Append("voiceSetting").Append(":").Append(voiceSetting == null ? "null" : voiceSetting.ToString()).Append(", ");
	builder.Append("playerIconBgk").Append(":").Append(playerIconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

