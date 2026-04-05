package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-信息
 **/
public class WeekCard_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 过期时间 超时时间标记，单位秒 */
private int expireTimeTagS;
/** 是否使用过免费试用 */
private boolean hadUseFreeTrial;
/** NPC类型 */
private CommonEnum.EWeekCardNPCType npcType;
private long npcId;


public WeekCard_Info() {
	expireTimeTagS = 0;
	hadUseFreeTrial = false;
	npcType = CommonEnum.EWeekCardNPCType.values()[0];
	npcId = (long)0;
}

public WeekCard_Info(
	 int _expireTimeTagS
	, boolean _hadUseFreeTrial
	, CommonEnum.EWeekCardNPCType _npcType
	, long _npcId
) {	expireTimeTagS = _expireTimeTagS;
	hadUseFreeTrial = _hadUseFreeTrial;
	npcType = _npcType;
	npcId = _npcId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 过期时间 超时时间标记，单位秒 */
public int getExpireTimeTagS() { return expireTimeTagS; }
/** 过期时间 超时时间标记，单位秒 */
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }
/** 是否使用过免费试用 */
public boolean getHadUseFreeTrial() { return hadUseFreeTrial; }
/** 是否使用过免费试用 */
public void setHadUseFreeTrial(boolean _hadUseFreeTrial) { hadUseFreeTrial = _hadUseFreeTrial; }
/** NPC类型 */
public CommonEnum.EWeekCardNPCType getNpcType() { return npcType; }
/** NPC类型 */
public void setNpcType(CommonEnum.EWeekCardNPCType _npcType) { npcType = _npcType; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTimeTagS = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadUseFreeTrial = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) npcType = CommonEnum.EWeekCardNPCType.EWeekCardNPCType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) npcId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(expireTimeTagS);
	_buf.put(hadUseFreeTrial?(byte)1:(byte)0);
	_buf.putInt(npcType.ordinal());

	_buf.putLong(npcId);
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

