using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_050_OnGuildShowInfoChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ShowInfo showInfo;


public GS2GC_032_050_OnGuildShowInfoChg() {
	showInfo = new Common.GuildObj.Guild_ShowInfo();
}

public GS2GC_032_050_OnGuildShowInfoChg(
	Common.GuildObj.Guild_ShowInfo _showInfo
) {	showInfo = _showInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)50; }

public Common.GuildObj.Guild_ShowInfo getShowInfo() { return showInfo; }
public void setShowInfo(Common.GuildObj.Guild_ShowInfo _showInfo) { showInfo = _showInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.getCurPos();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.setPosition(_showInfoCurPos + _showInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)50);
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
	builder.Append("showInfo").Append(":").Append(showInfo == null ? "null" : showInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

