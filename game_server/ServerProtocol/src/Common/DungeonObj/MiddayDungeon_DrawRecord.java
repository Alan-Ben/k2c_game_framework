package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本_宝箱领取记录
 **/
public class MiddayDungeon_DrawRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家ID */
private long cid;
/** 领取时间戳 */
private long drawTimeMs;


public MiddayDungeon_DrawRecord() {
	cid = (long)0;
	drawTimeMs = (long)0;
}

public MiddayDungeon_DrawRecord(
	 long _cid
	, long _drawTimeMs
) {	cid = _cid;
	drawTimeMs = _drawTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家ID */
public long getCid() { return cid; }
/** 玩家ID */
public void setCid(long _cid) { cid = _cid; }
/** 领取时间戳 */
public long getDrawTimeMs() { return drawTimeMs; }
/** 领取时间戳 */
public void setDrawTimeMs(long _drawTimeMs) { drawTimeMs = _drawTimeMs; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) drawTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(drawTimeMs);
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

