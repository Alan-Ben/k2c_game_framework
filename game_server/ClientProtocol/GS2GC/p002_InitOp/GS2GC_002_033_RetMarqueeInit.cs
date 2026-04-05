using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 请求跑马灯初始化
/// </summary>
public class GS2GC_002_033_RetMarqueeInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 跑马灯展示位置信息列表
/// </summary>
private List<Common.Common_MarqueeShowPosInfo> marqueeList;


public GS2GC_002_033_RetMarqueeInit() {
	marqueeList = new List<Common.Common_MarqueeShowPosInfo>();
}

public GS2GC_002_033_RetMarqueeInit(
	List<Common.Common_MarqueeShowPosInfo> _marqueeList
) {	marqueeList = _marqueeList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)33; }

/// <summary>
/// 跑马灯展示位置信息列表
/// </summary>
public List<Common.Common_MarqueeShowPosInfo> getMarqueeList() { return marqueeList; }
/// <summary>
/// 跑马灯展示位置信息列表
/// </summary>
public void addMarqueeList(Common.Common_MarqueeShowPosInfo _marqueeList) { marqueeList.Add(_marqueeList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < marqueeList.Count; _i++) {
	_size += 4 + marqueeList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < marqueeList.Count; _i++) {
	_size += 4 + marqueeList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _marqueeListCount = _buf.getShort();
	for(int _i = 0; _i < _marqueeListCount; _i++) { 
		Common.Common_MarqueeShowPosInfo _marqueeList = new Common.Common_MarqueeShowPosInfo();
		int __marqueeListCustLen = _buf.getInt();
	int __marqueeListCurPos = _buf.getCurPos();
	_marqueeList.ReadUnzipBuf(_buf, __marqueeListCurPos + __marqueeListCustLen);
	_buf.setPosition(__marqueeListCurPos + __marqueeListCustLen);

		marqueeList.Add(_marqueeList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)marqueeList.Count);
	for(int _i = 0; _i < marqueeList.Count; _i++) { 
		_buf.putInt(marqueeList[_i].GetBufSize());
	marqueeList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)33);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)33);
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
	builder.Append("marqueeList").Append(":").Append(marqueeList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

