using System;
namespace Bill
{
    public class ZXJX10CDR
    {
        public byte[] RecordType ; //记录类型   1
        public byte[] PartRecordID ; // Part record ID(部分记录标志) 1
        public byte[] NatureAddressOfCallerNumber ; // Nature address of caller number(主叫号码地址性质) 1
        public byte[] CallerNumber ; // Caller number(主叫号码) 20
        public byte[] NatureAddressOfCalleeNumber ; // Nature address of called number(被叫号码地址性质) 1
        public byte[] CalleeNumber ; // Called number(被叫号码) 20
        public byte[] StartTime ; //Start time(开始时间) 4 秒
        public byte[] StartTicks ; //Start ticks(开始时刻) 1 毫秒
        public byte[] ServiceCategory ; // Service category(业务类别) 1 
        public byte[] EndTime ; // End time(结束时间) 4 
        public byte[] EndTicks ; // End ticks(结束时刻) 1 
        public byte[] ReleaseReason ; // Release reason(终止原因) 1
        public byte[] CallerType ; // Caller type(主叫用户类型) 1
        public byte[] CallProperties ; // Record valid ID(记录用效性标志) 1/8低     取1位 里面包含数个属性 参考文档
        public byte[] IncomingTrunkGroup ; // Incoming trunk group(入中继群)  2
        public byte[] OutgoingTrunkGroup ; // Outgoing trunk group(出中继群) 2
        public byte[] SupplementServicee ; // Supplement servicee(补充业务) 7
        public byte[] ChargePartyID ; // Charge party ID(计费方标识) 1
        public byte[] NatureAddressOfLinkNumber ; // Nature address of link number(连接号码地址性质) 1
        public byte[] LinkNumber ; // Link number(连接号码) 20 BCD,左对齐，以0xF作为结束符
        public byte[] Fee ; // Fee(费用) 4

        public byte[] BearerServices ; // Bearer services(承载业务) 1
        public byte[] TerminalServices ; // Terminal services(终端业务) 1

        public byte[] UUS1 ; // UUS1 1
        public byte[] UUS3 ; // UUS3 1
        public byte[] CallerSpecialNumber ; // Caller special number(主叫专用号码) 5
        public byte[] CalleeSpecialNumber ; // Called special number(被叫专用号码) 5
        public byte[] CentrexGroupID ;  // Centrex group ID(CTX群标识) 2  
        public byte[] NatureAddressOfBilledNumber ; // Nature address of billed number(计费号码地址性质) 1
        public byte[] BilledNumber ; // Billed number(计费号码) 11
       

        public ZXJX10CDR()
        {
        }
    }
}
