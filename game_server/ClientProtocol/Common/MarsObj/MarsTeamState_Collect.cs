using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星队伍-采集状态数据
/// </summary>
public class MarsTeamState_Collect : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 火星矿实例ID
/// </summary>
private long mineInstanceId;
/// <summary>
/// 行军用时（毫秒），用于返程时长
/// </summary>
private long marchTimeMS;
/// <summary>
/// 所在位置
/// </summary>
private long pos;
/// <summary>
/// 采集截至时间（毫秒）
/// </summary>
private long endCollectMs;
/// <summary>
/// 采集开始时间（毫秒）
/// </summary>
private long startCollectMs;


public MarsTeamState_Collect() {
	mineInstanceId = (long)0;
	marchTimeMS = (long)0;
	pos = (long)0;
	endCollectMs = (long)0;
	startCollectMs = (long)0;
}

public MarsTeamState_Collect(
	long _mineInstanceId
	, long _marchTimeMS
	, long _pos
	, long _endCollectMs
	, long _startCollectMs
) {	mineInstanceId = _mineInstanceId;
	marchTimeMS = _marchTimeMS;
	pos = _pos;
	endCollectMs = _endCollectMs;
	startCollectMs = _startCollectMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 火星矿实例ID
/// </summary>
public long getMineInstanceId() { return mineInstanceId; }
/// <summary>
/// 火星矿实例ID
/// </summary>
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
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
/// <summary>
/// 采集截至时间（毫秒）
/// </summary>
public long getEndCollectMs() { return endCollectMs; }
/// <summary>
/// 采集截至时间（毫秒）
/// </summary>
public void setEndCollectMs(long _endCollectMs) { endCollectMs = _endCollectMs; }
/// <summary>
/// 采集开始时间（毫秒）
/// </summary>
public long getStartCollectMs() { return startCollectMs; }
/// <summary>
/// 采集开始时间（毫秒）
/// </summary>
public void setStartCollectMs(long _startCollectMs) { startCollectMs = _startCollectMs; }


public int GetBufSize() {
	int _size = 40;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	marchTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startCollectMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mineInstanceId);
	_buf.putLong(marchTimeMS);
	_buf.putLong(pos);
	_buf.putLong(endCollectMs);
	_buf.putLong(startCollectMs);
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
	builder.Append("mineInstanceId").Append(":").Append(mineInstanceId.ToString()).Append(", ");
	builder.Append("marchTimeMS").Append(":").Append(marchTimeMS.ToString()).Append(", ");
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("endCollectMs").Append(":").Append(endCollectMs.ToString()).Append(", ");
	builder.Append("startCollectMs").Append(":").Append(startCollectMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

