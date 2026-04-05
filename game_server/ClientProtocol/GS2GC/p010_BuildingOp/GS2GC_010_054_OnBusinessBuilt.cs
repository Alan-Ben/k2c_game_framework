using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p010_BuildingOp
{

/// <summary>
/// 推送经营建筑建造
/// </summary>
public class GS2GC_010_054_OnBusinessBuilt : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 经营建筑建筑数据
/// </summary>
private Common.BuildingObj.Building_Business business;


public GS2GC_010_054_OnBusinessBuilt() {
	business = new Common.BuildingObj.Building_Business();
}

public GS2GC_010_054_OnBusinessBuilt(
	Common.BuildingObj.Building_Business _business
) {	business = _business;
}

public byte getMainOrder() { return (byte)10; }

public byte getSubOrder() { return (byte)54; }

/// <summary>
/// 经营建筑建筑数据
/// </summary>
public Common.BuildingObj.Building_Business getBusiness() { return business; }
/// <summary>
/// 经营建筑建筑数据
/// </summary>
public void setBusiness(Common.BuildingObj.Building_Business _business) { business = _business; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + business.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + business.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _businessCustLen = _buf.getInt();
	int _businessCurPos = _buf.getCurPos();
	business.ReadUnzipBuf(_buf, _businessCurPos + _businessCustLen);
	_buf.setPosition(_businessCurPos + _businessCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(business.GetBufSize());
	business.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)54);
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
	builder.Append("business").Append(":").Append(business == null ? "null" : business.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

