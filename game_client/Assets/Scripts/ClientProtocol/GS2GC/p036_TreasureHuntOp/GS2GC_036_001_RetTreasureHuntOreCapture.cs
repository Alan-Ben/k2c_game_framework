using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

public class GS2GC_036_001_RetTreasureHuntOreCapture : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 获得经验值
/// </summary>
private long addExp;
/// <summary>
/// 捕捉结果列表
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_CaptureResult> resultList;


public GS2GC_036_001_RetTreasureHuntOreCapture() {
	addExp = (long)0;
	resultList = new List<Common.TreasureHuntObj.TreasureHunt_CaptureResult>();
}

public GS2GC_036_001_RetTreasureHuntOreCapture(
	long _addExp
	, List<Common.TreasureHuntObj.TreasureHunt_CaptureResult> _resultList
) {	addExp = _addExp;
	resultList = _resultList;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 获得经验值
/// </summary>
public long getAddExp() { return addExp; }
/// <summary>
/// 获得经验值
/// </summary>
public void setAddExp(long _addExp) { addExp = _addExp; }
/// <summary>
/// 捕捉结果列表
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_CaptureResult> getResultList() { return resultList; }
/// <summary>
/// 捕捉结果列表
/// </summary>
public void addResultList(Common.TreasureHuntObj.TreasureHunt_CaptureResult _resultList) { resultList.Add(_resultList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2;
for(int _i = 0; _i < resultList.Count; _i++) {
	_size += 4 + resultList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
for(int _i = 0; _i < resultList.Count; _i++) {
	_size += 4 + resultList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _resultListCount = _buf.getShort();
	for(int _i = 0; _i < _resultListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_CaptureResult _resultList = new Common.TreasureHuntObj.TreasureHunt_CaptureResult();
		int __resultListCustLen = _buf.getInt();
	int __resultListCurPos = _buf.getCurPos();
	_resultList.ReadUnzipBuf(_buf, __resultListCurPos + __resultListCustLen);
	_buf.setPosition(__resultListCurPos + __resultListCustLen);

		resultList.Add(_resultList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(addExp);
	_buf.putShort((short)resultList.Count);
	for(int _i = 0; _i < resultList.Count; _i++) { 
		_buf.putInt(resultList[_i].GetBufSize());
	resultList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)1);
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
	builder.Append("addExp").Append(":").Append(addExp.ToString()).Append(", ");
	builder.Append("resultList").Append(":").Append(resultList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

