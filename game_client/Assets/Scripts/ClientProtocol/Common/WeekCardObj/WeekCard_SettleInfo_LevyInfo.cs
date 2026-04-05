using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-征收结算信息
/// </summary>
public class WeekCard_SettleInfo_LevyInfo : ALBasicProtocolPack._IALProtocolStructure {
private Common.LevyEnum.ELevy_Type type;
/// <summary>
/// 次数
/// </summary>
private int num;


public WeekCard_SettleInfo_LevyInfo() {
	type = 0;
	num = 0;
}

public WeekCard_SettleInfo_LevyInfo(
	Common.LevyEnum.ELevy_Type _type
	, int _num
) {	type = _type;
	num = _num;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public Common.LevyEnum.ELevy_Type getType() { return type; }
public void setType(Common.LevyEnum.ELevy_Type _type) { type = _type; }
/// <summary>
/// 次数
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 次数
/// </summary>
public void setNum(int _num) { num = _num; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.LevyEnum.ELevy_Type)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putInt(num);
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
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

