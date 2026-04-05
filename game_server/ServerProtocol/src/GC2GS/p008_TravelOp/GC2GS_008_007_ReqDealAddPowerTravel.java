package GC2GS.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 处理大臣加国力游历事件
 **/
public class GC2GS_008_007_ReqDealAddPowerTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 游历事件ID */
private long instanceId;
/** 大臣ID */
private long heroId;


public GC2GS_008_007_ReqDealAddPowerTravel() {
	instanceId = (long)0;
	heroId = (long)0;
}

public GC2GS_008_007_ReqDealAddPowerTravel(
	 long _instanceId
	, long _heroId
) {	instanceId = _instanceId;
	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)7; }

/** 游历事件ID */
public long getInstanceId() { return instanceId; }
/** 游历事件ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 大臣ID */
public long getHeroId() { return heroId; }
/** 大臣ID */
public void setHeroId(long _heroId) { heroId = _heroId; }


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
	if(_buf.remaining() > 0) heroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(heroId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
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

