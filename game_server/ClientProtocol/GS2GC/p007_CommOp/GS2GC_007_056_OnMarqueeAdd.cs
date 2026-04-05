using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_056_OnMarqueeAdd : ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
private Common.Common_MarqueeInfo marqueeInfo;


public GS2GC_007_056_OnMarqueeAdd() {
	showPosId = 0;
	marqueeInfo = new Common.Common_MarqueeInfo();
}

public GS2GC_007_056_OnMarqueeAdd(
	int _showPosId
	, Common.Common_MarqueeInfo _marqueeInfo
) {	showPosId = _showPosId;
	marqueeInfo = _marqueeInfo;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)56; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
public Common.Common_MarqueeInfo getMarqueeInfo() { return marqueeInfo; }
public void setMarqueeInfo(Common.Common_MarqueeInfo _marqueeInfo) { marqueeInfo = _marqueeInfo; }


public int GetBufSize() {
	int _size = 4;
	_size += 4 + marqueeInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + marqueeInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _marqueeInfoCustLen = _buf.getInt();
	int _marqueeInfoCurPos = _buf.getCurPos();
	marqueeInfo.ReadUnzipBuf(_buf, _marqueeInfoCurPos + _marqueeInfoCustLen);
	_buf.setPosition(_marqueeInfoCurPos + _marqueeInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(showPosId);
	_buf.putInt(marqueeInfo.GetBufSize());
	marqueeInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)56);
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
	builder.Append("marqueeInfo").Append(":").Append(marqueeInfo == null ? "null" : marqueeInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

