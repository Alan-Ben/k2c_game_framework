package ALLRPC.DinnerServer.Dinner;

import java.nio.ByteBuffer;
public class DnsJoinDinner_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long groupId;
private long instanceId;
/** 是否玩家 */
private boolean isPlayer;
private long joinerId;
/** 参与宴会带来的宴会人气 */
private long joinerScore;


public DnsJoinDinner_Req() {
	groupId = (long)0;
	instanceId = (long)0;
	isPlayer = false;
	joinerId = (long)0;
	joinerScore = (long)0;
}

public DnsJoinDinner_Req(
	 long _groupId
	, long _instanceId
	, boolean _isPlayer
	, long _joinerId
	, long _joinerScore
) {	groupId = _groupId;
	instanceId = _instanceId;
	isPlayer = _isPlayer;
	joinerId = _joinerId;
	joinerScore = _joinerScore;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGroupId() { return groupId; }
public void setGroupId(long _groupId) { groupId = _groupId; }
public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 是否玩家 */
public boolean getIsPlayer() { return isPlayer; }
/** 是否玩家 */
public void setIsPlayer(boolean _isPlayer) { isPlayer = _isPlayer; }
public long getJoinerId() { return joinerId; }
public void setJoinerId(long _joinerId) { joinerId = _joinerId; }
/** 参与宴会带来的宴会人气 */
public long getJoinerScore() { return joinerScore; }
/** 参与宴会带来的宴会人气 */
public void setJoinerScore(long _joinerScore) { joinerScore = _joinerScore; }


public final int GetBufSize() {
	int _size = 33;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isPlayer = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinerScore = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(instanceId);
	_buf.put(isPlayer?(byte)1:(byte)0);
	_buf.putLong(joinerId);
	_buf.putLong(joinerScore);
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

