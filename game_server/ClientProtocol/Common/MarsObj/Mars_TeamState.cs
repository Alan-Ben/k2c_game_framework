using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-队伍状态数据
/// </summary>
public class Mars_TeamState : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍状态
/// </summary>
private Common.MarsEnum.EMarsExploreTeamState state;
/// <summary>
/// 队伍状态序列号
/// </summary>
private long stateSerialize;
/// <summary>
/// 状态开启时间（毫秒）
/// </summary>
private long stateStartMs;
/// <summary>
/// 状态结束时间（毫秒）
/// </summary>
private long stateEndMs;
/// <summary>
/// 状态对应的目标位置
/// </summary>
private long pos;
/// <summary>
/// 队伍状态的额外信息
/// </summary>
private byte[] exData;


public Mars_TeamState() {
	state = 0;
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 队伍状态
/// </summary>
public Common.MarsEnum.EMarsExploreTeamState getState() { return state; }
/// <summary>
/// 队伍状态
/// </summary>
public void setState(Common.MarsEnum.EMarsExploreTeamState _state) { state = _state; }
/// <summary>
/// 队伍状态序列号
/// </summary>
public long getStateSerialize() { return stateSerialize; }
/// <summary>
/// 队伍状态序列号
/// </summary>
public void setStateSerialize(long _stateSerialize) { stateSerialize = _stateSerialize; }
/// <summary>
/// 状态开启时间（毫秒）
/// </summary>
public long getStateStartMs() { return stateStartMs; }
/// <summary>
/// 状态开启时间（毫秒）
/// </summary>
public void setStateStartMs(long _stateStartMs) { stateStartMs = _stateStartMs; }
/// <summary>
/// 状态结束时间（毫秒）
/// </summary>
public long getStateEndMs() { return stateEndMs; }
/// <summary>
/// 状态结束时间（毫秒）
/// </summary>
public void setStateEndMs(long _stateEndMs) { stateEndMs = _stateEndMs; }
/// <summary>
/// 状态对应的目标位置
/// </summary>
public long getPos() { return pos; }
/// <summary>
/// 状态对应的目标位置
/// </summary>
public void setPos(long _pos) { pos = _pos; }
/// <summary>
/// 队伍状态的额外信息
/// </summary>
public byte[] getExData() { return exData; }

/// <summary>
/// 队伍状态的额外信息
/// </summary>
public void setExData(byte[] _exData) { exData = _exData; }



public int GetBufSize() {
	int _size = 36;
	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	state = (Common.MarsEnum.EMarsExploreTeamState)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stateSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stateStartMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stateEndMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)state);

	_buf.putLong(stateSerialize);
	_buf.putLong(stateStartMs);
	_buf.putLong(stateEndMs);
	_buf.putLong(pos);
	_buf.putByteBuffer(exData);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("state").Append(":").Append(state.ToString()).Append(", ");
	builder.Append("stateSerialize").Append(":").Append(stateSerialize.ToString()).Append(", ");
	builder.Append("stateStartMs").Append(":").Append(stateStartMs.ToString()).Append(", ");
	builder.Append("stateEndMs").Append(":").Append(stateEndMs.ToString()).Append(", ");
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("exData").Append(":").Append(exData == null ? "null" : exData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

