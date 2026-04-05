using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_024_RetRefreshChatBoxStatus : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 宝箱状态
/// </summary>
private NPEnum.ENPBoxChatStatus boxStatus;
/// <summary>
/// 已经领取的玩家CID列表
/// </summary>
private List<long> gainedCidList;


public GS2GC_004_024_RetRefreshChatBoxStatus() {
	instanceId = (long)0;
	boxStatus = 0;
	gainedCidList = new List<long>();
}

public GS2GC_004_024_RetRefreshChatBoxStatus(
	long _instanceId
	, NPEnum.ENPBoxChatStatus _boxStatus
	, List<long> _gainedCidList
) {	instanceId = _instanceId;
	boxStatus = _boxStatus;
	gainedCidList = _gainedCidList;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)24; }

/// <summary>
/// 宝箱实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 宝箱实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 宝箱状态
/// </summary>
public NPEnum.ENPBoxChatStatus getBoxStatus() { return boxStatus; }
/// <summary>
/// 宝箱状态
/// </summary>
public void setBoxStatus(NPEnum.ENPBoxChatStatus _boxStatus) { boxStatus = _boxStatus; }
/// <summary>
/// 已经领取的玩家CID列表
/// </summary>
public List<long> getGainedCidList() { return gainedCidList; }
/// <summary>
/// 已经领取的玩家CID列表
/// </summary>
public void addGainedCidList(long _gainedCidList) { gainedCidList.Add(_gainedCidList); }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (gainedCidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (gainedCidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	boxStatus = (NPEnum.ENPBoxChatStatus)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _gainedCidListCount = _buf.getShort();
	for(int _i = 0; _i < _gainedCidListCount; _i++) { 
		long _gainedCidList = (long)0;
		_gainedCidList = _buf.getLong();
		gainedCidList.Add(_gainedCidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt((int)boxStatus);

	_buf.putShort((short)gainedCidList.Count);
	for(int _i = 0; _i < gainedCidList.Count; _i++) { 
		_buf.putLong(gainedCidList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)24);
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
	builder.Append("boxStatus").Append(":").Append(boxStatus.ToString()).Append(", ");
	builder.Append("gainedCidList").Append(":").Append(gainedCidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

