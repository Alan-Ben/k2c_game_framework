package GC2GS.p038_MarsOp;

import java.nio.ByteBuffer;
/*********
 * 前往火星-请求阶段留言列表
 **/
public class GC2GS_038_005_ReqGetStageMsgList implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段 */
private int stage;


public GC2GS_038_005_ReqGetStageMsgList() {
	stage = 0;
}

public GC2GS_038_005_ReqGetStageMsgList(
	 int _stage
) {	stage = _stage;
}

public final byte getMainOrder() { return (byte)38; }

public final byte getSubOrder() { return (byte)5; }

/** 阶段 */
public int getStage() { return stage; }
/** 阶段 */
public void setStage(int _stage) { stage = _stage; }


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
	if(_buf.remaining() > 0) stage = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stage);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)38);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)38);
	_recBuf.put((byte)5);
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

