package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_MatchPlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private String playerName;
private int playerLvl;
private boolean isReady;
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
	, String _playerName
	, int _playerLvl
	, boolean _isReady
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getPlayerName() { return playerName; }
public void setPlayerName(String _playerName) { playerName = _playerName; }
public int getPlayerLvl() { return playerLvl; }
public void setPlayerLvl(int _playerLvl) { playerLvl = _playerLvl; }
public boolean getIsReady() { return isReady; }
public void setIsReady(boolean _isReady) { isReady = _isReady; }
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


public final int GetBufSize() {
	int _size = 59;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 61;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isReady = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerIcon = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.position();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.position(_lineupCurPos + _lineupCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) goupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) campId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _voiceSettingCustLen = _buf.getInt();
	int _voiceSettingCurPos = _buf.position();
	voiceSetting.ReadUnzipBuf(_buf, _voiceSettingCurPos + _voiceSettingCustLen);
	_buf.position(_voiceSettingCurPos + _voiceSettingCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerIconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
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

