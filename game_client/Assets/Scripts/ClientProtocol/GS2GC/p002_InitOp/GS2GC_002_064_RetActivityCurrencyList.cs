using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_064_RetActivityCurrencyList : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.Common_ActivityCurrencyInfo> currencyList;


public GS2GC_002_064_RetActivityCurrencyList() {
	currencyList = new List<Common.Common_ActivityCurrencyInfo>();
}

public GS2GC_002_064_RetActivityCurrencyList(
	List<Common.Common_ActivityCurrencyInfo> _currencyList
) {	currencyList = _currencyList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)64; }

public List<Common.Common_ActivityCurrencyInfo> getCurrencyList() { return currencyList; }
public void addCurrencyList(Common.Common_ActivityCurrencyInfo _currencyList) { currencyList.Add(_currencyList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (currencyList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (currencyList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _currencyListCount = _buf.getShort();
	for(int _i = 0; _i < _currencyListCount; _i++) { 
		Common.Common_ActivityCurrencyInfo _currencyList = new Common.Common_ActivityCurrencyInfo();
		int __currencyListCustLen = _buf.getInt();
	int __currencyListCurPos = _buf.getCurPos();
	_currencyList.ReadUnzipBuf(_buf, __currencyListCurPos + __currencyListCustLen);
	_buf.setPosition(__currencyListCurPos + __currencyListCustLen);

		currencyList.Add(_currencyList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)currencyList.Count);
	for(int _i = 0; _i < currencyList.Count; _i++) { 
		_buf.putInt(currencyList[_i].GetBufSize());
	currencyList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)64);
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
	builder.Append("currencyList").Append(":").Append(currencyList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

