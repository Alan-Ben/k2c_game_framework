using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 活动冲榜-客户端数据
/// </summary>
public class ActivityRankRush_ClientData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 排行榜id
/// </summary>
private long rankId;
/// <summary>
/// 记录的冲榜初始值
/// </summary>
private long recordInitialValue;
/// <summary>
/// 排名
/// </summary>
private long ranking;


public ActivityRankRush_ClientData() {
	instanceId = (long)0;
	rankId = (long)0;
	recordInitialValue = (long)0;
	ranking = (long)0;
}

public ActivityRankRush_ClientData(
	long _instanceId
	, long _rankId
	, long _recordInitialValue
	, long _ranking
) {	instanceId = _instanceId;
	rankId = _rankId;
	recordInitialValue = _recordInitialValue;
	ranking = _ranking;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 排行榜id
/// </summary>
public long getRankId() { return rankId; }
/// <summary>
/// 排行榜id
/// </summary>
public void setRankId(long _rankId) { rankId = _rankId; }
/// <summary>
/// 记录的冲榜初始值
/// </summary>
public long getRecordInitialValue() { return recordInitialValue; }
/// <summary>
/// 记录的冲榜初始值
/// </summary>
public void setRecordInitialValue(long _recordInitialValue) { recordInitialValue = _recordInitialValue; }
/// <summary>
/// 排名
/// </summary>
public long getRanking() { return ranking; }
/// <summary>
/// 排名
/// </summary>
public void setRanking(long _ranking) { ranking = _ranking; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	recordInitialValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ranking = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(rankId);
	_buf.putLong(recordInitialValue);
	_buf.putLong(ranking);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("rankId").Append(":").Append(rankId.ToString()).Append(", ");
	builder.Append("recordInitialValue").Append(":").Append(recordInitialValue.ToString()).Append(", ");
	builder.Append("ranking").Append(":").Append(ranking.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

