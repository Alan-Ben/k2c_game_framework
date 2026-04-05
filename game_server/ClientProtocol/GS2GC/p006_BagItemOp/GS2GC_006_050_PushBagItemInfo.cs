using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p006_BagItemOp
{

public class GS2GC_006_050_PushBagItemInfo : ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_BagItemInfo bagItemInfo;


public GS2GC_006_050_PushBagItemInfo() {
	bagItemInfo = new NPCommon.NPCommon_BagItemInfo();
}

public GS2GC_006_050_PushBagItemInfo(
	NPCommon.NPCommon_BagItemInfo _bagItemInfo
) {	bagItemInfo = _bagItemInfo;
}

public byte getMainOrder() { return (byte)6; }

public byte getSubOrder() { return (byte)50; }

public NPCommon.NPCommon_BagItemInfo getBagItemInfo() { return bagItemInfo; }
public void setBagItemInfo(NPCommon.NPCommon_BagItemInfo _bagItemInfo) { bagItemInfo = _bagItemInfo; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _bagItemInfoCustLen = _buf.getInt();
	int _bagItemInfoCurPos = _buf.getCurPos();
	bagItemInfo.ReadUnzipBuf(_buf, _bagItemInfoCurPos + _bagItemInfoCustLen);
	_buf.setPosition(_bagItemInfoCurPos + _bagItemInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(bagItemInfo.GetBufSize());
	bagItemInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)50);
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
	builder.Append("bagItemInfo").Append(":").Append(bagItemInfo == null ? "null" : bagItemInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

