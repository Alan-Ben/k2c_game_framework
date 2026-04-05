package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-队伍状态数据
 **/
public class Mars_TeamState implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍状态 */
private Common.MarsEnum.EMarsExploreTeamState state;
/** 队伍状态序列号 */
private long stateSerialize;
/** 状态开启时间（毫秒） */
private long stateStartMs;
/** 状态结束时间（毫秒） */
private long stateEndMs;
/** 状态对应的目标位置 */
private long pos;
/** 队伍状态的额外信息 */
private byte[] exData;


public Mars_TeamState() {
	state = Common.MarsEnum.EMarsExploreTeamState.values()[0];
	stateSerialize = (long)0;
	stateStartMs = (long)0;
	stateEndMs = (long)0;
	pos = (long)0;
	exData = null;
}

public Mars_TeamState(
	 Common.MarsEnum.EMarsExploreTeamState _state
	, long _stateSerialize
	, long _stateStartMs
	, long _stateEndMs
	, long _pos
	, byte[] _exData
) {	state = _state;
	stateSerialize = _stateSerialize;
	stateStartMs = _stateStartMs;
	stateEndMs = _stateEndMs;
	pos = _pos;
	exData = _exData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍状态 */
public Common.MarsEnum.EMarsExploreTeamState getState() { return state; }
/** 队伍状态 */
public void setState(Common.MarsEnum.EMarsExploreTeamState _state) { state = _state; }
/** 队伍状态序列号 */
public long getStateSerialize() { return stateSerialize; }
/** 队伍状态序列号 */
public void setStateSerialize(long _stateSerialize) { stateSerialize = _stateSerialize; }
/** 状态开启时间（毫秒） */
public long getStateStartMs() { return stateStartMs; }
/** 状态开启时间（毫秒） */
public void setStateStartMs(long _stateStartMs) { stateStartMs = _stateStartMs; }
/** 状态结束时间（毫秒） */
public long getStateEndMs() { return stateEndMs; }
/** 状态结束时间（毫秒） */
public void setStateEndMs(long _stateEndMs) { stateEndMs = _stateEndMs; }
/** 状态对应的目标位置 */
public long getPos() { return pos; }
/** 状态对应的目标位置 */
public void setPos(long _pos) { pos = _pos; }
/** 队伍状态的额外信息 */
public byte[] getExData() { return exData; }
public java.nio.ByteBuffer get_buffer_ExData() { if(null == exData)return null; else return ByteBuffer.wrap(exData); }

/** 队伍状态的额外信息 */
public void setExData(byte[] _exData) { exData = _exData; }
public void setExData(java.nio.ByteBuffer _exData) 
{
	if(null == _exData){return;}
	int _oldPos = _exData.position();
	int _bufLength = _exData.remaining();
	exData = new byte[_bufLength];
	_exData.get(exData);
	_exData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 36;
	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + (exData == null ? 0 : exData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) state = Common.MarsEnum.EMarsExploreTeamState.EMarsExploreTeamState_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stateSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stateStartMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stateEndMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exDataCount = _buf.getInt();
	if(0 < _exDataCount){
		exData = new byte[_exDataCount];
		_buf.get(exData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(state.ordinal());

	_buf.putLong(stateSerialize);
	_buf.putLong(stateStartMs);
	_buf.putLong(stateEndMs);
	_buf.putLong(pos);
	_buf.putInt((exData == null ? 0 : exData.length));
	if(null != exData){_buf.put(exData);}

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

