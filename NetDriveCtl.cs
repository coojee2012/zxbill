using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Bill
{
     //资源类，用于记录映射类型和设置
[StructLayout(LayoutKind.Sequential)]
public class NETRESOURCE1
 {
 public int dwScope;//只能取2
 public int dwType;//0为打印机或驱动器，1为驱动器，2为打印机
 public int dwDisplayType;//取0，自动设置
 public int dwUsage;//取1
 public string LocalName;//本地盘符或名称
 public string RemoteName;//远程地址
 public string Comment;//NULL即可，A pointer to a NULL-terminated string that contains a comment supplied by the network provider.
 public string Provider;//NULL即可，A pointer to a NULL-terminated string that contains the name of the provider that owns the resource. This member can be NULL if the provider name is unknown.
 public NETRESOURCE1()
 {
     this.dwScope = 2;
     this.dwType = 0;
     this.dwDisplayType = 0;
     this.dwUsage = 1;
 }
}

 //控制(主)类，创建、删除映射
 public class NetDriveCtl
 {
     ArrayList NDList;

     public NetDriveCtl()
     {
     NDList =new ArrayList();
     }

    public string CreateDrive(string LocalName, string RemoteName,string UserName,string Password)
    {
        NETRESOURCE1 NetDrive = new NETRESOURCE1();
        
        NetDrive.LocalName = LocalName;
        NetDrive.RemoteName = RemoteName;

        NDList.Add(NetDrive);
        return ConnectDrive(NetDrive, UserName, Password);
    }

     public Boolean DeleteDrive(string LocalName, string RemoteName)
     {
         foreach (NETRESOURCE1 NetDrive in NDList)
         {
             if ((NetDrive.LocalName == LocalName) && (NetDrive.RemoteName == RemoteName))
             {
                 DisconnectDrive(NetDrive);
                 NDList.Remove(NetDrive);
                 return true;
             }
         }
         return false;
     }

     private string ConnectDrive(NETRESOURCE1 NetDrive, string UserName, string Password)
     {
     StringBuilder UN =new StringBuilder(UserName);
     StringBuilder PW =new StringBuilder(Password);

     return WNetAddConnection2(NetDrive, PW, UN, 0).ToString();
     }

     private string DisconnectDrive(NETRESOURCE1 NetDrive)
     {
     string LocalName = NetDrive.LocalName;
     return WNetCancelConnection2(LocalName, 1, true).ToString();
     }

     private string DisconnectDrive(string LocalName)
     {
         return WNetCancelConnection2(LocalName, 1, true).ToString();
     }

     //这两个是系统API函数
     [DllImport("mpr.dll", EntryPoint ="WNetAddConnection2")]
     private static extern uint WNetAddConnection2([In] NETRESOURCE1 lpNetResource, StringBuilder lpPassword, StringBuilder lpUsername, uint dwFlags);
     [DllImport("Mpr.dll")]
     private static extern uint WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);
     }
}
