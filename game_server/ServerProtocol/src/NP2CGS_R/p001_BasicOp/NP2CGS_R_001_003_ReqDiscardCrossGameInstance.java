package NP2CGS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2CGS_R_001_003_ReqDiscardCrossGameInstance implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服游戏类型 */
private NPEnum.ENPCrossGameCategoryEnum category;
private long instanceId;


public NP2CGS_R_001_003_ReqDiscardCrossGameInstance() {
	category = NPEnum.ENPCrossGameCategoryEnum.values()[0];
	instanceId = (long)0;
}

public NP2CGS_R_001_003_ReqDiscardCrossGameInstance(
	 NPEnum.ENPCrossGameCategoryEnum _category
	, long _instanceId
) {	category = _category;
	instanceId = _instanceId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

/** 跨服游戏类型 */
public NPEnum.ENPCrossGameCategoryEnum getCategory() { return category; }
/** 跨服游戏类型 */
public void setCategory(NPEnum.ENPCrossGameCategoryEnum _category) { category = _category; }
public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }


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
	if(_buf.remaining() > 0) category = NPEnum.ENPCrossGameCategoryEnum.ENPCrossGameCategoryEnum_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(category.ordinal());

	_buf.putLong(instanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

