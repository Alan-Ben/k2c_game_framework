using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.LevyObj
{

/// <summary>
/// 征收信息
/// </summary>
public class Levy_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 征收类型
/// </summary>
private Common.LevyEnum.ELevy_Type levyType;
private byte[] levyInfo;
/// <summary>
/// 征收的数量累计总和
/// </summary>
private long levySum;


public Levy_Info() {
	levyType = 0;
	levyInfo = null;
	levySum = (long)0;
}

public Levy_Info(
	Common.LevyEnum.ELevy_Type _levyType
	, byte[] _levyInfo
	, long _levySum
) {	levyType = _levyType;
	levyInfo = _levyInfo;
	levySum = _levySum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 征收类型
/// </summary>
public Common.LevyEnum.ELevy_Type getLevyType() { return levyType; }
/// <summary>
/// 征收类型
/// </summary>
public void setLevyType(Common.LevyEnum.ELevy_Type _levyType) { levyType = _levyType; }
public byte[] getLevyInfo() { return levyInfo; }

public void setLevyInfo(byte[] _levyInfo) { levyInfo = _levyInfo; }

/// <summary>
/// 征收的数量累计总和
/// </summary>
public long getLevySum() { return levySum; }
/// <summary>
/// 征收的数量累计总和
/// </summary>
public void setLevySum(long _levySum) { levySum = _levySum; }


public int GetBufSize() {
	int _size = 12;
	_size += 4 + (levyInfo == null ? 0 : levyInfo.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (levyInfo == null ? 0 : levyInfo.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	levyType = (Common.LevyEnum.ELevy_Type)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	levyInfo = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	levySum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)levyType);

	_buf.putByteBuffer(levyInfo);

	_buf.putLong(levySum);
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
	builder.Append("levyType").Append(":").Append(levyType.ToString()).Append(", ");
	builder.Append("levyInfo").Append(":").Append(levyInfo == null ? "null" : levyInfo.ToString()).Append(", ");
	builder.Append("levySum").Append(":").Append(levySum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

