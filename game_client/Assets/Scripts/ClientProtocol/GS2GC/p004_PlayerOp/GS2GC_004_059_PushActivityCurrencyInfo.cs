using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_059_PushActivityCurrencyInfo : ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_ActivityCurrencyInfo currencyInfo;


public GS2GC_004_059_PushActivityCurrencyInfo() {
	currencyInfo = new Common.Common_ActivityCurrencyInfo();
}

public GS2GC_004_059_PushActivityCurrencyInfo(
	Common.Common_ActivityCurrencyInfo _currencyInfo
) {	currencyInfo = _currencyInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)59; }

public Common.Common_ActivityCurrencyInfo getCurrencyInfo() { return currencyInfo; }
public void setCurrencyInfo(Common.Common_ActivityCurrencyInfo _currencyInfo) { currencyInfo = _currencyInfo; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _currencyInfoCustLen = _buf.getInt();
	int _currencyInfoCurPos = _buf.getCurPos();
	currencyInfo.ReadUnzipBuf(_buf, _currencyInfoCurPos + _currencyInfoCustLen);
	_buf.setPosition(_currencyInfoCurPos + _currencyInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(currencyInfo.GetBufSize());
	currencyInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)59);
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
	builder.Append("currencyInfo").Append(":").Append(currencyInfo == null ? "null" : currencyInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

