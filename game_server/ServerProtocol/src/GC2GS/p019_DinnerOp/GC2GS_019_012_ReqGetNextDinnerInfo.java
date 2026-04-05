package GC2GS.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 取指定宴会的下一条宴会数据
 **/
public class GC2GS_019_012_ReqGetNextDinnerInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会实例ID */
private long instanceId;
/** 当前排序 */
private int idx;


public GC2GS_019_012_ReqGetNextDinnerInfo() {
	instanceId = (long)0;
	idx = 0;
}

public GC2GS_019_012_ReqGetNextDinnerInfo(
	 long _instanceId
	, int _idx
) {	instanceId = _instanceId;
	idx = _idx;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)12; }

/** 宴会实例ID */
public long getInstanceId() { return instanceId; }
/** 宴会实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 当前排序 */
public int getIdx() { return idx; }
/** 当前排序 */
public void setIdx(int _idx) { idx = _idx; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) idx = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(idx);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)12);
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

