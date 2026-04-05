package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 联盟宝箱新增数量变更
 **/
public class GS2GC_042_055_OnGuildBoxAddCountChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱类型 */
private Common.GuildEnum.EGuildBoxType boxType;
/** 新增数量 */
private int addCount;


public GS2GC_042_055_OnGuildBoxAddCountChg() {
	boxType = Common.GuildEnum.EGuildBoxType.values()[0];
	addCount = 0;
}

public GS2GC_042_055_OnGuildBoxAddCountChg(
	 Common.GuildEnum.EGuildBoxType _boxType
	, int _addCount
) {	boxType = _boxType;
	addCount = _addCount;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)55; }

/** 宝箱类型 */
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/** 宝箱类型 */
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
/** 新增数量 */
public int getAddCount() { return addCount; }
/** 新增数量 */
public void setAddCount(int _addCount) { addCount = _addCount; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxType = Common.GuildEnum.EGuildBoxType.EGuildBoxType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boxType.ordinal());

	_buf.putInt(addCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)55);
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

