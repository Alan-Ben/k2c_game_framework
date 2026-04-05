package Common;

import java.nio.ByteBuffer;
public class Common_SeasonInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int lastSeasonDesc;
private int curSeason;
private int seasonStartTime;
private int seasonEndTime;


public Common_SeasonInfo() {
	lastSeasonDesc = 0;
	curSeason = 0;
	seasonStartTime = 0;
	seasonEndTime = 0;
}

public Common_SeasonInfo(
	 int _lastSeasonDesc
	, int _curSeason
	, int _seasonStartTime
	, int _seasonEndTime
) {	lastSeasonDesc = _lastSeasonDesc;
	curSeason = _curSeason;
	seasonStartTime = _seasonStartTime;
	seasonEndTime = _seasonEndTime;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getLastSeasonDesc() { return lastSeasonDesc; }
public void setLastSeasonDesc(int _lastSeasonDesc) { lastSeasonDesc = _lastSeasonDesc; }
public int getCurSeason() { return curSeason; }
public void setCurSeason(int _curSeason) { curSeason = _curSeason; }
public int getSeasonStartTime() { return seasonStartTime; }
public void setSeasonStartTime(int _seasonStartTime) { seasonStartTime = _seasonStartTime; }
public int getSeasonEndTime() { return seasonEndTime; }
public void setSeasonEndTime(int _seasonEndTime) { seasonEndTime = _seasonEndTime; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSeasonDesc = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curSeason = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) seasonStartTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) seasonEndTime = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lastSeasonDesc);
	_buf.putInt(curSeason);
	_buf.putInt(seasonStartTime);
	_buf.putInt(seasonEndTime);
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

