using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_MarqueeShowPosInfo : ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
private List<Common.Common_MarqueeInfo> marqueeList;


public Common_MarqueeShowPosInfo() {
	showPosId = 0;
	marqueeList = new List<Common.Common_MarqueeInfo>();
}

public Common_MarqueeShowPosInfo(
	int _showPosId
	, List<Common.Common_MarqueeInfo> _marqueeList
) {	showPosId = _showPosId;
	marqueeList = _marqueeList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
public List<Common.Common_MarqueeInfo> getMarqueeList() { return marqueeList; }
public void addMarqueeList(Common.Common_MarqueeInfo _marqueeList) { marqueeList.Add(_marqueeList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2;
for(int _i = 0; _i < marqueeList.Count; _i++) {
	_size += 4 + marqueeList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
for(int _i = 0; _i < marqueeList.Count; _i++) {
	_size += 4 + marqueeList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _marqueeListCount = _buf.getShort();
	for(int _i = 0; _i < _marqueeListCount; _i++) { 
		Common.Common_MarqueeInfo _marqueeList = new Common.Common_MarqueeInfo();
		int __marqueeListCustLen = _buf.getInt();
	int __marqueeListCurPos = _buf.getCurPos();
	_marqueeList.ReadUnzipBuf(_buf, __marqueeListCurPos + __marqueeListCustLen);
	_buf.setPosition(__marqueeListCurPos + __marqueeListCustLen);

		marqueeList.Add(_marqueeList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(showPosId);
	_buf.putShort((short)marqueeList.Count);
	for(int _i = 0; _i < marqueeList.Count; _i++) { 
		_buf.putInt(marqueeList[_i].GetBufSize());
	marqueeList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("showPosId").Append(":").Append(showPosId.ToString()).Append(", ");
	builder.Append("marqueeList").Append(":").Append(marqueeList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

