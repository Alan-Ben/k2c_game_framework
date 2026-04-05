using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.AchieveObj
{

/// <summary>
/// 成就数据
/// </summary>
public class Achieve_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成就ID
/// </summary>
private long achieveId;
/// <summary>
/// 成就步骤计数
/// </summary>
private long counter;
/// <summary>
/// 已领取成就步骤列表
/// </summary>
private List<int> hadDrawStepList;


public Achieve_Info() {
	achieveId = (long)0;
	counter = (long)0;
	hadDrawStepList = new List<int>();
}

public Achieve_Info(
	long _achieveId
	, long _counter
	, List<int> _hadDrawStepList
) {	achieveId = _achieveId;
	counter = _counter;
	hadDrawStepList = _hadDrawStepList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 成就ID
/// </summary>
public long getAchieveId() { return achieveId; }
/// <summary>
/// 成就ID
/// </summary>
public void setAchieveId(long _achieveId) { achieveId = _achieveId; }
/// <summary>
/// 成就步骤计数
/// </summary>
public long getCounter() { return counter; }
/// <summary>
/// 成就步骤计数
/// </summary>
public void setCounter(long _counter) { counter = _counter; }
/// <summary>
/// 已领取成就步骤列表
/// </summary>
public List<int> getHadDrawStepList() { return hadDrawStepList; }
/// <summary>
/// 已领取成就步骤列表
/// </summary>
public void addHadDrawStepList(int _hadDrawStepList) { hadDrawStepList.Add(_hadDrawStepList); }


public int GetBufSize() {
	int _size = 16;
	_size += 2 + (hadDrawStepList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (hadDrawStepList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	achieveId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	counter = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepListCount; _i++) { 
		int _hadDrawStepList = 0;
		_hadDrawStepList = _buf.getInt();
		hadDrawStepList.Add(_hadDrawStepList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(achieveId);
	_buf.putLong(counter);
	_buf.putShort((short)hadDrawStepList.Count);
	for(int _i = 0; _i < hadDrawStepList.Count; _i++) { 
		_buf.putInt(hadDrawStepList[_i]);
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
	builder.Append("achieveId").Append(":").Append(achieveId.ToString()).Append(", ");
	builder.Append("counter").Append(":").Append(counter.ToString()).Append(", ");
	builder.Append("hadDrawStepList").Append(":").Append(hadDrawStepList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

