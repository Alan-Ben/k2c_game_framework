package GS2GC.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
public class GS2GC_039_009_RetHomeCollectTimeChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 最后一次收集产出时间点 */
private int lastHomeOutputCollectTimeS;


public GS2GC_039_009_RetHomeCollectTimeChg() {
	lastHomeOutputCollectTimeS = 0;
}

public GS2GC_039_009_RetHomeCollectTimeChg(
	 int _lastHomeOutputCollectTimeS
) {	lastHomeOutputCollectTimeS = _lastHomeOutputCollectTimeS;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)9; }

/** 最后一次收集产出时间点 */
public int getLastHomeOutputCollectTimeS() { return lastHomeOutputCollectTimeS; }
/** 最后一次收集产出时间点 */
public void setLastHomeOutputCollectTimeS(int _lastHomeOutputCollectTimeS) { lastHomeOutputCollectTimeS = _lastHomeOutputCollectTimeS; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastHomeOutputCollectTimeS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lastHomeOutputCollectTimeS);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)9);
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

