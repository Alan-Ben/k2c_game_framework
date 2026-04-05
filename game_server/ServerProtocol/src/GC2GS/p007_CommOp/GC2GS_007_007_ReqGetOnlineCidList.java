package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 请求在线玩家CID列表
 **/
public class GC2GS_007_007_ReqGetOnlineCidList implements ALBasicProtocolPack._IALProtocolStructure {
/** 客户端需要数量，服务端做保护，不超过100个 */
private int num;


public GC2GS_007_007_ReqGetOnlineCidList() {
	num = 0;
}

public GC2GS_007_007_ReqGetOnlineCidList(
	 int _num
) {	num = _num;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)7; }

/** 客户端需要数量，服务端做保护，不超过100个 */
public int getNum() { return num; }
/** 客户端需要数量，服务端做保护，不超过100个 */
public void setNum(int _num) { num = _num; }


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
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)7);
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

