using System;
namespace Bill
{
    public class GaoLingCDR
    {
        public byte[] LetterCode ;
        public byte[] ModuleNumber ;
        public byte[] CallCategory ;
        public byte[] CallerAreaCode ;
        public byte[] CalleeAreaCode ;
        public byte[] EnterDirectionSign ; // 入局局向号/路由号
        public byte[] BillingType ; //
        public byte[] FreeOCharge ;
        public byte[] StationOrNewServiceType ;
        public byte[] CallerAndCalleeAddress ;
        public byte[] CalleeNumber ;
        public byte[] CallerUserType ;
        public byte[] BillingSign ;
        public byte[] CallEndDate ;
        public byte[] OutTo ;
        public byte[] Duration ;
        public byte[] HangupReason ;
        public byte[] HangupTime ;
        public byte[] CallerNumer ;
        public byte[] CTXCommunityNo ;
        public byte[] CTXGroupNo ;

        public byte[] ISDNType ;
        public byte[] AlternateNumber ;
       
        public byte[] NewBusinesIdentity ;
        public byte[] UUS1 ;
        public byte[] UUS2 ;
        public byte[] BillingAddressNature ;
        public byte[] BillingNumber ;
        public byte[] CallerSpecialNumber ;
        public byte[] CalleeSpecialNumber ;
        public byte[] ConnectionNumberType ;

        public byte[] TakeLength ;
        public byte[] ConnectionNnumber ;
        public byte[] CIC ;
        public byte[] Other ;

        public GaoLingCDR()
        {
        }
    }
}
