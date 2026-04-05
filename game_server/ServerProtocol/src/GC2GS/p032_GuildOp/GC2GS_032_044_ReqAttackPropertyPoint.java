package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 攻击属性据点
 **/
public class GC2GS_032_044_ReqAttackPropertyPoint implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励据点位置 */
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/** 属性据点索引 */
private int propertyPointIndex;
/** 出战大臣ID */
private long heroId;


public GC2GS_032_044_ReqAttackPropertyPoint() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	propertyPointIndex = 0;
	heroId = (long)0;
}

public GC2GS_032_044_ReqAttackPropertyPoint(
	 Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, int _propertyPointIndex
	, long _heroId
) {	pos = _pos;
	propertyPointIndex = _propertyPointIndex;
	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)44; }

/** 奖励据点位置 */
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/** 奖励据点位置 */
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/** 属性据点索引 */
public int getPropertyPointIndex() { return propertyPointIndex; }
/** 属性据点索引 */
public void setPropertyPointIndex(int _propertyPointIndex) { propertyPointIndex = _propertyPointIndex; }
/** 出战大臣ID */
public long getHeroId() { return heroId; }
/** 出战大臣ID */
public void setHeroId(long _heroId) { heroId = _heroId; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.position();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.position(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) propertyPointIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putInt(propertyPointIndex);
	_buf.putLong(heroId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)44);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)44);
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

