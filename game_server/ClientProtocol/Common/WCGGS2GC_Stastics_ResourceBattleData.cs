using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_Stastics_ResourceBattleData : ALBasicProtocolPack._IALProtocolStructure {
private long resAbout;
private List<long> dataList;
private float wasteData;


public WCGGS2GC_Stastics_ResourceBattleData() {
	resAbout = (long)0;
	dataList = new List<long>();
	wasteData = 0f;
}

public WCGGS2GC_Stastics_ResourceBattleData(
	long _resAbout
	, List<long> _dataList
	, float _wasteData
) {	resAbout = _resAbout;
	dataList = _dataList;
	wasteData = _wasteData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getResAbout() { return resAbout; }
public void setResAbout(long _resAbout) { resAbout = _resAbout; }
public List<long> getDataList() { return dataList; }
public void addDataList(long _dataList) { dataList.Add(_dataList); }
public float getWasteData() { return wasteData; }
public void setWasteData(float _wasteData) { wasteData = _wasteData; }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (dataList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (dataList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	resAbout = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		long _dataList = (long)0;
		_dataList = _buf.getLong();
		dataList.Add(_dataList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	wasteData = _buf.getFloat();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(resAbout);
	_buf.putShort((short)dataList.Count);
	for(int _i = 0; _i < dataList.Count; _i++) { 
		_buf.putLong(dataList[_i]);
	}
	_buf.putFloat(wasteData);
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
	builder.Append("resAbout").Append(":").Append(resAbout.ToString()).Append(", ");
	builder.Append("dataList").Append(":").Append(dataList.ToString()).Append(", ");
	builder.Append("wasteData").Append(":").Append(wasteData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

