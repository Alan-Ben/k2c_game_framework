package GC2GS.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 购买活动钻石礼包
 **/
public class GC2GS_017_017_ReqBuyActivityCrystalGiftPack implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 礼包组id */
private long groupId;
/** 礼包id */
private long packId;
/** 购买数量 */
private int num;


public GC2GS_017_017_ReqBuyActivityCrystalGiftPack() {
	instanceId = (long)0;
	groupId = (long)0;
	packId = (long)0;
	num = 0;
}

public GC2GS_017_017_ReqBuyActivityCrystalGiftPack(
	 long _instanceId
	, long _groupId
	, long _packId
	, int _num
) {	instanceId = _instanceId;
	groupId = _groupId;
	packId = _packId;
	num = _num;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)17; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 礼包组id */
public long getGroupId() { return groupId; }
/** 礼包组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 礼包id */
public long getPackId() { return packId; }
/** 礼包id */
public void setPackId(long _packId) { packId = _packId; }
/** 购买数量 */
public int getNum() { return num; }
/** 购买数量 */
public void setNum(int _num) { num = _num; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) packId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(groupId);
	_buf.putLong(packId);
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)17);
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

