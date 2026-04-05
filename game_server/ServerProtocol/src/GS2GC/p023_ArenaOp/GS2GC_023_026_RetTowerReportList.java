package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_026_RetTowerReportList implements ALBasicProtocolPack._IALProtocolStructure {
/** 战报列表 */
private java.util.ArrayList<Common.TowerObj.Tower_ReportInfo> reportList;


public GS2GC_023_026_RetTowerReportList() {
	reportList = new java.util.ArrayList<Common.TowerObj.Tower_ReportInfo>();
}

public GS2GC_023_026_RetTowerReportList(
	 java.util.ArrayList<Common.TowerObj.Tower_ReportInfo> _reportList
) {	reportList = _reportList;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)26; }

/** 战报列表 */
public java.util.ArrayList<Common.TowerObj.Tower_ReportInfo> getReportList() { return reportList; }
/** 战报列表 */
public void addReportList(Common.TowerObj.Tower_ReportInfo _reportList) { reportList.add(_reportList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (reportList.size() * 41);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (reportList.size() * 41);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _reportListCount = _buf.getShort();
	for(int _i = 0; _i < _reportListCount; _i++) { 
		Common.TowerObj.Tower_ReportInfo _reportList = new Common.TowerObj.Tower_ReportInfo();
		if(_buf.remaining() <= 0) return;
	int __reportListCustLen = _buf.getInt();
	int __reportListCurPos = _buf.position();
	_reportList.ReadUnzipBuf(_buf, __reportListCurPos + __reportListCustLen);
	_buf.position(__reportListCurPos + __reportListCustLen);

		reportList.add(_reportList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)reportList.size());
	for(int _i = 0; _i < reportList.size(); _i++) { 
		_buf.putInt(reportList.get(_i).GetBufSize());
	reportList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)26);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

