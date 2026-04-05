using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-已领取矿石记录档位变更
/// </summary>
public class GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// 已领取奖励档位列表
/// </summary>
private List<int> hadDrawRecordRewardList;


public GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg() {
	oreId = (long)0;
	hadDrawRecordRewardList = new List<int>();
}

public GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg(
	long _oreId
	, List<int> _hadDrawRecordRewardList
) {	oreId = _oreId;
	hadDrawRecordRewardList = _hadDrawRecordRewardList;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)59; }

/// <summary>
/// 矿石ID
/// </summary>
public long getOreId() { return oreId; }
/// <summary>
/// 矿石ID
/// </summary>
public void setOreId(long _oreId) { oreId = _oreId; }
/// <summary>
/// 已领取奖励档位列表
/// </summary>
public List<int> getHadDrawRecordRewardList() { return hadDrawRecordRewardList; }
/// <summary>
/// 已领取奖励档位列表
/// </summary>
public void addHadDrawRecordRewardList(int _hadDrawRecordRewardList) { hadDrawRecordRewardList.Add(_hadDrawRecordRewardList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (hadDrawRecordRewardList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (hadDrawRecordRewardList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawRecordRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRecordRewardListCount; _i++) { 
		int _hadDrawRecordRewardList = 0;
		_hadDrawRecordRewardList = _buf.getInt();
		hadDrawRecordRewardList.Add(_hadDrawRecordRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.putShort((short)hadDrawRecordRewardList.Count);
	for(int _i = 0; _i < hadDrawRecordRewardList.Count; _i++) { 
		_buf.putInt(hadDrawRecordRewardList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)59);
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
	builder.Append("oreId").Append(":").Append(oreId.ToString()).Append(", ");
	builder.Append("hadDrawRecordRewardList").Append(":").Append(hadDrawRecordRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

