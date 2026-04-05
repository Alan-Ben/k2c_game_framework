package NP2GLS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2GLS_R_001_009_SyncGroupTeamData implements ALBasicProtocolPack._IALProtocolStructure {
/** 游戏逻辑实例ID */
private long instanceId;
/** US服务器ID */
private int usId;
/** 群组ID */
private long groupId;
/** 队伍ID */
private long teamId;


public NP2GLS_R_001_009_SyncGroupTeamData() {
	instanceId = (long)0;
	usId = 0;
	groupId = (long)0;
	teamId = (long)0;
}

public NP2GLS_R_001_009_SyncGroupTeamData(
	 long _instanceId
	, int _usId
	, long _groupId
	, long _teamId
) {	instanceId = _instanceId;
	usId = _usId;
	groupId = _groupId;
	teamId = _teamId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)9; }

/** 游戏逻辑实例ID */
public long getInstanceId() { return instanceId; }
/** 游戏逻辑实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** US服务器ID */
public int getUsId() { return usId; }
/** US服务器ID */
public void setUsId(int _usId) { usId = _usId; }
/** 群组ID */
public long getGroupId() { return groupId; }
/** 群组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 队伍ID */
public long getTeamId() { return teamId; }
/** 队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }


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
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(usId);
	_buf.putLong(groupId);
	_buf.putLong(teamId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)9);
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

