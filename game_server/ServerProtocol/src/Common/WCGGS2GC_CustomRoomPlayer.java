package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_CustomRoomPlayer implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private String name;
private long icon;
private boolean isHost;
private boolean isAI;
private int aiLevel;
private long aiRace;
private boolean isReady;
private boolean isRandomRace;
private Common.Common_Lineup lineup;
private boolean isOnline;
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
	, String _name
	, long _icon
	, boolean _isHost
	, boolean _isAI
	, int _aiLevel
	, long _aiRace
	, boolean _isReady
	, boolean _isRandomRace
	, Common.Common_Lineup _lineup
	, boolean _isOnline
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getName() { return name; }
public void setName(String _name) { name = _name; }
public long getIcon() { return icon; }
public void setIcon(long _icon) { icon = _icon; }
public boolean getIsHost() { return isHost; }
public void setIsHost(boolean _isHost) { isHost = _isHost; }
public boolean getIsAI() { return isAI; }
public void setIsAI(boolean _isAI) { isAI = _isAI; }
public int getAiLevel() { return aiLevel; }
public void setAiLevel(int _aiLevel) { aiLevel = _aiLevel; }
public long getAiRace() { return aiRace; }
public void setAiRace(long _aiRace) { aiRace = _aiRace; }
public boolean getIsReady() { return isReady; }
public void setIsReady(boolean _isReady) { isReady = _isReady; }
public boolean getIsRandomRace() { return isRandomRace; }
public void setIsRandomRace(boolean _isRandomRace) { isRandomRace = _isRandomRace; }
public Common.Common_Lineup getLineup() { return lineup; }
public void setLineup(Common.Common_Lineup _lineup) { lineup = _lineup; }
public boolean getIsOnline() { return isOnline; }
public void setIsOnline(boolean _isOnline) { isOnline = _isOnline; }
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


public final int GetBufSize() {
	int _size = 79;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 81;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isHost = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAI = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) aiLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) aiRace = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isRandomRace = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.position();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.position(_lineupCurPos + _lineupCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOnline = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) campId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _voiceSettingCustLen = _buf.getInt();
	int _voiceSettingCurPos = _buf.position();
	voiceSetting.ReadUnzipBuf(_buf, _voiceSettingCurPos + _voiceSettingCustLen);
	_buf.position(_voiceSettingCurPos + _voiceSettingCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
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

