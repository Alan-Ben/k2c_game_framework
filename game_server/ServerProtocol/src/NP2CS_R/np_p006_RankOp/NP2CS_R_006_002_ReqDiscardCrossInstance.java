package NP2CS_R.np_p006_RankOp;

import java.nio.ByteBuffer;
/*********
 * 请求销毁跨服排行榜分组实例id
 **/
public class NP2CS_R_006_002_ReqDiscardCrossInstance implements ALBasicProtocolPack._IALProtocolStructure {
private long crossInstanceId;


public NP2CS_R_006_002_ReqDiscardCrossInstance() {
	crossInstanceId = (long)0;
}

public NP2CS_R_006_002_ReqDiscardCrossInstance(
	 long _crossInstanceId
) {	crossInstanceId = _crossInstanceId;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)2; }

public long getCrossInstanceId() { return crossInstanceId; }
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }


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
	if(_buf.remaining() > 0) crossInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(crossInstanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
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

