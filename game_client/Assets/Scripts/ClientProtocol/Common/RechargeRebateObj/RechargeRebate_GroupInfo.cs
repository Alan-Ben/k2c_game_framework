using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.RechargeRebateObj
{

/// <summary>
/// 充值返利组信息
/// </summary>
public class RechargeRebate_GroupInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组ID
/// </summary>
private long groupId;
/// <summary>
/// 计数 VIP经验或充值天数
/// </summary>
private long count;
/// <summary>
/// 已领取档位ID列表
/// </summary>
private List<long> hadDrawStepList;


public RechargeRebate_GroupInfo() {
	groupId = (long)0;
	count = (long)0;
	hadDrawStepList = new List<long>();
}

public RechargeRebate_GroupInfo(
	long _groupId
	, long _count
	, List<long> _hadDrawStepList
) {	groupId = _groupId;
	count = _count;
	hadDrawStepList = _hadDrawStepList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 组ID
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 组ID
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 计数 VIP经验或充值天数
/// </summary>
public long getCount() { return count; }
/// <summary>
/// 计数 VIP经验或充值天数
/// </summary>
public void setCount(long _count) { count = _count; }
/// <summary>
/// 已领取档位ID列表
/// </summary>
public List<long> getHadDrawStepList() { return hadDrawStepList; }
/// <summary>
/// 已领取档位ID列表
/// </summary>
public void addHadDrawStepList(long _hadDrawStepList) { hadDrawStepList.Add(_hadDrawStepList); }


public int GetBufSize() {
	int _size = 16;
	_size += 2 + (hadDrawStepList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (hadDrawStepList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepListCount; _i++) { 
		long _hadDrawStepList = (long)0;
		_hadDrawStepList = _buf.getLong();
		hadDrawStepList.Add(_hadDrawStepList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putLong(count);
	_buf.putShort((short)hadDrawStepList.Count);
	for(int _i = 0; _i < hadDrawStepList.Count; _i++) { 
		_buf.putLong(hadDrawStepList[_i]);
	}
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
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("hadDrawStepList").Append(":").Append(hadDrawStepList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

