package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 宴会凭证数据
 **/
public class Dinner_Permit implements ALBasicProtocolPack._IALProtocolStructure {
/** 凭证实例ID */
private long id;
/** 凭证类型 */
private Common.DinnerEnum.EDinnerPermitType permitType;
/** 凭证类型ID */
private long typeId;
/** 凭证过期时间（秒） */
private int expiredTs;


public Dinner_Permit() {
	id = (long)0;
	permitType = Common.DinnerEnum.EDinnerPermitType.values()[0];
	typeId = (long)0;
	expiredTs = 0;
}

public Dinner_Permit(
	 long _id
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _typeId
	, int _expiredTs
) {	id = _id;
	permitType = _permitType;
	typeId = _typeId;
	expiredTs = _expiredTs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 凭证实例ID */
public long getId() { return id; }
/** 凭证实例ID */
public void setId(long _id) { id = _id; }
/** 凭证类型 */
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/** 凭证类型 */
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/** 凭证类型ID */
public long getTypeId() { return typeId; }
/** 凭证类型ID */
public void setTypeId(long _typeId) { typeId = _typeId; }
/** 凭证过期时间（秒） */
public int getExpiredTs() { return expiredTs; }
/** 凭证过期时间（秒） */
public void setExpiredTs(int _expiredTs) { expiredTs = _expiredTs; }


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
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitType = Common.DinnerEnum.EDinnerPermitType.EDinnerPermitType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) typeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTs = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(permitType.ordinal());

	_buf.putLong(typeId);
	_buf.putInt(expiredTs);
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

