package Common;

import java.nio.ByteBuffer;
public class Common_GuildMemberInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private String playerName;
private long playerIcon;
private int guildPost;
private long grades;
private int starhoner;
private long legendscore;
private boolean isOnline;
private int state;
private int playerLevel;
private long playerIconBgk;


public Common_GuildMemberInfo() {
	uid = (long)0;
	playerName = "";
	playerIcon = (long)0;
	guildPost = 0;
	grades = (long)0;
	starhoner = 0;
	legendscore = (long)0;
	isOnline = false;
	state = 0;
	playerLevel = 0;
	playerIconBgk = (long)0;
}

public Common_GuildMemberInfo(
	 long _uid
	, String _playerName
	, long _playerIcon
	, int _guildPost
	, long _grades
	, int _starhoner
	, long _legendscore
	, boolean _isOnline
	, int _state
	, int _playerLevel
	, long _playerIconBgk
) {	uid = _uid;
	playerName = _playerName;
	playerIcon = _playerIcon;
	guildPost = _guildPost;
	grades = _grades;
	starhoner = _starhoner;
	legendscore = _legendscore;
	isOnline = _isOnline;
	state = _state;
	playerLevel = _playerLevel;
	playerIconBgk = _playerIconBgk;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getPlayerName() { return playerName; }
public void setPlayerName(String _playerName) { playerName = _playerName; }
public long getPlayerIcon() { return playerIcon; }
public void setPlayerIcon(long _playerIcon) { playerIcon = _playerIcon; }
public int getGuildPost() { return guildPost; }
public void setGuildPost(int _guildPost) { guildPost = _guildPost; }
public long getGrades() { return grades; }
public void setGrades(long _grades) { grades = _grades; }
public int getStarhoner() { return starhoner; }
public void setStarhoner(int _starhoner) { starhoner = _starhoner; }
public long getLegendscore() { return legendscore; }
public void setLegendscore(long _legendscore) { legendscore = _legendscore; }
public boolean getIsOnline() { return isOnline; }
public void setIsOnline(boolean _isOnline) { isOnline = _isOnline; }
public int getState() { return state; }
public void setState(int _state) { state = _state; }
public int getPlayerLevel() { return playerLevel; }
public void setPlayerLevel(int _playerLevel) { playerLevel = _playerLevel; }
public long getPlayerIconBgk() { return playerIconBgk; }
public void setPlayerIconBgk(long _playerIconBgk) { playerIconBgk = _playerIconBgk; }


public final int GetBufSize() {
	int _size = 57;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 59;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildPost = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOnline = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) state = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerIconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putLong(playerIcon);
	_buf.putInt(guildPost);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
	_buf.put(isOnline?(byte)1:(byte)0);
	_buf.putInt(state);
	_buf.putInt(playerLevel);
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

