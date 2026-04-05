package GS2GC.p003_FristTeamActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_003_002_RetGameLogicRedirectTest implements ALBasicProtocolPack._IALProtocolStructure {
/** 测试参数1 */
private long param1;


public GS2GC_003_002_RetGameLogicRedirectTest() {
	param1 = (long)0;
}

public GS2GC_003_002_RetGameLogicRedirectTest(
	 long _param1
) {	param1 = _param1;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)2; }

/** 测试参数1 */
public long getParam1() { return param1; }
/** 测试参数1 */
public void setParam1(long _param1) { param1 = _param1; }


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
	if(_buf.remaining() > 0) param1 = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(param1);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)2);
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

