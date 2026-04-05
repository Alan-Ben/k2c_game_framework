package GC2GS.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 请求火星系统求助
 **/
public class GC2GS_042_001_ReqSendMarsHelp implements ALBasicProtocolPack._IALProtocolStructure {
/** 求助目标类型 */
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/** 求助目标ID */
private long objId;


public GC2GS_042_001_ReqSendMarsHelp() {
	objType = Common.GuildEnum.EGuildMarsHelpObjType.values()[0];
	objId = (long)0;
}

public GC2GS_042_001_ReqSendMarsHelp(
	 Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
) {	objType = _objType;
	objId = _objId;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)1; }

/** 求助目标类型 */
public Common.GuildEnum.EGuildMarsHelpObjType getObjType() { return objType; }
/** 求助目标类型 */
public void setObjType(Common.GuildEnum.EGuildMarsHelpObjType _objType) { objType = _objType; }
/** 求助目标ID */
public long getObjId() { return objId; }
/** 求助目标ID */
public void setObjId(long _objId) { objId = _objId; }


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
	if(_buf.remaining() > 0) objType = Common.GuildEnum.EGuildMarsHelpObjType.EGuildMarsHelpObjType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(objType.ordinal());

	_buf.putLong(objId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)1);
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

