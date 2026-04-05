using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 已领取奖励点列表变更推送
/// </summary>
public class GS2GC_032_078_OnHadDrawRewardPointListChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 信息
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList data;


public GS2GC_032_078_OnHadDrawRewardPointListChg() {
	data = new Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList();
}

public GS2GC_032_078_OnHadDrawRewardPointListChg(
	Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _data
) {	data = _data;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)78; }

/// <summary>
/// 信息
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList getData() { return data; }
/// <summary>
/// 信息
/// </summary>
public void setData(Common.GuildCooperateObj.GuildCooperate_HadDrawRewardPointList _data) { data = _data; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + data.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + data.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _dataCustLen = _buf.getInt();
	int _dataCurPos = _buf.getCurPos();
	data.ReadUnzipBuf(_buf, _dataCurPos + _dataCustLen);
	_buf.setPosition(_dataCurPos + _dataCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(data.GetBufSize());
	data.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)78);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)78);
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
	builder.Append("data").Append(":").Append(data == null ? "null" : data.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

