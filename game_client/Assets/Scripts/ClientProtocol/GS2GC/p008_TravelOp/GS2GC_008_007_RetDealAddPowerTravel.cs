using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p008_TravelOp
{

/// <summary>
/// 处理大臣加国力游历事件
/// </summary>
public class GS2GC_008_007_RetDealAddPowerTravel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 游历事件结果
/// </summary>
private Common.TravelObj.Travel_EventResult result;


public GS2GC_008_007_RetDealAddPowerTravel() {
	result = new Common.TravelObj.Travel_EventResult();
}

public GS2GC_008_007_RetDealAddPowerTravel(
	Common.TravelObj.Travel_EventResult _result
) {	result = _result;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 游历事件结果
/// </summary>
public Common.TravelObj.Travel_EventResult getResult() { return result; }
/// <summary>
/// 游历事件结果
/// </summary>
public void setResult(Common.TravelObj.Travel_EventResult _result) { result = _result; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + result.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + result.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _resultCustLen = _buf.getInt();
	int _resultCurPos = _buf.getCurPos();
	result.ReadUnzipBuf(_buf, _resultCurPos + _resultCustLen);
	_buf.setPosition(_resultCurPos + _resultCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(result.GetBufSize());
	result.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)7);
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
	builder.Append("result").Append(":").Append(result == null ? "null" : result.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

