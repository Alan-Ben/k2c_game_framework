using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_026_RetTowerReportList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 战报列表
/// </summary>
private List<Common.TowerObj.Tower_ReportInfo> reportList;


public GS2GC_023_026_RetTowerReportList() {
	reportList = new List<Common.TowerObj.Tower_ReportInfo>();
}

public GS2GC_023_026_RetTowerReportList(
	List<Common.TowerObj.Tower_ReportInfo> _reportList
) {	reportList = _reportList;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)26; }

/// <summary>
/// 战报列表
/// </summary>
public List<Common.TowerObj.Tower_ReportInfo> getReportList() { return reportList; }
/// <summary>
/// 战报列表
/// </summary>
public void addReportList(Common.TowerObj.Tower_ReportInfo _reportList) { reportList.Add(_reportList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (reportList.Count * 41);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (reportList.Count * 41);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _reportListCount = _buf.getShort();
	for(int _i = 0; _i < _reportListCount; _i++) { 
		Common.TowerObj.Tower_ReportInfo _reportList = new Common.TowerObj.Tower_ReportInfo();
		int __reportListCustLen = _buf.getInt();
	int __reportListCurPos = _buf.getCurPos();
	_reportList.ReadUnzipBuf(_buf, __reportListCurPos + __reportListCustLen);
	_buf.setPosition(__reportListCurPos + __reportListCustLen);

		reportList.Add(_reportList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)reportList.Count);
	for(int _i = 0; _i < reportList.Count; _i++) { 
		_buf.putInt(reportList[_i].GetBufSize());
	reportList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)26);
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
	builder.Append("reportList").Append(":").Append(reportList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

