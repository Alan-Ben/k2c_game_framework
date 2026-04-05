package Common.GraveObj;

import java.nio.ByteBuffer;
/*********
 * 杰出者记录
 **/
public class GraveObj_Record implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 达成时间，时间戳：秒 */
private int achieveTs;


public GraveObj_Record() {
	cid = (long)0;
	achieveTs = 0;
}

public GraveObj_Record(
	 long _cid
	, int _achieveTs
) {	cid = _cid;
	achieveTs = _achieveTs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 达成时间，时间戳：秒 */
public int getAchieveTs() { return achieveTs; }
/** 达成时间，时间戳：秒 */
public void setAchieveTs(int _achieveTs) { achieveTs = _achieveTs; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) achieveTs = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(achieveTs);
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

