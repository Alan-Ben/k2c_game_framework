using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p011_ClientDataOp
{

public class GC2GS_011_003_ReqWebEncryptedData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 账号充值美金价值
/// </summary>
private float userPay;
/// <summary>
/// 用户语言
/// </summary>
private string langCode;
/// <summary>
/// 项目id
/// </summary>
private string project;
/// <summary>
/// 项目名称
/// </summary>
private string packageName;
/// <summary>
/// 商品列表
/// </summary>
private string products;


public GC2GS_011_003_ReqWebEncryptedData() {
	userPay = 0f;
	langCode = "";
	project = "";
	packageName = "";
	products = "";
}

public GC2GS_011_003_ReqWebEncryptedData(
	float _userPay
	, string _langCode
	, string _project
	, string _packageName
	, string _products
) {	userPay = _userPay;
	langCode = _langCode;
	project = _project;
	packageName = _packageName;
	products = _products;
}

public byte getMainOrder() { return (byte)11; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 账号充值美金价值
/// </summary>
public float getUserPay() { return userPay; }
/// <summary>
/// 账号充值美金价值
/// </summary>
public void setUserPay(float _userPay) { userPay = _userPay; }
/// <summary>
/// 用户语言
/// </summary>
public string getLangCode() { return langCode; }
/// <summary>
/// 用户语言
/// </summary>
public void setLangCode(string _langCode) { langCode = _langCode; }
/// <summary>
/// 项目id
/// </summary>
public string getProject() { return project; }
/// <summary>
/// 项目id
/// </summary>
public void setProject(string _project) { project = _project; }
/// <summary>
/// 项目名称
/// </summary>
public string getPackageName() { return packageName; }
/// <summary>
/// 项目名称
/// </summary>
public void setPackageName(string _packageName) { packageName = _packageName; }
/// <summary>
/// 商品列表
/// </summary>
public string getProducts() { return products; }
/// <summary>
/// 商品列表
/// </summary>
public void setProducts(string _products) { products = _products; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(langCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(project);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(packageName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(products);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(langCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(project);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(packageName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(products);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	userPay = _buf.getFloat();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	langCode = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	project = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	packageName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	products = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putFloat(userPay);
	_buf.putString(langCode);
	_buf.putString(project);
	_buf.putString(packageName);
	_buf.putString(products);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)11);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)11);
	_recBuf.put((byte)3);
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
	builder.Append("userPay").Append(":").Append(userPay.ToString()).Append(", ");
	builder.Append("langCode").Append(":").Append(langCode.ToString()).Append(", ");
	builder.Append("project").Append(":").Append(project.ToString()).Append(", ");
	builder.Append("packageName").Append(":").Append(packageName.ToString()).Append(", ");
	builder.Append("products").Append(":").Append(products.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

