package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人对话是否添加好友标志位变更
 **/
public class GS2GC_015_074_OnConsortChatHasAddChg implements ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/** 是否添加好友 */
private boolean hasAdd;


public GS2GC_015_074_OnConsortChatHasAddChg() {
	consortId = (long)0;
	hasAdd = false;
}

public GS2GC_015_074_OnConsortChatHasAddChg(
	 long _consortId
	, boolean _hasAdd
) {	consortId = _consortId;
	hasAdd = _hasAdd;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)74; }

public long getConsortId() { return consortId; }
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 是否添加好友 */
public boolean getHasAdd() { return hasAdd; }
/** 是否添加好友 */
public void setHasAdd(boolean _hasAdd) { hasAdd = _hasAdd; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasAdd = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.put(hasAdd?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)74);
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

