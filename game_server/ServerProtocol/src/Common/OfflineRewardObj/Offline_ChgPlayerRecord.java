package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 玩家记录操作
 **/
public class Offline_ChgPlayerRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 记录类型 */
private NPEnum.ENPPlayerRecordParam type;
/** 操作类型 */
private NPEnum.ENCounterDealType dealType;
/** 数值 */
private long num;


public Offline_ChgPlayerRecord() {
	type = NPEnum.ENPPlayerRecordParam.values()[0];
	dealType = NPEnum.ENCounterDealType.values()[0];
	num = (long)0;
}

public Offline_ChgPlayerRecord(
	 NPEnum.ENPPlayerRecordParam _type
	, NPEnum.ENCounterDealType _dealType
	, long _num
) {	type = _type;
	dealType = _dealType;
	num = _num;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 记录类型 */
public NPEnum.ENPPlayerRecordParam getType() { return type; }
/** 记录类型 */
public void setType(NPEnum.ENPPlayerRecordParam _type) { type = _type; }
/** 操作类型 */
public NPEnum.ENCounterDealType getDealType() { return dealType; }
/** 操作类型 */
public void setDealType(NPEnum.ENCounterDealType _dealType) { dealType = _dealType; }
/** 数值 */
public long getNum() { return num; }
/** 数值 */
public void setNum(long _num) { num = _num; }


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
	if(_buf.remaining() > 0) type = NPEnum.ENPPlayerRecordParam.ENPPlayerRecordParam_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealType = NPEnum.ENCounterDealType.ENCounterDealType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt(dealType.ordinal());

	_buf.putLong(num);
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

