package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟火星求助展示信息
 **/
public class Guild_MarsHelpShowInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 求助实例ID */
private long id;
/** 发起玩家CID */
private long senderCid;
/** 对象类型 */
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/** 对象实例ID */
private long objId;
/** 求助允许处理的次数上限 */
private int dealLimit;
/** 被帮助的次数 */
private int dealedCount;
/** 额外数据 */
private byte[] ext;


public Guild_MarsHelpShowInfo() {
	id = (long)0;
	senderCid = (long)0;
	objType = Common.GuildEnum.EGuildMarsHelpObjType.values()[0];
	objId = (long)0;
	dealLimit = 0;
	dealedCount = 0;
	ext = null;
}

public Guild_MarsHelpShowInfo(
	 long _id
	, long _senderCid
	, Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
	, int _dealLimit
	, int _dealedCount
	, byte[] _ext
) {	id = _id;
	senderCid = _senderCid;
	objType = _objType;
	objId = _objId;
	dealLimit = _dealLimit;
	dealedCount = _dealedCount;
	ext = _ext;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 求助实例ID */
public long getId() { return id; }
/** 求助实例ID */
public void setId(long _id) { id = _id; }
/** 发起玩家CID */
public long getSenderCid() { return senderCid; }
/** 发起玩家CID */
public void setSenderCid(long _senderCid) { senderCid = _senderCid; }
/** 对象类型 */
public Common.GuildEnum.EGuildMarsHelpObjType getObjType() { return objType; }
/** 对象类型 */
public void setObjType(Common.GuildEnum.EGuildMarsHelpObjType _objType) { objType = _objType; }
/** 对象实例ID */
public long getObjId() { return objId; }
/** 对象实例ID */
public void setObjId(long _objId) { objId = _objId; }
/** 求助允许处理的次数上限 */
public int getDealLimit() { return dealLimit; }
/** 求助允许处理的次数上限 */
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }
/** 被帮助的次数 */
public int getDealedCount() { return dealedCount; }
/** 被帮助的次数 */
public void setDealedCount(int _dealedCount) { dealedCount = _dealedCount; }
/** 额外数据 */
public byte[] getExt() { return ext; }
public java.nio.ByteBuffer get_buffer_Ext() { if(null == ext)return null; else return ByteBuffer.wrap(ext); }

/** 额外数据 */
public void setExt(byte[] _ext) { ext = _ext; }
public void setExt(java.nio.ByteBuffer _ext) 
{
	if(null == _ext){return;}
	int _oldPos = _ext.position();
	int _bufLength = _ext.remaining();
	ext = new byte[_bufLength];
	_ext.get(ext);
	_ext.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 36;
	_size += 4 + (ext == null ? 0 : ext.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + (ext == null ? 0 : ext.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objType = Common.GuildEnum.EGuildMarsHelpObjType.EGuildMarsHelpObjType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extCount = _buf.getInt();
	if(0 < _extCount){
		ext = new byte[_extCount];
		_buf.get(ext);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(senderCid);
	_buf.putInt(objType.ordinal());

	_buf.putLong(objId);
	_buf.putInt(dealLimit);
	_buf.putInt(dealedCount);
	_buf.putInt((ext == null ? 0 : ext.length));
	if(null != ext){_buf.put(ext);}

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

