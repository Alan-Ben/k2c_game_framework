package GC2GS.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 加入宴会
 **/
public class GC2GS_019_008_ReqJoinDinner implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会实例ID */
private long instanceId;
/** 消耗配置ID */
private long costId;


public GC2GS_019_008_ReqJoinDinner() {
	instanceId = (long)0;
	costId = (long)0;
}

public GC2GS_019_008_ReqJoinDinner(
	 long _instanceId
	, long _costId
) {	instanceId = _instanceId;
	costId = _costId;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)8; }

/** 宴会实例ID */
public long getInstanceId() { return instanceId; }
/** 宴会实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 消耗配置ID */
public long getCostId() { return costId; }
/** 消耗配置ID */
public void setCostId(long _costId) { costId = _costId; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(costId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)8);
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

