using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 藏品新增推送
/// </summary>
public class GS2GC_013_065_OnEquipAdd : ALBasicProtocolPack._IALProtocolStructure {
private Common.HeroObj.Equip_Info equipInfo;


public GS2GC_013_065_OnEquipAdd() {
	equipInfo = new Common.HeroObj.Equip_Info();
}

public GS2GC_013_065_OnEquipAdd(
	Common.HeroObj.Equip_Info _equipInfo
) {	equipInfo = _equipInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)65; }

public Common.HeroObj.Equip_Info getEquipInfo() { return equipInfo; }
public void setEquipInfo(Common.HeroObj.Equip_Info _equipInfo) { equipInfo = _equipInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + equipInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + equipInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _equipInfoCustLen = _buf.getInt();
	int _equipInfoCurPos = _buf.getCurPos();
	equipInfo.ReadUnzipBuf(_buf, _equipInfoCurPos + _equipInfoCustLen);
	_buf.setPosition(_equipInfoCurPos + _equipInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(equipInfo.GetBufSize());
	equipInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)65);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)65);
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
	builder.Append("equipInfo").Append(":").Append(equipInfo == null ? "null" : equipInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

