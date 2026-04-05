package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 网页支付角色信息
 **/
public class ServerObj_WebPayRoleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 角色ID */
private long cid;
/** 用户ID */
private String uid;
/** 角色昵称 */
private String name;
/** 等级 */
private int lvl;
/** VIP等级 */
private int vipLvl;


public ServerObj_WebPayRoleInfo() {
	cid = (long)0;
	uid = "";
	name = "";
	lvl = 0;
	vipLvl = 0;
}

public ServerObj_WebPayRoleInfo(
	 long _cid
	, String _uid
	, String _name
	, int _lvl
	, int _vipLvl
) {	cid = _cid;
	uid = _uid;
	name = _name;
	lvl = _lvl;
	vipLvl = _vipLvl;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 角色ID */
public long getCid() { return cid; }
/** 角色ID */
public void setCid(long _cid) { cid = _cid; }
/** 用户ID */
public String getUid() { return uid; }
/** 用户ID */
public void setUid(String _uid) { uid = _uid; }
/** 角色昵称 */
public String getName() { return name; }
/** 角色昵称 */
public void setName(String _name) { name = _name; }
/** 等级 */
public int getLvl() { return lvl; }
/** 等级 */
public void setLvl(int _lvl) { lvl = _lvl; }
/** VIP等级 */
public int getVipLvl() { return vipLvl; }
/** VIP等级 */
public void setVipLvl(int _vipLvl) { vipLvl = _vipLvl; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) vipLvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putInt(lvl);
	_buf.putInt(vipLvl);
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

