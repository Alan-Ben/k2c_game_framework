using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 好友归属分组变更
/// </summary>
public class GS2GC_021_058_OnFriendBelongGroupChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标分组数据id
/// </summary>
private long targetGroupDbId;
/// <summary>
/// 变动玩家列表
/// </summary>
private List<long> cidList;


public GS2GC_021_058_OnFriendBelongGroupChg() {
	targetGroupDbId = (long)0;
	cidList = new List<long>();
}

public GS2GC_021_058_OnFriendBelongGroupChg(
	long _targetGroupDbId
	, List<long> _cidList
) {	targetGroupDbId = _targetGroupDbId;
	cidList = _cidList;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 目标分组数据id
/// </summary>
public long getTargetGroupDbId() { return targetGroupDbId; }
/// <summary>
/// 目标分组数据id
/// </summary>
public void setTargetGroupDbId(long _targetGroupDbId) { targetGroupDbId = _targetGroupDbId; }
/// <summary>
/// 变动玩家列表
/// </summary>
public List<long> getCidList() { return cidList; }
/// <summary>
/// 变动玩家列表
/// </summary>
public void addCidList(long _cidList) { cidList.Add(_cidList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (cidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (cidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetGroupDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		_cidList = _buf.getLong();
		cidList.Add(_cidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(targetGroupDbId);
	_buf.putShort((short)cidList.Count);
	for(int _i = 0; _i < cidList.Count; _i++) { 
		_buf.putLong(cidList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)58);
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
	builder.Append("targetGroupDbId").Append(":").Append(targetGroupDbId.ToString()).Append(", ");
	builder.Append("cidList").Append(":").Append(cidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

