using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p019_DinnerOp
{

/// <summary>
/// 宴会凭证推送
/// </summary>
public class GS2GC_019_052_OnPermitAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会凭证
/// </summary>
private Common.DinnerObj.Dinner_Permit permit;


public GS2GC_019_052_OnPermitAdd() {
	permit = new Common.DinnerObj.Dinner_Permit();
}

public GS2GC_019_052_OnPermitAdd(
	Common.DinnerObj.Dinner_Permit _permit
) {	permit = _permit;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 宴会凭证
/// </summary>
public Common.DinnerObj.Dinner_Permit getPermit() { return permit; }
/// <summary>
/// 宴会凭证
/// </summary>
public void setPermit(Common.DinnerObj.Dinner_Permit _permit) { permit = _permit; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _permitCustLen = _buf.getInt();
	int _permitCurPos = _buf.getCurPos();
	permit.ReadUnzipBuf(_buf, _permitCurPos + _permitCustLen);
	_buf.setPosition(_permitCurPos + _permitCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(permit.GetBufSize());
	permit.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)52);
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
	builder.Append("permit").Append(":").Append(permit == null ? "null" : permit.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

