package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 请求创建联盟
 **/
public class GC2GS_032_001_ReqCreateGuild implements ALBasicProtocolPack._IALProtocolStructure {
/** 旗帜id */
private long flagId;
/** 名称 */
private String name;
/** 简称 */
private String simpleName;
/** 宣言 */
private String declaration;
/** 是否允许自由加入 */
private boolean canFreeJoin;


public GC2GS_032_001_ReqCreateGuild() {
	flagId = (long)0;
	name = "";
	simpleName = "";
	declaration = "";
	canFreeJoin = false;
}

public GC2GS_032_001_ReqCreateGuild(
	 long _flagId
	, String _name
	, String _simpleName
	, String _declaration
	, boolean _canFreeJoin
) {	flagId = _flagId;
	name = _name;
	simpleName = _simpleName;
	declaration = _declaration;
	canFreeJoin = _canFreeJoin;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)1; }

/** 旗帜id */
public long getFlagId() { return flagId; }
/** 旗帜id */
public void setFlagId(long _flagId) { flagId = _flagId; }
/** 名称 */
public String getName() { return name; }
/** 名称 */
public void setName(String _name) { name = _name; }
/** 简称 */
public String getSimpleName() { return simpleName; }
/** 简称 */
public void setSimpleName(String _simpleName) { simpleName = _simpleName; }
/** 宣言 */
public String getDeclaration() { return declaration; }
/** 宣言 */
public void setDeclaration(String _declaration) { declaration = _declaration; }
/** 是否允许自由加入 */
public boolean getCanFreeJoin() { return canFreeJoin; }
/** 是否允许自由加入 */
public void setCanFreeJoin(boolean _canFreeJoin) { canFreeJoin = _canFreeJoin; }


public final int GetBufSize() {
	int _size = 9;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) flagId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) simpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) declaration = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canFreeJoin = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(flagId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, simpleName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, declaration);
	_buf.put(canFreeJoin?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
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

