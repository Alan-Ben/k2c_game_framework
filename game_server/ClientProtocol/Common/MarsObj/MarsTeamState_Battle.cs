using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星队伍-battle战斗状态数据
/// </summary>
public class MarsTeamState_Battle : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件实例ID
/// </summary>
private long eventInstanceId;
/// <summary>
/// 行军用时（毫秒），用于返程时长
/// </summary>
private long marchTimeMS;
/// <summary>
/// 所在位置
/// </summary>
private long pos;


public MarsTeamState_Battle() {
	eventInstanceId = (long)0;
	marchTimeMS = (long)0;
	pos = (long)0;
}

public MarsTeamState_Battle(
	long _eventInstanceId
	, long _marchTimeMS
	, long _pos
) {	eventInstanceId = _eventInstanceId;
	marchTimeMS = _marchTimeMS;
	pos = _pos;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 事件实例ID
/// </summary>
public long getEventInstanceId() { return eventInstanceId; }
/// <summary>
/// 事件实例ID
/// </summary>
public void setEventInstanceId(long _eventInstanceId) { eventInstanceId = _eventInstanceId; }
/// <summary>
/// 行军用时（毫秒），用于返程时长
/// </summary>
public long getMarchTimeMS() { return marchTimeMS; }
/// <summary>
/// 行军用时（毫秒），用于返程时长
/// </summary>
public void setMarchTimeMS(long _marchTimeMS) { marchTimeMS = _marchTimeMS; }
/// <summary>
/// 所在位置
/// </summary>
public long getPos() { return pos; }
/// <summary>
/// 所在位置
/// </summary>
public void setPos(long _pos) { pos = _pos; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	marchTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pos = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(eventInstanceId);
	_buf.putLong(marchTimeMS);
	_buf.putLong(pos);
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
	builder.Append("eventInstanceId").Append(":").Append(eventInstanceId.ToString()).Append(", ");
	builder.Append("marchTimeMS").Append(":").Append(marchTimeMS.ToString()).Append(", ");
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

