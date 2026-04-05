package GC2GS.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 处理妃子酒馆游历事件
 **/
public class GC2GS_008_004_ReqDealConsortBarTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 游历事件ID */
private long instanceId;
/** 妃子ID */
private long consortId;
/** 消耗配置ID */
private long costId;


public GC2GS_008_004_ReqDealConsortBarTravel() {
	instanceId = (long)0;
	consortId = (long)0;
	costId = (long)0;
}

public GC2GS_008_004_ReqDealConsortBarTravel(
	 long _instanceId
	, long _consortId
	, long _costId
) {	instanceId = _instanceId;
	consortId = _consortId;
	costId = _costId;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)4; }

/** 游历事件ID */
public long getInstanceId() { return instanceId; }
/** 游历事件ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 妃子ID */
public long getConsortId() { return consortId; }
/** 妃子ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 消耗配置ID */
public long getCostId() { return costId; }
/** 消耗配置ID */
public void setCostId(long _costId) { costId = _costId; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(consortId);
	_buf.putLong(costId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)4);
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

