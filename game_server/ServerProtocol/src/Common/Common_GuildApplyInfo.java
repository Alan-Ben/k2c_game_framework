package Common;

import java.nio.ByteBuffer;
public class Common_GuildApplyInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long sId;
private long uid;
private String playerName;
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
	, String _playerName
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getSId() { return sId; }
public void setSId(long _sId) { sId = _sId; }
public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getPlayerName() { return playerName; }
public void setPlayerName(String _playerName) { playerName = _playerName; }
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


public final int GetBufSize() {
	int _size = 56;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 58;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerIconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sId);
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putInt(playerLevel);
	_buf.putLong(playerIcon);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
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

