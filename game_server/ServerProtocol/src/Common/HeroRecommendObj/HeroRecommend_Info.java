package Common.HeroRecommendObj;

import java.nio.ByteBuffer;
/*********
 * 大臣推荐事件数据
 **/
public class HeroRecommend_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long instanceId;
/** 配置ID */
private long refId;
/** 随机种子 */
private long rndSeed;


public HeroRecommend_Info() {
	instanceId = (long)0;
	refId = (long)0;
	rndSeed = (long)0;
}

public HeroRecommend_Info(
	 long _instanceId
	, long _refId
	, long _rndSeed
) {	instanceId = _instanceId;
	refId = _refId;
	rndSeed = _rndSeed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getInstanceId() { return instanceId; }
/** 实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 配置ID */
public long getRefId() { return refId; }
/** 配置ID */
public void setRefId(long _refId) { refId = _refId; }
/** 随机种子 */
public long getRndSeed() { return rndSeed; }
/** 随机种子 */
public void setRndSeed(long _rndSeed) { rndSeed = _rndSeed; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rndSeed = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(refId);
	_buf.putLong(rndSeed);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

