package NP2CGS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2CGS_R_001_002_ReqCreateCrossGameInstance implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服游戏类型 */
private NPEnum.ENPCrossGameCategoryEnum category;
/** 实例ID */
private long instanceId;
/** 消息 */
private byte[] extData;


public NP2CGS_R_001_002_ReqCreateCrossGameInstance() {
	category = NPEnum.ENPCrossGameCategoryEnum.values()[0];
	instanceId = (long)0;
	extData = null;
}

public NP2CGS_R_001_002_ReqCreateCrossGameInstance(
	 NPEnum.ENPCrossGameCategoryEnum _category
	, long _instanceId
	, byte[] _extData
) {	category = _category;
	instanceId = _instanceId;
	extData = _extData;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

/** 跨服游戏类型 */
public NPEnum.ENPCrossGameCategoryEnum getCategory() { return category; }
/** 跨服游戏类型 */
public void setCategory(NPEnum.ENPCrossGameCategoryEnum _category) { category = _category; }
/** 实例ID */
public long getInstanceId() { return instanceId; }
/** 实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 消息 */
public byte[] getExtData() { return extData; }
public java.nio.ByteBuffer get_buffer_ExtData() { if(null == extData)return null; else return ByteBuffer.wrap(extData); }

/** 消息 */
public void setExtData(byte[] _extData) { extData = _extData; }
public void setExtData(java.nio.ByteBuffer _extData) 
{
	if(null == _extData){return;}
	int _oldPos = _extData.position();
	int _bufLength = _extData.remaining();
	extData = new byte[_bufLength];
	_extData.get(extData);
	_extData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 12;
	_size += 4 + (extData == null ? 0 : extData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (extData == null ? 0 : extData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) category = NPEnum.ENPCrossGameCategoryEnum.ENPCrossGameCategoryEnum_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extDataCount = _buf.getInt();
	if(0 < _extDataCount){
		extData = new byte[_extDataCount];
		_buf.get(extData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(category.ordinal());

	_buf.putLong(instanceId);
	_buf.putInt((extData == null ? 0 : extData.length));
	if(null != extData){_buf.put(extData);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

