package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 自身发起的求助数据变更
 **/
public class GS2GC_042_050_OnMyMarsHelpChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 求助实例ID */
private long id;
/** 公会求助时长（秒） */
private int guildHelpSecs;
/** 对象类型 */
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/** 对象实例ID */
private long objId;


public GS2GC_042_050_OnMyMarsHelpChg() {
	id = (long)0;
	guildHelpSecs = 0;
	objType = Common.GuildEnum.EGuildMarsHelpObjType.values()[0];
	objId = (long)0;
}

public GS2GC_042_050_OnMyMarsHelpChg(
	 long _id
	, int _guildHelpSecs
	, Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
) {	id = _id;
	guildHelpSecs = _guildHelpSecs;
	objType = _objType;
	objId = _objId;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)50; }

/** 求助实例ID */
public long getId() { return id; }
/** 求助实例ID */
public void setId(long _id) { id = _id; }
/** 公会求助时长（秒） */
public int getGuildHelpSecs() { return guildHelpSecs; }
/** 公会求助时长（秒） */
public void setGuildHelpSecs(int _guildHelpSecs) { guildHelpSecs = _guildHelpSecs; }
/** 对象类型 */
public Common.GuildEnum.EGuildMarsHelpObjType getObjType() { return objType; }
/** 对象类型 */
public void setObjType(Common.GuildEnum.EGuildMarsHelpObjType _objType) { objType = _objType; }
/** 对象实例ID */
public long getObjId() { return objId; }
/** 对象实例ID */
public void setObjId(long _objId) { objId = _objId; }


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
	if(_buf.remaining() > 0) guildHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objType = Common.GuildEnum.EGuildMarsHelpObjType.EGuildMarsHelpObjType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(guildHelpSecs);
	_buf.putInt(objType.ordinal());

	_buf.putLong(objId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)50);
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

