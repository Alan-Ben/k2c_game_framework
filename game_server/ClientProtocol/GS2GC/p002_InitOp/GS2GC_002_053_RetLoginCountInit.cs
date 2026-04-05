using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_053_RetLoginCountInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已经领取的奖励天数
/// </summary>
private List<int> hadDrawRewardDays;


public GS2GC_002_053_RetLoginCountInit() {
	hadDrawRewardDays = new List<int>();
}

public GS2GC_002_053_RetLoginCountInit(
	List<int> _hadDrawRewardDays
) {	hadDrawRewardDays = _hadDrawRewardDays;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 已经领取的奖励天数
/// </summary>
public List<int> getHadDrawRewardDays() { return hadDrawRewardDays; }
/// <summary>
/// 已经领取的奖励天数
/// </summary>
public void addHadDrawRewardDays(int _hadDrawRewardDays) { hadDrawRewardDays.Add(_hadDrawRewardDays); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadDrawRewardDays.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadDrawRewardDays.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawRewardDaysCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRewardDaysCount; _i++) { 
		int _hadDrawRewardDays = 0;
		_hadDrawRewardDays = _buf.getInt();
		hadDrawRewardDays.Add(_hadDrawRewardDays);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)hadDrawRewardDays.Count);
	for(int _i = 0; _i < hadDrawRewardDays.Count; _i++) { 
		_buf.putInt(hadDrawRewardDays[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)53);
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
	builder.Append("hadDrawRewardDays").Append(":").Append(hadDrawRewardDays.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

