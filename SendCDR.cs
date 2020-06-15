using System;
using System.Collections.Generic;
using System.Text;

namespace Bill
{
    public class SendCDR
    {
        public string CALLING_NUMBER { get; set; }	//主叫号码
        public string CALLED_NUMBER { get; set; }	//	被叫号码
        public string CALLING_LORD { get; set; }	//主叫组号
        public string CALLED_LORD { get; set; }	//被叫组号
        public int INCOMING_BUREAU { get; set; }	//入局局向
        public int OUT_BUREAU { get; set; }	//出局局向
        public int ISFREE { get; set; }	//是否免费 0 免费 1 计费
        public string START_TIME { get; set; }	//开始时间
        public string END_TIME { get; set; }	//结束时间
        public long DURATION { get; set; }	//通话时长
        public int BILLING_LOGO { get; set; }	//计费方标识 0 主叫计费 1被叫计费
        public string BILLING_BUNBER { get; set; }	//计费号码
        public int TYPE { get; set; }	//类别 0  其他 1 市话  2 长途

        public SendCDR()
        {     
        }
    }
}
