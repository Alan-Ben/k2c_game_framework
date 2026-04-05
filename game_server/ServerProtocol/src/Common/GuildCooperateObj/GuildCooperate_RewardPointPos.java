package Common.GuildCooperateObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作奖励点位置
 **/
public class GuildCooperate_RewardPointPos implements ALBasicProtocolPack._IALProtocolStructure {
/** 区域ID */
private long areaId;
/** 奖励点索引，从0开始 */
private int index;


public GuildCooperate_RewardPointPos() {
	areaId = (long)0;
	index = 0;
}

public GuildCooperate_RewardPointPos(
	 long _areaId
	, int _index
) {	areaId = _areaId;
	index = _index;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 区域ID */
public long getAreaId() { return areaId; }
/** 区域ID */
public void setAreaId(long _areaId) { areaId = _areaId; }
/** 奖励点索引，从0开始 */
public int getIndex() { return index; }
/** 奖励点索引，从0开始 */
public void setIndex(int _index) { index = _index; }


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
	if(_buf.remaining() > 0) areaId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) index = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(areaId);
	_buf.putInt(index);
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

