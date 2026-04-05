using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p012_ActivityTeamOp
{

public class GS2GC_012_012_RetSelfActivityTeamApplyList : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.CrossTeamObj.CrossTeam_PlayerApplyInfo> applyList;


public GS2GC_012_012_RetSelfActivityTeamApplyList() {
	applyList = new List<Common.CrossTeamObj.CrossTeam_PlayerApplyInfo>();
}

public GS2GC_012_012_RetSelfActivityTeamApplyList(
	List<Common.CrossTeamObj.CrossTeam_PlayerApplyInfo> _applyList
) {	applyList = _applyList;
}

public byte getMainOrder() { return (byte)12; }

public byte getSubOrder() { return (byte)12; }

public List<Common.CrossTeamObj.CrossTeam_PlayerApplyInfo> getApplyList() { return applyList; }
public void addApplyList(Common.CrossTeamObj.CrossTeam_PlayerApplyInfo _applyList) { applyList.Add(_applyList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (applyList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (applyList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _applyListCount = _buf.getShort();
	for(int _i = 0; _i < _applyListCount; _i++) { 
		Common.CrossTeamObj.CrossTeam_PlayerApplyInfo _applyList = new Common.CrossTeamObj.CrossTeam_PlayerApplyInfo();
		int __applyListCustLen = _buf.getInt();
	int __applyListCurPos = _buf.getCurPos();
	_applyList.ReadUnzipBuf(_buf, __applyListCurPos + __applyListCustLen);
	_buf.setPosition(__applyListCurPos + __applyListCustLen);

		applyList.Add(_applyList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)applyList.Count);
	for(int _i = 0; _i < applyList.Count; _i++) { 
		_buf.putInt(applyList[_i].GetBufSize());
	applyList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)12);
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
	builder.Append("applyList").Append(":").Append(applyList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

