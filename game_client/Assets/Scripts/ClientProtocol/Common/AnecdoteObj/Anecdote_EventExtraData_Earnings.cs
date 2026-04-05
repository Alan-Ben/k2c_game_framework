using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.AnecdoteObj
{

/// <summary>
/// 政务事件额外数据_赚速
/// </summary>
public class Anecdote_EventExtraData_Earnings : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否已领取首次奖励
/// </summary>
private bool hadDrawFirstReward;
/// <summary>
/// 是否已领取最终奖励
/// </summary>
private bool hadDrawFinalReward;


public Anecdote_EventExtraData_Earnings() {
	hadDrawFirstReward = false;
	hadDrawFinalReward = false;
}

public Anecdote_EventExtraData_Earnings(
	bool _hadDrawFirstReward
	, bool _hadDrawFinalReward
) {	hadDrawFirstReward = _hadDrawFirstReward;
	hadDrawFinalReward = _hadDrawFinalReward;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 是否已领取首次奖励
/// </summary>
public bool getHadDrawFirstReward() { return hadDrawFirstReward; }
/// <summary>
/// 是否已领取首次奖励
/// </summary>
public void setHadDrawFirstReward(bool _hadDrawFirstReward) { hadDrawFirstReward = _hadDrawFirstReward; }
/// <summary>
/// 是否已领取最终奖励
/// </summary>
public bool getHadDrawFinalReward() { return hadDrawFinalReward; }
/// <summary>
/// 是否已领取最终奖励
/// </summary>
public void setHadDrawFinalReward(bool _hadDrawFinalReward) { hadDrawFinalReward = _hadDrawFinalReward; }


public int GetBufSize() {
	int _size = 2;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawFirstReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawFinalReward = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(hadDrawFirstReward?(byte)1:(byte)0);
	_buf.put(hadDrawFinalReward?(byte)1:(byte)0);
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
	builder.Append("hadDrawFirstReward").Append(":").Append(hadDrawFirstReward.ToString()).Append(", ");
	builder.Append("hadDrawFinalReward").Append(":").Append(hadDrawFinalReward.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

