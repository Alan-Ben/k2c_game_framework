package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_055_OnOfflineRewardAdd implements ALBasicProtocolPack._IALProtocolStructure {
private Common.OfflineRewardObj.OfflineReward_Info reward;


public GS2GC_007_055_OnOfflineRewardAdd() {
	reward = new Common.OfflineRewardObj.OfflineReward_Info();
}

public GS2GC_007_055_OnOfflineRewardAdd(
	 Common.OfflineRewardObj.OfflineReward_Info _reward
) {	reward = _reward;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)55; }

public Common.OfflineRewardObj.OfflineReward_Info getReward() { return reward; }
public void setReward(Common.OfflineRewardObj.OfflineReward_Info _reward) { reward = _reward; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + reward.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + reward.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rewardCustLen = _buf.getInt();
	int _rewardCurPos = _buf.position();
	reward.ReadUnzipBuf(_buf, _rewardCurPos + _rewardCustLen);
	_buf.position(_rewardCurPos + _rewardCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(reward.GetBufSize());
	reward.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)55);
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

