using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.QuestObj
{

/// <summary>
/// 日常任务数据
/// </summary>
public class DailyQuest_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标配置ID
/// </summary>
private long targetId;
/// <summary>
/// 当前计数
/// </summary>
private long curCount;
/// <summary>
/// 是否已完成,且领取奖励
/// </summary>
private bool isFinish;


public DailyQuest_Info() {
	targetId = (long)0;
	curCount = (long)0;
	isFinish = false;
}

public DailyQuest_Info(
	long _targetId
	, long _curCount
	, bool _isFinish
) {	targetId = _targetId;
	curCount = _curCount;
	isFinish = _isFinish;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 目标配置ID
/// </summary>
public long getTargetId() { return targetId; }
/// <summary>
/// 目标配置ID
/// </summary>
public void setTargetId(long _targetId) { targetId = _targetId; }
/// <summary>
/// 当前计数
/// </summary>
public long getCurCount() { return curCount; }
/// <summary>
/// 当前计数
/// </summary>
public void setCurCount(long _curCount) { curCount = _curCount; }
/// <summary>
/// 是否已完成,且领取奖励
/// </summary>
public bool getIsFinish() { return isFinish; }
/// <summary>
/// 是否已完成,且领取奖励
/// </summary>
public void setIsFinish(bool _isFinish) { isFinish = _isFinish; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isFinish = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(targetId);
	_buf.putLong(curCount);
	_buf.put(isFinish?(byte)1:(byte)0);
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
	builder.Append("targetId").Append(":").Append(targetId.ToString()).Append(", ");
	builder.Append("curCount").Append(":").Append(curCount.ToString()).Append(", ");
	builder.Append("isFinish").Append(":").Append(isFinish.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

