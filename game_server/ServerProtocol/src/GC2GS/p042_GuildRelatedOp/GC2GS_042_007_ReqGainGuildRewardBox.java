package GC2GS.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 领取联盟宝箱奖励
 **/
public class GC2GS_042_007_ReqGainGuildRewardBox implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱类型 */
private Common.GuildEnum.EGuildBoxType boxType;
/** 宝箱实例ID */
private long id;


public GC2GS_042_007_ReqGainGuildRewardBox() {
	boxType = Common.GuildEnum.EGuildBoxType.values()[0];
	id = (long)0;
}

public GC2GS_042_007_ReqGainGuildRewardBox(
	 Common.GuildEnum.EGuildBoxType _boxType
	, long _id
) {	boxType = _boxType;
	id = _id;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)7; }

/** 宝箱类型 */
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/** 宝箱类型 */
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
/** 宝箱实例ID */
public long getId() { return id; }
/** 宝箱实例ID */
public void setId(long _id) { id = _id; }


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
	if(_buf.remaining() > 0) boxType = Common.GuildEnum.EGuildBoxType.EGuildBoxType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boxType.ordinal());

	_buf.putLong(id);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
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

