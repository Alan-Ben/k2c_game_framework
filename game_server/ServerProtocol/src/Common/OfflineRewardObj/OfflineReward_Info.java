package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 离线奖励数据
 **/
public class OfflineReward_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 奖励类型 - ECommonOfflineRewardType类型 */
private int rewardType;
/** 奖励展示 */
private byte[] rewardShow;


public OfflineReward_Info() {
	id = (long)0;
	rewardType = 0;
	rewardShow = null;
}

public OfflineReward_Info(
	 long _id
	, int _rewardType
	, byte[] _rewardShow
) {	id = _id;
	rewardType = _rewardType;
	rewardShow = _rewardShow;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 奖励类型 - ECommonOfflineRewardType类型 */
public int getRewardType() { return rewardType; }
/** 奖励类型 - ECommonOfflineRewardType类型 */
public void setRewardType(int _rewardType) { rewardType = _rewardType; }
/** 奖励展示 */
public byte[] getRewardShow() { return rewardShow; }
public java.nio.ByteBuffer get_buffer_RewardShow() { if(null == rewardShow)return null; else return ByteBuffer.wrap(rewardShow); }

/** 奖励展示 */
public void setRewardShow(byte[] _rewardShow) { rewardShow = _rewardShow; }
public void setRewardShow(java.nio.ByteBuffer _rewardShow) 
{
	if(null == _rewardShow){return;}
	int _oldPos = _rewardShow.position();
	int _bufLength = _rewardShow.remaining();
	rewardShow = new byte[_bufLength];
	_rewardShow.get(rewardShow);
	_rewardShow.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 12;
	_size += 4 + (rewardShow == null ? 0 : rewardShow.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (rewardShow == null ? 0 : rewardShow.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewardType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rewardShowCount = _buf.getInt();
	if(0 < _rewardShowCount){
		rewardShow = new byte[_rewardShowCount];
		_buf.get(rewardShow);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(rewardType);
	_buf.putInt((rewardShow == null ? 0 : rewardShow.length));
	if(null != rewardShow){_buf.put(rewardShow);}

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

