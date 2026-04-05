using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-设置信息
/// </summary>
public class WeekCard_SingleSettingInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 类型
/// </summary>
private CommonEnum.EWeekCardSettleType type;
/// <summary>
/// 额外设置
/// </summary>
private byte[] extraInfo;


public WeekCard_SingleSettingInfo() {
	type = 0;
	extraInfo = null;
}

public WeekCard_SingleSettingInfo(
	CommonEnum.EWeekCardSettleType _type
	, byte[] _extraInfo
) {	type = _type;
	extraInfo = _extraInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 类型
/// </summary>
public CommonEnum.EWeekCardSettleType getType() { return type; }
/// <summary>
/// 类型
/// </summary>
public void setType(CommonEnum.EWeekCardSettleType _type) { type = _type; }
/// <summary>
/// 额外设置
/// </summary>
public byte[] getExtraInfo() { return extraInfo; }

/// <summary>
/// 额外设置
/// </summary>
public void setExtraInfo(byte[] _extraInfo) { extraInfo = _extraInfo; }



public int GetBufSize() {
	int _size = 4;
	_size += 4 + (extraInfo == null ? 0 : extraInfo.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (extraInfo == null ? 0 : extraInfo.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (CommonEnum.EWeekCardSettleType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extraInfo = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putByteBuffer(extraInfo);

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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("extraInfo").Append(":").Append(extraInfo == null ? "null" : extraInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

