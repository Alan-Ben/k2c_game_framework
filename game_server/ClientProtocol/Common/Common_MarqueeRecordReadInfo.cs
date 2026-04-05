using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_MarqueeRecordReadInfo : ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
private long recordDbId;
private List<Common.Common_MarqueeRecordPriorityReadInfo> priorityReadInfoList;


public Common_MarqueeRecordReadInfo() {
	showPosId = 0;
	recordDbId = (long)0;
	priorityReadInfoList = new List<Common.Common_MarqueeRecordPriorityReadInfo>();
}

public Common_MarqueeRecordReadInfo(
	int _showPosId
	, long _recordDbId
	, List<Common.Common_MarqueeRecordPriorityReadInfo> _priorityReadInfoList
) {	showPosId = _showPosId;
	recordDbId = _recordDbId;
	priorityReadInfoList = _priorityReadInfoList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
public long getRecordDbId() { return recordDbId; }
public void setRecordDbId(long _recordDbId) { recordDbId = _recordDbId; }
public List<Common.Common_MarqueeRecordPriorityReadInfo> getPriorityReadInfoList() { return priorityReadInfoList; }
public void addPriorityReadInfoList(Common.Common_MarqueeRecordPriorityReadInfo _priorityReadInfoList) { priorityReadInfoList.Add(_priorityReadInfoList); }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (priorityReadInfoList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (priorityReadInfoList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	recordDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _priorityReadInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _priorityReadInfoListCount; _i++) { 
		Common.Common_MarqueeRecordPriorityReadInfo _priorityReadInfoList = new Common.Common_MarqueeRecordPriorityReadInfo();
		int __priorityReadInfoListCustLen = _buf.getInt();
	int __priorityReadInfoListCurPos = _buf.getCurPos();
	_priorityReadInfoList.ReadUnzipBuf(_buf, __priorityReadInfoListCurPos + __priorityReadInfoListCustLen);
	_buf.setPosition(__priorityReadInfoListCurPos + __priorityReadInfoListCustLen);

		priorityReadInfoList.Add(_priorityReadInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(showPosId);
	_buf.putLong(recordDbId);
	_buf.putShort((short)priorityReadInfoList.Count);
	for(int _i = 0; _i < priorityReadInfoList.Count; _i++) { 
		_buf.putInt(priorityReadInfoList[_i].GetBufSize());
	priorityReadInfoList[_i].PutUnzipBuf(_buf);
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
	builder.Append("recordDbId").Append(":").Append(recordDbId.ToString()).Append(", ");
	builder.Append("priorityReadInfoList").Append(":").Append(priorityReadInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

