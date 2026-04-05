using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

/// <summary>
/// 探索数据变化
/// </summary>
public class GS2GC_041_050_OnExploreChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 探索数据
/// </summary>
private Common.MarsObj.Mars_Explore explore;


public GS2GC_041_050_OnExploreChg() {
	explore = new Common.MarsObj.Mars_Explore();
}

public GS2GC_041_050_OnExploreChg(
	Common.MarsObj.Mars_Explore _explore
) {	explore = _explore;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 探索数据
/// </summary>
public Common.MarsObj.Mars_Explore getExplore() { return explore; }
/// <summary>
/// 探索数据
/// </summary>
public void setExplore(Common.MarsObj.Mars_Explore _explore) { explore = _explore; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _exploreCustLen = _buf.getInt();
	int _exploreCurPos = _buf.getCurPos();
	explore.ReadUnzipBuf(_buf, _exploreCurPos + _exploreCustLen);
	_buf.setPosition(_exploreCurPos + _exploreCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(explore.GetBufSize());
	explore.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
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
	builder.Append("explore").Append(":").Append(explore == null ? "null" : explore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

