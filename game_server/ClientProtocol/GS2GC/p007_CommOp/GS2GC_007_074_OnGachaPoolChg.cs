using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_074_OnGachaPoolChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池信息
/// </summary>
private Common.GachaObj.Gacha_PoolInfo poolInfo;


public GS2GC_007_074_OnGachaPoolChg() {
	poolInfo = new Common.GachaObj.Gacha_PoolInfo();
}

public GS2GC_007_074_OnGachaPoolChg(
	Common.GachaObj.Gacha_PoolInfo _poolInfo
) {	poolInfo = _poolInfo;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)74; }

/// <summary>
/// 卡池信息
/// </summary>
public Common.GachaObj.Gacha_PoolInfo getPoolInfo() { return poolInfo; }
/// <summary>
/// 卡池信息
/// </summary>
public void setPoolInfo(Common.GachaObj.Gacha_PoolInfo _poolInfo) { poolInfo = _poolInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + poolInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + poolInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _poolInfoCustLen = _buf.getInt();
	int _poolInfoCurPos = _buf.getCurPos();
	poolInfo.ReadUnzipBuf(_buf, _poolInfoCurPos + _poolInfoCustLen);
	_buf.setPosition(_poolInfoCurPos + _poolInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(poolInfo.GetBufSize());
	poolInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)74);
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
	builder.Append("poolInfo").Append(":").Append(poolInfo == null ? "null" : poolInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

