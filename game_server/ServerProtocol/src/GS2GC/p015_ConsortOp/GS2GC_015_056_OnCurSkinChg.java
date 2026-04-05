package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人当前皮肤变更
 **/
public class GS2GC_015_056_OnCurSkinChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long consortId;
/** 当前皮肤ID */
private long curSkinId;


public GS2GC_015_056_OnCurSkinChg() {
	consortId = (long)0;
	curSkinId = (long)0;
}

public GS2GC_015_056_OnCurSkinChg(
	 long _consortId
	, long _curSkinId
) {	consortId = _consortId;
	curSkinId = _curSkinId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)56; }

/** 空 */
public long getConsortId() { return consortId; }
/** 空 */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 当前皮肤ID */
public long getCurSkinId() { return curSkinId; }
/** 当前皮肤ID */
public void setCurSkinId(long _curSkinId) { curSkinId = _curSkinId; }


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
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curSkinId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(curSkinId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)56);
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

