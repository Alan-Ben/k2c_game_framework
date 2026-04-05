package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_RankInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int rank;
private String playername;
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
	, String _playername
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getRank() { return rank; }
public void setRank(int _rank) { rank = _rank; }
public String getPlayername() { return playername; }
public void setPlayername(String _playername) { playername = _playername; }
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


public final int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playername);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playername);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playername = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) icon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) grades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starhoner = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) legendscore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconBgk = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(rank);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playername);
	_buf.putLong(icon);
	_buf.putInt(serverId);
	_buf.putLong(grades);
	_buf.putInt(starhoner);
	_buf.putLong(legendscore);
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

