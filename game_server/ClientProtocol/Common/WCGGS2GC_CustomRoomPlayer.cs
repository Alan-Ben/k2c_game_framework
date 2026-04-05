using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_CustomRoomPlayer : ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private string name;
private long icon;
private bool isHost;
private bool isAI;
private int aiLevel;
private long aiRace;
private bool isReady;
private bool isRandomRace;
private Common.Common_Lineup lineup;
private bool isOnline;
private long grades;
private int starhoner;
private long legendscore;
private int level;
private int groupId;
private int campId;
private Common.Common_VoiceSetting voiceSetting;
private long iconBgk;


public WCGGS2GC_CustomRoomPlayer() {
	uid = (long)0;
	name = "";
	icon = (long)0;
	isHost = false;
	isAI = false;
	aiLevel = 0;
	aiRace = (long)0;
	isReady = false;
	isRandomRace = false;
	lineup = new Common.Common_Lineup();
	isOnline = false;
	grades = (long)0;
	starhoner = 0;
	legendscore = (long)0;
	level = 0;
	groupId = 0;
	campId = 0;
	voiceSetting = new Common.Common_VoiceSetting();
	iconBgk = (long)0;
}

public WCGGS2GC_CustomRoomPlayer(
	long _uid
	, string _name
	, long _icon
	, bool _isHost
	, bool _isAI
	, int _aiLevel
	, long _aiRace
	, bool _isReady
	, bool _isRandomRace
	, Common.Common_Lineup _lineup
	, bool _isOnline
	, long _grades
	, int _starhoner
	, long _legendscore
	, int _level
	, int _groupId
	, int _campId
	, Common.Common_VoiceSetting _voiceSetting
	, long _iconBgk
) {	uid = _uid;
	name = _name;
	icon = _icon;
	isHost = _isHost;
	isAI = _isAI;
	aiLevel = _aiLevel;
	aiRace = _aiRace;
	isReady = _isReady;
	isRandomRace = _isRandomRace;
	lineup = _lineup;
	isOnline = _isOnline;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	level = _level;
	groupId = _groupId;
	campId = _campId;
	voiceSetting = _voiceSetting;
	iconBgk = _iconBgk;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public string getName() { return name; }
public void setName(string _name) { name = _name; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public bool getIsHost() { return isHost; }
public void setIsHost(bool _isHost) { isHost = _isHost; }
public bool getIsAI() { return isAI; }
public void setIsAI(bool _isAI) { isAI = _isAI; }
public int getAiLevel() { return aiLevel; }
public void setAiLevel(int _aiLevel) { aiLevel = _aiLevel; }
public long getAiRace() { return aiRace; }
public void setAiRace(long _aiRace) { aiRace = _aiRace; }
public bool getIsReady() { return isReady; }
public void setIsReady(bool _isReady) { isReady = _isReady; }
public bool getIsRandomRace() { return isRandomRace; }
public void setIsRandomRace(bool _isRandomRace) { isRandomRace = _isRandomRace; }
public Common.Common_Lineup getLineup() { return lineup; }
public void setLineup(Common.Common_Lineup _lineup) { lineup = _lineup; }
public bool getIsOnline() { return isOnline; }
public void setIsOnline(bool _isOnline) { isOnline = _isOnline; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getGroupId() { return groupId; }
public void setGroupId(int _groupId) { groupId = _groupId; }
public int getCampId() { return campId; }
public void setCampId(int _campId) { campId = _campId; }
public Common.Common_VoiceSetting getVoiceSetting() { return voiceSetting; }
public void setVoiceSetting(Common.Common_VoiceSetting _voiceSetting) { voiceSetting = _voiceSetting; }
public long getIconBgk() { return iconBgk; }
public void setIconBgk(long _iconBgk) { iconBgk = _iconBgk; }


public int GetBufSize() {
	int _size = 79;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 81;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isHost = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAI = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	aiLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	aiRace = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isRandomRace = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.getCurPos();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.setPosition(_lineupCurPos + _lineupCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isOnline = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	campId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _voiceSettingCustLen = _buf.getInt();
	int _voiceSettingCurPos = _buf.getCurPos();
	voiceSetting.ReadUnzipBuf(_buf, _voiceSettingCurPos + _voiceSettingCustLen);
	_buf.setPosition(_voiceSettingCurPos + _voiceSettingCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgk = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uid);
	_buf.putString(name);
	_buf.putLong(icon);
	_buf.put(isHost?(byte)1:(byte)0);
	_buf.put(isAI?(byte)1:(byte)0);
	_buf.putInt(aiLevel);
	_buf.putLong(aiRace);
	_buf.put(isReady?(byte)1:(byte)0);
	_buf.put(isRandomRace?(byte)1:(byte)0);
	_buf.putInt(lineup.GetBufSize());
	lineup.PutUnzipBuf(_buf);
	_buf.put(isOnline?(byte)1:(byte)0);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.putInt(level);
	_buf.putInt(groupId);
	_buf.putInt(campId);
	_buf.putInt(voiceSetting.GetBufSize());
	voiceSetting.PutUnzipBuf(_buf);
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
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("icon").Append(":").Append(icon.ToString()).Append(", ");
	builder.Append("isHost").Append(":").Append(isHost.ToString()).Append(", ");
	builder.Append("isAI").Append(":").Append(isAI.ToString()).Append(", ");
	builder.Append("aiLevel").Append(":").Append(aiLevel.ToString()).Append(", ");
	builder.Append("aiRace").Append(":").Append(aiRace.ToString()).Append(", ");
	builder.Append("isReady").Append(":").Append(isReady.ToString()).Append(", ");
	builder.Append("isRandomRace").Append(":").Append(isRandomRace.ToString()).Append(", ");
	builder.Append("lineup").Append(":").Append(lineup == null ? "null" : lineup.ToString()).Append(", ");
	builder.Append("isOnline").Append(":").Append(isOnline.ToString()).Append(", ");
	builder.Append("grades").Append(":").Append(grades.ToString()).Append(", ");
	builder.Append("starhoner").Append(":").Append(starhoner.ToString()).Append(", ");
	builder.Append("legendscore").Append(":").Append(legendscore.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("campId").Append(":").Append(campId.ToString()).Append(", ");
	builder.Append("voiceSetting").Append(":").Append(voiceSetting == null ? "null" : voiceSetting.ToString()).Append(", ");
	builder.Append("iconBgk").Append(":").Append(iconBgk.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

