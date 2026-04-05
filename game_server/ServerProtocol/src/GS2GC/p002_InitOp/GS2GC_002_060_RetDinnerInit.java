package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 宴会初始化
 **/
public class GS2GC_002_060_RetDinnerInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 正在举办的宴会实例ID */
private long instanceId;
/** 是否有开宴的奖励 */
private boolean hasOwnerReward;
/** 宴会凭证数据列表 */
private java.util.ArrayList<Common.DinnerObj.Dinner_Permit> permitList;


public GS2GC_002_060_RetDinnerInit() {
	instanceId = (long)0;
	hasOwnerReward = false;
	permitList = new java.util.ArrayList<Common.DinnerObj.Dinner_Permit>();
}

public GS2GC_002_060_RetDinnerInit(
	 long _instanceId
	, boolean _hasOwnerReward
	, java.util.ArrayList<Common.DinnerObj.Dinner_Permit> _permitList
) {	instanceId = _instanceId;
	hasOwnerReward = _hasOwnerReward;
	permitList = _permitList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)60; }

/** 正在举办的宴会实例ID */
public long getInstanceId() { return instanceId; }
/** 正在举办的宴会实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 是否有开宴的奖励 */
public boolean getHasOwnerReward() { return hasOwnerReward; }
/** 是否有开宴的奖励 */
public void setHasOwnerReward(boolean _hasOwnerReward) { hasOwnerReward = _hasOwnerReward; }
/** 宴会凭证数据列表 */
public java.util.ArrayList<Common.DinnerObj.Dinner_Permit> getPermitList() { return permitList; }
/** 宴会凭证数据列表 */
public void addPermitList(Common.DinnerObj.Dinner_Permit _permitList) { permitList.add(_permitList); }


public final int GetBufSize() {
	int _size = 9;
	_size += 2 + (permitList.size() * 28);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += 2 + (permitList.size() * 28);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasOwnerReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _permitListCount = _buf.getShort();
	for(int _i = 0; _i < _permitListCount; _i++) { 
		Common.DinnerObj.Dinner_Permit _permitList = new Common.DinnerObj.Dinner_Permit();
		if(_buf.remaining() <= 0) return;
	int __permitListCustLen = _buf.getInt();
	int __permitListCurPos = _buf.position();
	_permitList.ReadUnzipBuf(_buf, __permitListCurPos + __permitListCustLen);
	_buf.position(__permitListCurPos + __permitListCustLen);

		permitList.add(_permitList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.put(hasOwnerReward?(byte)1:(byte)0);
	_buf.putShort((short)permitList.size());
	for(int _i = 0; _i < permitList.size(); _i++) { 
		_buf.putInt(permitList.get(_i).GetBufSize());
	permitList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)60);
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

