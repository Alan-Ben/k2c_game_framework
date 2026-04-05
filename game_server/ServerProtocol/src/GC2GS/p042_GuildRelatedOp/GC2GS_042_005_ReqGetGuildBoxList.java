package GC2GS.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 获取可领取联盟宝箱列表
 **/
public class GC2GS_042_005_ReqGetGuildBoxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱类型 */
private Common.GuildEnum.EGuildBoxType boxType;


public GC2GS_042_005_ReqGetGuildBoxList() {
	boxType = Common.GuildEnum.EGuildBoxType.values()[0];
}

public GC2GS_042_005_ReqGetGuildBoxList(
	 Common.GuildEnum.EGuildBoxType _boxType
) {	boxType = _boxType;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)5; }

/** 宝箱类型 */
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/** 宝箱类型 */
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxType = Common.GuildEnum.EGuildBoxType.EGuildBoxType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boxType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)5);
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

