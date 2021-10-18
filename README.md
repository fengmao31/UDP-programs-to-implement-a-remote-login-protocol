# UDP-programs-to-implement-a-remote-login-protocol
# Advanced Network Security课程
# RSA加密
# 写在前面的话
尽量不要用C#做这些稀奇古怪的事情。它做这些方面本身就少，你会遇见没有类库的尴尬之事，而且国内没有探索。
# 踩过的坑

## hash是结果16进制
其实C#安全密码库里面有相关函数，不熟悉也不会用。尽量复制 https://stackoverflow.com/ 函数，不要自己写，否则会产生非常多bug。（注明引用是个好习惯）

## C#没base的编码函数
C#没base的编码函数得变成byte转化

## 传输用utf8
传输用啥无所谓。无非把数据送过去。
注意utf8和base64无法相互转化，最后是乱码无法转换回来。

## rsa带入需要先编程base64编码
rsa加密读取需要使用base64方式读取到byte流明文输出密文，解密同样,明文密文也必须是base64编码的字符串。

## 网上生成的都是pem的格式密钥
C#生成的是xml格式的证书密钥，如果从pem构键xml格式的话，部分参数不知道。好在网上有在线转化网页。
C#无此官方函数，第三方的库年代未知。

##rsa函数必须填充
rsa函数必须填充才能加密解密，否则是乱码。

# 重要参考网站
RSAEncryptionPadding.OaepSHA1 Property

https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsaencryptionpadding.oaepsha1?view=net-5.0

.NET Encryption Simplified
https://www.codeproject.com/Articles/10154/NET-Encryption-Simplified

# 工具类网站
编码转换

https://www.asciitohex.com/

rsa加密解密

http://www.metools.info/code/c81.html

pem和xml格式密钥转换

https://superdry.apphb.com/tools/online-rsa-key-converter
# 使用方法
同时调试2个窗口server和client。

或者同时打开两个Virtual Stduio同时调试。
