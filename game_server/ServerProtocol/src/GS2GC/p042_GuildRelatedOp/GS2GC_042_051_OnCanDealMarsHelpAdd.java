package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 可以帮助的火星求助实例数量新增
 **/
public class GS2GC_042_051_OnCanDealMarsHelpAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 新增可帮助的火星求助实例ID */
private long canDealId;


public GS2GC_042_051_OnCanDealMarsHelpAdd() {
	canDealId = (long)0;
}

public GS2GC_042_051_OnCanDealMarsHelpAdd(
	 long _canDealId
) {	canDealId = _canDealId;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)51; }

/** 新增可帮助的火星求助实例ID */
public long getCanDealId() { return canDealId; }
/** 新增可帮助的火星求助实例ID */
public void setCanDealId(long _canDealId) { canDealId = _canDealId; }


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
	if(_buf.remaining() > 0) canDealId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(canDealId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)51);
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

