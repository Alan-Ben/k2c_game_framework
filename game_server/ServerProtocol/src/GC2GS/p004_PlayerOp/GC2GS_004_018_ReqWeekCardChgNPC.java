package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 请求修改周卡NPC
 **/
public class GC2GS_004_018_ReqWeekCardChgNPC implements ALBasicProtocolPack._IALProtocolStructure {
/** NPC类型 */
private CommonEnum.EWeekCardNPCType npcType;
private long npcId;


public GC2GS_004_018_ReqWeekCardChgNPC() {
	npcType = CommonEnum.EWeekCardNPCType.values()[0];
	npcId = (long)0;
}

public GC2GS_004_018_ReqWeekCardChgNPC(
	 CommonEnum.EWeekCardNPCType _npcType
	, long _npcId
) {	npcType = _npcType;
	npcId = _npcId;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)18; }

/** NPC类型 */
public CommonEnum.EWeekCardNPCType getNpcType() { return npcType; }
/** NPC类型 */
public void setNpcType(CommonEnum.EWeekCardNPCType _npcType) { npcType = _npcType; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }


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
	if(_buf.remaining() > 0) npcType = CommonEnum.EWeekCardNPCType.EWeekCardNPCType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) npcId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(npcType.ordinal());

	_buf.putLong(npcId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)18);
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

