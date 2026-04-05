package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 对自身指定请求移除推送
 **/
public class GS2GC_014_058_OnToMeApplyDel implements ALBasicProtocolPack._IALProtocolStructure {
/** 请求子嗣实例ID */
private long applyAdultId;


public GS2GC_014_058_OnToMeApplyDel() {
	applyAdultId = (long)0;
}

public GS2GC_014_058_OnToMeApplyDel(
	 long _applyAdultId
) {	applyAdultId = _applyAdultId;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)58; }

/** 请求子嗣实例ID */
public long getApplyAdultId() { return applyAdultId; }
/** 请求子嗣实例ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyAdultId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)58);
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

