package WCGCS2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_001_003_NotifyPlayerSeasonEnd implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int curSeason;
private int lastSeason;
private int ranking;


public WCGCS2US_001_003_NotifyPlayerSeasonEnd() {
	uid = (long)0;
	curSeason = 0;
	lastSeason = 0;
	ranking = 0;
}

public WCGCS2US_001_003_NotifyPlayerSeasonEnd(
	 long _uid
	, int _curSeason
	, int _lastSeason
	, int _ranking
) {	uid = _uid;
	curSeason = _curSeason;
	lastSeason = _lastSeason;
	ranking = _ranking;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getCurSeason() { return curSeason; }
public void setCurSeason(int _curSeason) { curSeason = _curSeason; }
public int getLastSeason() { return lastSeason; }
public void setLastSeason(int _lastSeason) { lastSeason = _lastSeason; }
public int getRanking() { return ranking; }
public void setRanking(int _ranking) { ranking = _ranking; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curSeason = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSeason = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ranking = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(curSeason);
	_buf.putInt(lastSeason);
	_buf.putInt(ranking);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

