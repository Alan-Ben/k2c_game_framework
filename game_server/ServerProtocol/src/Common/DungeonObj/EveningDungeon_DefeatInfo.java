package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 晚间副本击杀信息
 **/
public class EveningDungeon_DefeatInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家ID */
private long cid;
/** 击杀时间戳 */
private long defeatTimeMs;


public EveningDungeon_DefeatInfo() {
	cid = (long)0;
	defeatTimeMs = (long)0;
}

public EveningDungeon_DefeatInfo(
	 long _cid
	, long _defeatTimeMs
) {	cid = _cid;
	defeatTimeMs = _defeatTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家ID */
public long getCid() { return cid; }
/** 玩家ID */
public void setCid(long _cid) { cid = _cid; }
/** 击杀时间戳 */
public long getDefeatTimeMs() { return defeatTimeMs; }
/** 击杀时间戳 */
public void setDefeatTimeMs(long _defeatTimeMs) { defeatTimeMs = _defeatTimeMs; }


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
	if(_buf.remaining() > 0) defeatTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(defeatTimeMs);
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

