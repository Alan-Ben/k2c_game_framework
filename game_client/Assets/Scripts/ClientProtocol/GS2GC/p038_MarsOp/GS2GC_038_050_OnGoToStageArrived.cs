using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p038_MarsOp
{

/// <summary>
/// 前往火星-到达新阶段
/// </summary>
public class GS2GC_038_050_OnGoToStageArrived : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当前阶段
/// </summary>
private int stage;
/// <summary>
/// 开启前往火星时间（毫秒）
/// </summary>
private long startMs;
/// <summary>
/// 当前阶段开始时间（毫秒）
/// </summary>
private long stageStartMs;


public GS2GC_038_050_OnGoToStageArrived() {
	stage = 0;
	startMs = (long)0;
	stageStartMs = (long)0;
}

public GS2GC_038_050_OnGoToStageArrived(
	int _stage
	, long _startMs
	, long _stageStartMs
) {	stage = _stage;
	startMs = _startMs;
	stageStartMs = _stageStartMs;
}

public byte getMainOrder() { return (byte)38; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 当前阶段
/// </summary>
public int getStage() { return stage; }
/// <summary>
/// 当前阶段
/// </summary>
public void setStage(int _stage) { stage = _stage; }
/// <summary>
/// 开启前往火星时间（毫秒）
/// </summary>
public long getStartMs() { return startMs; }
/// <summary>
/// 开启前往火星时间（毫秒）
/// </summary>
public void setStartMs(long _startMs) { startMs = _startMs; }
/// <summary>
/// 当前阶段开始时间（毫秒）
/// </summary>
public long getStageStartMs() { return stageStartMs; }
/// <summary>
/// 当前阶段开始时间（毫秒）
/// </summary>
public void setStageStartMs(long _stageStartMs) { stageStartMs = _stageStartMs; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stage = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stageStartMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stage);
	_buf.putLong(startMs);
	_buf.putLong(stageStartMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)38);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)38);
	_recBuf.put((byte)50);
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
	builder.Append("stage").Append(":").Append(stage.ToString()).Append(", ");
	builder.Append("startMs").Append(":").Append(startMs.ToString()).Append(", ");
	builder.Append("stageStartMs").Append(":").Append(stageStartMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

