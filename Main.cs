using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using System.Collections.Generic;

using System.Diagnostics;
using Newtonsoft.Json;
using System.Text.RegularExpressions;



namespace Bill
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass
	{
		public static int Conver16To10(byte[] b)
		{
            string str =  BitConverter.ToString(b).Replace("-","");
			return Convert.ToInt32(str, 16);
		}

		// 左对齐右边补0,默认，也可以根据需要传入补位
		public static string Conver16To2Left(byte[] b, char c )
		{
			string str = Convert.ToString(Conver16To10(b), 2);
			return str.PadRight(8 - str.Length, c);
		}
		// 右对齐左边补0默认，也可以根据需要传入补位
		public static string Conver16To2Right(byte[] b, char c)
		{
			string str = Convert.ToString(Conver16To10(b), 2);
			return str.PadLeft(8 - str.Length, c);
		}


		// 右边补0,默认，也可以根据需要传入补位
		public static string Conver16To2Left2(byte b, char c)
		{
			string bstr = b.ToString("X2");
			//Console.WriteLine("test:" + bstr);
			string str = Convert.ToString(Convert.ToInt32(bstr, 16), 2);
			return str.PadRight(8, c);
		}

		// // 左边补0,默认，也可以根据需要传入补位
		public static string Conver16To2Right2(byte b, char c)
		{
			string bstr = b.ToString("X2");
			//Console.WriteLine("test:" + bstr);
			string str = Convert.ToString(Convert.ToInt32(bstr, 16), 2);
			return str.PadLeft(8, c);
		}



		private static byte ConvertBCD(byte b)//byte转换为BCD码
		{
			//高四位  
			byte b1 = (byte)(b / 10);
			//低四位  
			byte b2 = (byte)(b % 10);
			return (byte)((b1 << 4) | b2);
		}

		/// <summary>  
		/// 将BCD一字节数据转换到byte 十进制数据  
		/// </summary>  
		/// <param name="b" />字节数  
		/// <returns>返回转换后的BCD码</returns>  
		public static byte ConvertBCDToInt(byte b)
		{
			//高四位  
			byte b1 = (byte)((b >> 4) & 0xF);
			//低四位  
			byte b2 = (byte)(b & 0xF);

			return (byte)(b1 * 10 + b2);
		}

		public static int ConvertBCDToInt2(string s, int start)
		{
			int a1 = Convert.ToInt32(s.Substring(0, 1));
			int a2 = Convert.ToInt32(s.Substring(1, 1));
			int a3 = Convert.ToInt32(s.Substring(2, 1));
			int a4 = Convert.ToInt32(s.Substring(3, 1));

			int result = a1 * start + a2 * 4 + a3 * 2 + a4 * 1;
			return result;
		}


		public static string ConvertZXBCD(string s, int start)
		{
			if(s == "1010")
				return "0";
			if(s=="1111")
			{
			return "";
			}
			int a1 = Convert.ToInt32(s.Substring(0, 1));
			int a2 = Convert.ToInt32(s.Substring(1, 1));
			int a3 = Convert.ToInt32(s.Substring(2, 1));
			int a4 = Convert.ToInt32(s.Substring(3, 1));

			int result = a1 * start + a2 * 4 + a3 * 2 + a4 * 1;
			return result.ToString();
		}


		public static string ConverGLBCD(string s, int start)
		{
			if(s == "1010")
				return "0";
			if(s=="1111")
			{
				return "";
			}
			int a1 = Convert.ToInt32(s.Substring(0, 1));
			int a2 = Convert.ToInt32(s.Substring(1, 1));
			int a3 = Convert.ToInt32(s.Substring(2, 1));
			int a4 = Convert.ToInt32(s.Substring(3, 1));

			int result = a1 * start + a2 * 4 + a3 * 2 + a4 * 1;
			return result.ToString();
		}



        #region 关闭控制台 快速编辑模式、插入模式
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_INSERT_MODE = 0x0020;
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int hConsoleHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);

        public static void DisbleQuickEditMode()
        {
            IntPtr hStdin = GetStdHandle(STD_INPUT_HANDLE);
            uint mode;
            GetConsoleMode(hStdin, out mode);
            mode &= ~ENABLE_QUICK_EDIT_MODE;//移除快速编辑模式
            mode &= ~ENABLE_INSERT_MODE;      //移除插入模式
            SetConsoleMode(hStdin, mode);
        }
        #endregion

        #region 设置控制台标题 禁用关闭按钮
        const string VERSION = "话单处理服务程序v1.0";

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        static void DisbleClosebtn()
        {
            //线程睡眠，确保closebtn中能够正常FindWindow，否则有时会Find失败。。
            Thread.Sleep(100);
            IntPtr windowHandle = FindWindow(null, VERSION);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }
        protected static void CloseConsole(object sender, ConsoleCancelEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        /// <summary>
        /// 创建日志文件,每天一个
        /// </summary>
        /// <param name="logContent">日志内容 </param>
        public static void CreateInLog(string logContent)
        {
            string logDir = Directory.GetCurrentDirectory() + @"\logs\";
            string filePath = logDir.Trim();//路径：如 E:\
            filePath = filePath.EndsWith(@"\") ? filePath : (filePath + @"\");
            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);//判断该路径是否存在，不存在则创建该路径文件夹
            }

            DateTime dtNow = DateTime.Now;//获取当前日期
            string dateString = dtNow.Year.ToString();
            dateString += dtNow.Month.ToString().Length < 2 ? ("0" + dtNow.Month.ToString()) : (dtNow.Month.ToString());
            dateString += dtNow.Day.ToString().Length < 2 ? ("0" + dtNow.Day.ToString()) : (dtNow.Day.ToString());  //将日期格式转成yyyyMMdd的格式，如：20170921

            string filename = filePath + "Log_" + dateString + ".txt"; //创建文件名
            StreamWriter sw = null;

            if (File.Exists(filename))
            {
                sw = new StreamWriter(filename, true, System.Text.Encoding.GetEncoding("UTF-8"));
            }
            else
            {
                sw = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("UTF-8"));
            }

            StringBuilder dataRow = new StringBuilder("");
            dataRow.Append(DateTime.Now.ToString() + "    "+ logContent);
            sw.WriteLine(dataRow.ToString());//写入内容
            sw.Close();

            Console.WriteLine(dataRow.ToString());
        }


        #region 处理试图关闭程序
        //当用户点击按钮关闭关闭Console时，系统会发送次消息
         private const int CTRL_CLOSE_EVENT = 2;
        //Ctrl+C，系统会发送次消息
        private const int CTRL_C_EVENT = 0;
         //Ctrl+break，系统会发送次消息
        private const int CTRL_BREAK_EVENT = 1;
         //用户退出（注销），系统会发送次消息
         private const int CTRL_LOGOFF_EVENT = 5;
         //系统关闭，系统会发送次消息
        private const int CTRL_SHUTDOWN_EVENT = 6;
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            CreateInLog("程序被强制关闭:" + CtrlType.ToString()); //Ctrl+C关闭 
            switch (CtrlType)
            {
                case CTRL_C_EVENT:
                    Console.WriteLine("0工具被强制关闭"); //Ctrl+C关闭 
                    break;
                case CTRL_CLOSE_EVENT:
                    Console.WriteLine("2工具被强制关闭");//按控制台关闭按钮关闭 
                    break;
            }
            return false; 
        } 
        #endregion

        public static object JsonToObject(string jsonString, object obj)
        {
            return JsonConvert.DeserializeObject(jsonString, obj.GetType());
        }
        public static bool Post(string url, string jsonParam = "")
        {
			try
			{		
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				//string postData = "thing1=hello";
				//postData += "&thing2=world";
                //request.ContentType = "application/x-www-form-urlencoded";

                // json参数
                // string jsonParam = "{ phonenumber:\"18665885202\",pwd:\"tsp\"}";
                // request.ContentType = "application/json;charset=UTF-8";

                Byte[] data = Encoding.ASCII.GetBytes(jsonParam);

				request.Method = "POST";		
                request.ContentType = "application/json;charset=UTF-8";
				request.ContentLength = data.Length;
				request.Timeout = 60 * 1000;

				using (Stream stream = request.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}

				WebResponse response = (HttpWebResponse)request.GetResponse();

				string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                CreateInLog("PostReturnString:" + responseString);

                PostResponse postResponse = (PostResponse)JsonToObject(responseString, new PostResponse());

                if (postResponse.Code == 200)
                {
                    return true;
                }
                else
                {
                    CreateInLog("Post话单请求返回错误:" + postResponse.Message);
                    return false;
                }
			}
			catch (Exception e) 
			{
                CreateInLog("Post Exception: "+ e.ToString());
                return false;
			}
		}

        public static PostRetyrResponse GetRetryAndInterval(string url, string jsonParam = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                Byte[] data = Encoding.ASCII.GetBytes(jsonParam);

                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                request.ContentLength = data.Length;
                request.Timeout = 60 * 1000;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                WebResponse response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                CreateInLog("PostReturnString:" + responseString);

                PostRetyrResponse postResponse = (PostRetyrResponse)JsonToObject(responseString, new PostRetyrResponse());

                if (postResponse.Code == 200)
                {
                    return postResponse;
                }
                else
                {
                    CreateInLog("GetRetryAndInterval返回错误");
                    return null;
                }
            }
            catch (Exception e)
            {
                CreateInLog("Post Exception: " + e.ToString());
                return null;
            }
        }

        public static void RecordLock(DateTime doneTime, string fileName, string content)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.SetLength(0);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(doneTime.ToString("yyyy-MM-dd HH:mm:ss") + "," + content);
                sw.Close();
            }
            catch (Exception ex)
            {
                CreateInLog("记录LockFile:"+fileName+"时发生错误：" + ex.Message);
            }          
        }

        public static string GetLockContent(string fileName)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                return lines[0];
            }
            catch (Exception ex)
            {
                CreateInLog("获取LockFile:" + fileName + "时发生错误：" + ex.Message);
                return String.Empty ;
            }
        }
        private static int DateDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateEnd.ToShortDateString());
            TimeSpan sp = end.Subtract(start);
            return sp.Days;
        }
        private static string CorrectTelNumber(string telnumber)
        {
            string res = telnumber;
            if ((telnumber.Length >= 12 || telnumber.Length == 9) && telnumber[0] == '2')
            {
                res = telnumber.Substring(1);
            }
            else if (telnumber.Length == 12 && telnumber[11] == '0')
            {
                res = telnumber.Substring(0, 11);
            }
            return res;
        }

        private static int CorrectCallType(string calledNumber)
        {
            if (calledNumber.Length < 8)
            {
                return 0;
            }
            else if (calledNumber[0] == '0')
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        // 中兴JX10话单处理
        public static int HandleZXJx10(string url,string zxCdrFile, string tempCdrFile,string exchange, int startRecord = 1,bool isDebug=false)
        {
            int postCount = 0; //记录post的记录数
           

            //string zxj10BillFile = Directory.GetCurrentDirectory() + @"\test_zxj10.txt";
            if (File.Exists(tempCdrFile))
            {
                File.SetAttributes(tempCdrFile, FileAttributes.Normal);
                File.Delete(tempCdrFile);  
            }
            File.Copy(zxCdrFile, tempCdrFile);

            //Console.WriteLine("{0} copied to {1}", zxCdrFile, tempCdrFile);


            //FileStream fsZxj10 = new FileStream(zxj10BillFile, FileMode.Create, FileAccess.Write);
            //fsZxj10.SetLength(0);

            //StreamWriter swZxj10 = new StreamWriter(fsZxj10, System.Text.Encoding.UTF8);

            if (!File.Exists(tempCdrFile))
            {
                CreateInLog("话单文件不存在:" + tempCdrFile);
                //CreateInLog("请按任意键结束程序！");

                //Console.ReadLine();
                //Environment.Exit(0);
                return 0;
            }

            BinaryReader zxbr = new BinaryReader(new FileStream(tempCdrFile, FileMode.Open,FileAccess.Read));
            CreateInLog("开始解析中兴话单:" + zxCdrFile);
            CreateInLog("总记录数:" + (zxbr.BaseStream.Length / 123 - 1).ToString());
            CreateInLog("已经处理:" + (startRecord-1).ToString());
            DateTime ZXTime = new DateTime(1994, 1, 1);

            if (startRecord > 0)
            {
                zxbr.ReadBytes(startRecord * 123);// 过滤已经读取过的记录
            }
            

            List<object> sendCdrs = new List<object>(); // 记录收集到的记录数，当记录数=20时，发送一次post并归零继续处理

            while (zxbr.BaseStream.Position < zxbr.BaseStream.Length)
            {
                //Console.WriteLine("Remain Bytes:" + (zxbr.BaseStream.Length - zxbr.BaseStream.Position).ToString());
                ZXJX10CDR zxCdr = new ZXJX10CDR();
                SendCDR sendCdr = new SendCDR();

                zxCdr.RecordType = zxbr.ReadBytes(1);
                zxCdr.PartRecordID = zxbr.ReadBytes(1);
                zxCdr.NatureAddressOfCallerNumber = zxbr.ReadBytes(1);
                zxCdr.CallerNumber = zxbr.ReadBytes(20);
                zxCdr.NatureAddressOfCalleeNumber = zxbr.ReadBytes(1);
                zxCdr.CalleeNumber = zxbr.ReadBytes(20);
                zxCdr.StartTime = zxbr.ReadBytes(4);
                zxCdr.StartTicks = zxbr.ReadBytes(1);
                zxCdr.ServiceCategory = zxbr.ReadBytes(1);
                zxCdr.EndTime = zxbr.ReadBytes(4);
                zxCdr.EndTicks = zxbr.ReadBytes(1);
                zxCdr.ReleaseReason = zxbr.ReadBytes(1);
                zxCdr.CallerType = zxbr.ReadBytes(1);
                zxCdr.CallProperties = zxbr.ReadBytes(1);
                zxCdr.IncomingTrunkGroup = zxbr.ReadBytes(2);
                zxCdr.OutgoingTrunkGroup = zxbr.ReadBytes(2);
                zxCdr.SupplementServicee = zxbr.ReadBytes(7);
                zxCdr.ChargePartyID = zxbr.ReadBytes(1);
                zxCdr.NatureAddressOfLinkNumber = zxbr.ReadBytes(1);
                zxCdr.LinkNumber = zxbr.ReadBytes(20);
                zxCdr.Fee = zxbr.ReadBytes(4);
                zxCdr.BearerServices = zxbr.ReadBytes(1);
                zxCdr.TerminalServices = zxbr.ReadBytes(1);
                zxCdr.UUS1 = zxbr.ReadBytes(1);
                zxCdr.UUS3 = zxbr.ReadBytes(1);
                zxCdr.CallerSpecialNumber = zxbr.ReadBytes(5);
                zxCdr.CalleeSpecialNumber = zxbr.ReadBytes(5);
                zxCdr.CentrexGroupID = zxbr.ReadBytes(2);
                zxCdr.NatureAddressOfBilledNumber = zxbr.ReadBytes(1);
                zxCdr.BilledNumber = zxbr.ReadBytes(11);


                // = Convert.ToInt32("10", 16)
                //Console.WriteLine(BitConverter.ToString(zxCdr.IncomingTrunkGroup));
                //Console.WriteLine(BitConverter.ToString(zxCdr.OutgoingTrunkGroup));

                /**
                short r = 0;
                for (int i = zxCdr.OutgoingTrunkGroup.Length - 1; i >= 0; i--)
                {
                    r <<= 8;
                    r |= (short)(zxCdr.OutgoingTrunkGroup[i] & 0x00ff);
                }
                 * **/
              

               

                /**
                r = 0;
                for (int i = 0; i < zxCdr.OutgoingTrunkGroup.Length; i++)
                {
                    r <<= 8;
                    r |= (short)(zxCdr.OutgoingTrunkGroup[i] & 0x00ff);
                }
                Console.WriteLine(r.ToString());
                 * **/

                sendCdr.TYPE = Conver16To10(zxCdr.ServiceCategory);


                string incomingTrunkGroupStr = BitConverter.ToString(zxCdr.IncomingTrunkGroup).Split('-')[0];
                string outgoingTrunkGroupStr = BitConverter.ToString(zxCdr.OutgoingTrunkGroup).Split('-')[0];
                sendCdr.INCOMING_BUREAU = Convert.ToInt32(incomingTrunkGroupStr, 16);
                sendCdr.OUT_BUREAU = Convert.ToInt32(outgoingTrunkGroupStr, 16);


                string callProperties = Conver16To2Right2(zxCdr.CallProperties[0], '0');
                //Console.WriteLine("callProperties:" + callProperties);
                sendCdr.ISFREE = Convert.ToInt32(callProperties.Substring(5, 1));
                sendCdr.BILLING_LOGO = Convert.ToInt32(callProperties.Substring(3, 1));
                //sendCdr.BILLING_LOGO = 0;
                string startStr = BitConverter.ToString(zxCdr.StartTime);
                string[] startStrs = startStr.Split('-');
                string startStrRevert = startStrs[3] + startStrs[2] + startStrs[1] + startStrs[0];
                long startSec = Convert.ToInt64(startStrRevert, 16);
                DateTime newDate = ZXTime.AddSeconds(startSec);
                // 真尼玛坑  为什么要反转下字符串

                //swZxj10.Write("StartTime:" + newDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sendCdr.START_TIME = newDate.ToString("yyyy-MM-dd HH:mm:ss");


                string endStr = BitConverter.ToString(zxCdr.EndTime);
                string[] endStrs = endStr.Split('-');
                string endStrRevert = endStrs[3] + endStrs[2] + endStrs[1] + endStrs[0];
                long endSec = Convert.ToInt64(endStrRevert, 16);
                newDate = ZXTime.AddSeconds(endSec);

                sendCdr.END_TIME = newDate.ToString("yyyy-MM-dd HH:mm:ss");
                sendCdr.DURATION = endSec - startSec;

                string CallerNumber = "";
                for (int i = 0; i < zxCdr.CallerNumber.Length; i++)
                {
                    string str = Conver16To2Right2(zxCdr.CallerNumber[i], '0');
                    if (str == "11111111")
                    {
                        break;
                    }


                    CallerNumber += ConvertZXBCD(str.Substring(4, 4), 8);
                    CallerNumber += ConvertZXBCD(str.Substring(0, 4), 8);

                }
                
                sendCdr.CALLING_NUMBER = CallerNumber;

                string CalleeNumber = "";
                for (int i = 0; i < zxCdr.CalleeNumber.Length; i++)
                {
                    string str = Conver16To2Right2(zxCdr.CalleeNumber[i], '0');
                    if (str == "11111111")
                    {
                        break;
                    }

                    CalleeNumber += ConvertZXBCD(str.Substring(4, 4), 8);
                    CalleeNumber += ConvertZXBCD(str.Substring(0, 4), 8);

                }
                sendCdr.CALLED_NUMBER = CorrectTelNumber(CalleeNumber);

                //sendCdr.BILLING_BUNBER = sendCdr.BILLING_LOGO == 0 ? sendCdr.CALLING_NUMBER : sendCdr.CALLED_NUMBER;
                sendCdr.BILLING_BUNBER = sendCdr.CALLING_NUMBER;
                sendCdr.TYPE = CorrectCallType(sendCdr.CALLED_NUMBER);
               // sendCdr.ISFREE = sendCdr.TYPE > 0 ? 1 : 0;
                sendCdr.EXCHANGE = exchange;

                sendCdrs.Add(sendCdr);
                if (isDebug)
                {
                    CreateInLog(sendCdr.CALLING_NUMBER + " " + sendCdr.CALLED_NUMBER + " " + sendCdr.OUT_BUREAU + " " + sendCdr.ISFREE);
                }
                if (sendCdrs.Count >= 20)
                {
                    // TODO do post
                    ListsConvertToJson t = new ListsConvertToJson();
                    string json = t.ConvertJson(sendCdrs, "SendCDR");
                    if (Post(url, json))
                    {
                        Thread.Sleep(1 * 1000 * 2);
                        postCount += sendCdrs.Count;
                        CreateInLog("本次成功处理记录:" + postCount.ToString());
                        
                        sendCdrs = new List<object>();// 重新来过
                    }
                    else
                    {
                        sendCdrs = new List<object>();
                        break;
                    }
                    
                }

                if (isDebug && postCount >= 100)
                {
                    break;
                }
            }
        
            zxbr.Close();

            ListsConvertToJson t1 = new ListsConvertToJson();
            string jsonParams = t1.ConvertJson(sendCdrs, "SendCDR");
            if (Post(url, jsonParams))
            {
                postCount += sendCdrs.Count;
            }
            CreateInLog("中兴话单：" + zxCdrFile + "本次处理执行完毕!合计处理："+postCount.ToString()+"条！");
            return postCount;
        }
		
        
        // 高凌GL04话单处理
        public static int HandleGL04(string url, string glCdrFile, string tempCdrFile,string exchange, int startRecord = 1, bool isDebug=false)
        {

            int postCount = 0; //记录post的记录数
            
            //string glCdrFile = cdrdir + @"\" + todayBillFile;

            if (File.Exists(tempCdrFile))
            {
                File.SetAttributes(tempCdrFile, FileAttributes.Normal);
                File.Delete(tempCdrFile);
            }
            File.Copy(glCdrFile, tempCdrFile);
            //Console.WriteLine("{0} copied to {1}", glCdrFile, tempCdrFile);


            //string gl40BillFile = Directory.GetCurrentDirectory() + @"\test_ngl04.txt";
            //FileStream fsGl40 = new FileStream(gl40BillFile, FileMode.Create, FileAccess.Write);
            //fsGl40.SetLength(0);
            //StreamWriter swGl40 = new StreamWriter(fsGl40, System.Text.Encoding.UTF8);


           

            if (!File.Exists(glCdrFile))
            {
                CreateInLog("高凌04话单文件不存在:" + glCdrFile);
                return 0;
            }


            BinaryReader glbr = new BinaryReader(new FileStream(tempCdrFile, FileMode.Open,FileAccess.Read));
            CreateInLog("开始解析高凌NGL04话单:" + glCdrFile);
            CreateInLog("总记录数:" + (glbr.BaseStream.Length / 96).ToString());
            CreateInLog("已经处理:" + (startRecord - 1).ToString());

            if (startRecord > 1)
            {
                glbr.ReadBytes((startRecord-1) * 96);// 过滤已经读取过的记录
            }
            

            List<object> sendCdrs = new List<object>(); // 记录收集到的记录数，当记录数=20时，发送一次post并归零继续处理

            while (glbr.BaseStream.Position < glbr.BaseStream.Length)
            {

                // Console.WriteLine("Remain Bytes:"+ (br.BaseStream.Length - br.BaseStream.Position).ToString());

                GaoLingCDR glCdr = new GaoLingCDR();
                SendCDR sendCdr = new SendCDR();

                glCdr.LetterCode = glbr.ReadBytes(1);
                glCdr.ModuleNumber = glbr.ReadBytes(1);
                glCdr.CallCategory = glbr.ReadBytes(1);
                glCdr.CallerAreaCode = glbr.ReadBytes(1);
                glCdr.CalleeAreaCode = glbr.ReadBytes(1);
                glCdr.EnterDirectionSign = glbr.ReadBytes(1); // 入局局向号/路由号
                glCdr.BillingType = glbr.ReadBytes(1); //
                glCdr.FreeOCharge = glbr.ReadBytes(1);
                glCdr.StationOrNewServiceType = glbr.ReadBytes(1);
                glCdr.CallerAndCalleeAddress = glbr.ReadBytes(1);
                glCdr.CalleeNumber = glbr.ReadBytes(12);
                glCdr.CallerUserType = glbr.ReadBytes(1);
                glCdr.BillingSign = glbr.ReadBytes(1);
                glCdr.CallEndDate = glbr.ReadBytes(4);
                glCdr.OutTo = glbr.ReadBytes(1);
                glCdr.Duration = glbr.ReadBytes(3);
                glCdr.HangupReason = glbr.ReadBytes(1);
                glCdr.HangupTime = glbr.ReadBytes(3);
                glCdr.CallerNumer = glbr.ReadBytes(10);
                glCdr.CTXCommunityNo = glbr.ReadBytes(1);
                glCdr.CTXGroupNo = glbr.ReadBytes(1);
                glCdr.ISDNType = glbr.ReadBytes(1);
                glCdr.AlternateNumber = glbr.ReadBytes(1);

                glCdr.NewBusinesIdentity = glbr.ReadBytes(7);
                glCdr.UUS1 = glbr.ReadBytes(1);
                glCdr.UUS2 = glbr.ReadBytes(1);
                glCdr.BillingAddressNature = glbr.ReadBytes(1);
                glCdr.BillingNumber = glbr.ReadBytes(10);
                glCdr.CallerSpecialNumber = glbr.ReadBytes(4);
                glCdr.CalleeSpecialNumber = glbr.ReadBytes(4);
                glCdr.ConnectionNumberType = glbr.ReadBytes(1);

                glCdr.TakeLength = glbr.ReadBytes(3);
                glCdr.ConnectionNnumber = glbr.ReadBytes(8);
                glCdr.CIC = glbr.ReadBytes(3);
                glCdr.Other = glbr.ReadBytes(3); //文档记录有错误，应该是93-95



                //Convert.ToInt32("28de1212", 16);
                //Console.WriteLine(Convert.ToInt32(BitConverter.ToString(glCdr.CallCategory); 16));

                string callEndStr = BitConverter.ToString(glCdr.CallEndDate);
                string[] callEndStrs = callEndStr.Split('-');
                callEndStr = callEndStrs[0] + callEndStrs[1] + "-" + callEndStrs[2] + "-" + callEndStrs[3];

                string hangupTimeStr = BitConverter.ToString(glCdr.HangupTime);
                string[] hangupTimeStrs = hangupTimeStr.Split('-');
                hangupTimeStr = hangupTimeStrs[0].PadLeft(2, '0') + ":" + hangupTimeStrs[1].PadLeft(2, '0') + ":" + Convert.ToInt32(hangupTimeStrs[2], 16).ToString().PadLeft(2, '0');



                DateTime ngl04HangupTime = Convert.ToDateTime(callEndStr + " " + hangupTimeStr);


                //int durationIndex = 0;
                //int[] durationMeight = new int[3] { 1000000, 1000, 1 };
                int duration = Conver16To10(glCdr.Duration);

                //Console.WriteLine("durationduration:" + Conver16To10(glCdr.Duration).ToString());
                //foreach (byte b in glCdr.Duration)
                //{
                    // TODO
                    // 需要确认这个是取3个的和还是？
                    //string bstr = b.ToString("X2");
                    //int bint = Convert.ToInt32(bstr, 16);
                    //duration += durationMeight[durationIndex] * bint;
                    //durationIndex++;


               // }
                sendCdr.START_TIME = ngl04HangupTime.AddSeconds(0 - duration).ToString("yyyy-MM-dd HH:mm:ss");
                sendCdr.END_TIME = ngl04HangupTime.ToString("yyyy-MM-dd HH:mm:ss");
                sendCdr.DURATION = duration;

                //swGl40.Write(" StartTime:" + ngl04HangupTime.AddSeconds(0 - duration).ToString("yyyy-MM-dd HH:mm:ss"));
                //swGl40.Write(" HangupTime:" + ngl04HangupTime.ToString("yyyy-MM-dd HH:mm:ss"));

                //swGl40.Write(" Duration:");
                //swGl40.Write(duration.ToString());

                //swGl40.Write(" CallerNumer Original:");
                //swGl40.Write(BitConverter.ToString(glCdr.CallerNumer));


               // swGl40.Write(" CallerNumer:");

                string CallerNumber = "";
                for (int i = 0; i < glCdr.CallerNumer.Length; i++)
                {
                    string str = Conver16To2Right2(glCdr.CallerNumer[i], '0');
                    if (str == "00000000")
                    {
                        break;
                    }

                    CallerNumber += ConverGLBCD(str.Substring(0, 4), 8);
                    CallerNumber += ConverGLBCD(str.Substring(4, 4), 8);

                }
                //swGl40.Write(CallerNumber);
               
                sendCdr.CALLING_NUMBER = CallerNumber;
                



                //swGl40.Write(" CalleeNumber Original:");
                //swGl40.Write(BitConverter.ToString(glCdr.CalleeNumber));

                //swGl40.Write(" CalleeNumber:");

                string CalleeNumber = "";
                for (int i = 0; i < glCdr.CalleeNumber.Length; i++)
                {
                    string str = Conver16To2Right2(glCdr.CalleeNumber[i], '0');
                    if (str == "00000000")
                    {
                        break;
                    }

                    CalleeNumber += ConverGLBCD(str.Substring(0, 4), 8);
                    CalleeNumber += ConverGLBCD(str.Substring(4, 4), 8);

                }
                //swGl40.Write(CalleeNumber);
                //swGl40.WriteLine();
                sendCdr.CALLED_NUMBER = CorrectTelNumber(CalleeNumber);
                sendCdr.BILLING_LOGO = 0;

                sendCdr.BILLING_BUNBER = sendCdr.CALLING_NUMBER;
                sendCdr.TYPE = CorrectCallType(sendCdr.CALLED_NUMBER);
                sendCdr.ISFREE = sendCdr.TYPE > 0 ? 1 : 0;
                sendCdr.EXCHANGE = exchange;




                // 下面是测试会用到的日志输出 不要删除

                //Console.WriteLine("LetterCode:" + Conver16To10(glCdr.LetterCode).ToString());
                //Console.WriteLine("ModuleNumber:" + Conver16To10(glCdr.ModuleNumber).ToString());
                //Console.WriteLine("CallCategory:" + Conver16To10(glCdr.CallCategory).ToString());
                //sendCdr.TYPE = Conver16To10(glCdr.CallCategory);
                //Console.WriteLine("CallerAreaCode:" + Conver16To10(glCdr.CallerAreaCode).ToString());
                //Console.WriteLine("CallerAreaCode:" + Conver16To10(glCdr.CallerAreaCode).ToString());
                //Console.WriteLine("CalleeAreaCode:" + Conver16To10(glCdr.CalleeAreaCode).ToString());
                //Console.WriteLine("EnterDirectionSign:" + Conver16To10(glCdr.EnterDirectionSign).ToString());
                sendCdr.INCOMING_BUREAU = Conver16To10(glCdr.EnterDirectionSign);
                // Console.WriteLine("BillingType:" + Conver16To10(glCdr.BillingType).ToString());
                //Console.WriteLine("FreeOCharge:" + Conver16To10(glCdr.FreeOCharge).ToString());
                //sendCdr.ISFREE = Conver16To10(glCdr.FreeOCharge) == 255 ? 0 : 1;
                //Console.WriteLine("StationOrNewServiceType:" + Conver16To10(glCdr.StationOrNewServiceType).ToString());
                //Console.WriteLine("CallerAndCalleeAddress:" + Conver16To10(glCdr.CallerAndCalleeAddress).ToString());


                //Console.Write("CalleeNumber 2421:");

                //var CalleeNumber = "";
                //for (var i = 0; i < 4; i++)
                //{
                //    var str = Conver16To2Left2(glCdr.CalleeNumber[i]);

                //    CalleeNumber += ConvertBCDToInt2(str.Substring(0, 4); 2).ToString();
                //    CalleeNumber += ConvertBCDToInt2(str.Substring(3, 4); 2).ToString();

                //}
                //Console.WriteLine(CalleeNumber);

                //Console.WriteLine();

               // Console.WriteLine("CallerUserType:" + Conver16To10(glCdr.CallerUserType).ToString());

               // Console.WriteLine("BillingSign:" + Conver16To10(glCdr.BillingSign).ToString());

                //Console.WriteLine("CallEndDate:" + BitConverter.ToString(glCdr.CallEndDate));

                //Console.WriteLine("OutTo:" + Conver16To10(glCdr.OutTo).ToString());
                sendCdr.OUT_BUREAU = Conver16To10(glCdr.OutTo);
               // Console.WriteLine("Duration:" + BitConverter.ToString(glCdr.Duration));
                //Console.Write("Duration:");
                //foreach (byte b in glCdr.Duration)
                //{
                //    // 需要确认这个是取3个的和还是？
                //    var bstr = b.ToString("X2");

                //    Console.Write(Convert.ToInt32(bstr, 16).ToString() + " ");
                //}
                //Console.WriteLine();


                //Console.WriteLine("HangupReason:" + Conver16To10(glCdr.HangupReason).ToString()); //需要开处理 暂时不管 TODO
                //Console.WriteLine("HangupTime:" + BitConverter.ToString(glCdr.HangupTime));


                //Console.Write("CallerNumer 8421:");

                //var CallerNumber = "";
                //for (var i = 0; i < 4; i++)
                //{
                //    var str = Conver16To2Left2(glCdr.CallerNumer[i]);

                //    CallerNumber += ConvertBCDToInt2(str.Substring(0, 4); 8).ToString();
                //    CallerNumber += ConvertBCDToInt2(str.Substring(3, 4); 8).ToString();

                //}
                //Console.WriteLine(CallerNumber);


                //Console.Write("CallerNumer 5421:");

                //CallerNumber = "";
                //for (var i = 0; i < 4; i++)
                //{
                //    var str = Conver16To2Left2(glCdr.CallerNumer[i]);

                //    CallerNumber += ConvertBCDToInt2(str.Substring(0, 4); 5).ToString();
                //    CallerNumber += ConvertBCDToInt2(str.Substring(3, 4); 5).ToString();

                //}
                //Console.WriteLine(CallerNumber);


                //Console.Write("CallerNumer 2421:");

                //CallerNumber = "";
                //for (var i = 0; i < 4; i++)
                //{
                //    var str = Conver16To2Left2(glCdr.CallerNumer[i]);

                //    CallerNumber += ConvertBCDToInt2(str.Substring(0, 4); 2).ToString();
                //    CallerNumber += ConvertBCDToInt2(str.Substring(3, 4); 2).ToString();

                //}
                //Console.WriteLine(CallerNumber);


                //Console.WriteLine("CTXCommunityNo:" + Conver16To10(glCdr.CTXCommunityNo).ToString());

                //Console.WriteLine("CTXGroupNo:" + Conver16To10(glCdr.CTXGroupNo).ToString());

                //Console.WriteLine("ISDNType:" + Conver16To10(glCdr.ISDNType).ToString());

                //Console.WriteLine("AlternateNumber:" + Conver16To10(glCdr.AlternateNumber).ToString());
                //Console.WriteLine("NewBusinesIdentity:" + BitConverter.ToString(glCdr.ISDNType));
                //Console.WriteLine("UUS1:" + Conver16To10(glCdr.UUS1).ToString());
                //Console.WriteLine("UUS2:" + Conver16To10(glCdr.UUS2).ToString());

                //Console.WriteLine("BillingAddressNature:" + Conver16To10(glCdr.BillingAddressNature).ToString());



                //Console.Write("BillingNumber 8421:");

                //var BillingNumber = "";
                //for (var i = 0; i < 4; i++)
                //{
                //    var str = Conver16To2Left2(glCdr.BillingNumber[i]);

                //    BillingNumber += ConvertBCDToInt2(str.Substring(0, 4); 8).ToString();
                //    BillingNumber += ConvertBCDToInt2(str.Substring(3, 4); 8).ToString();

                //}
                //Console.WriteLine(BillingNumber);


                //Console.Write("BillingNumber 5421:");

                //BillingNumber = "";
                //for (var i = 0; i < 4; i++)
                //{
                //    var str = Conver16To2Left2(glCdr.BillingNumber[i]);

                //    BillingNumber += ConvertBCDToInt2(str.Substring(0, 4); 5).ToString();
                //    BillingNumber += ConvertBCDToInt2(str.Substring(3, 4); 5).ToString();

                //}
                //Console.WriteLine(BillingNumber);


                //Console.Write("BillingNumber 2421:");

                //BillingNumber = "";
                //for (var i = 0; i < 4; i++)
                //{
                //    var str = Conver16To2Left2(glCdr.BillingNumber[i]);

                //    BillingNumber += ConvertBCDToInt2(str.Substring(0, 4); 2).ToString();
                //    BillingNumber += ConvertBCDToInt2(str.Substring(3, 4); 2).ToString();

                //}
                //Console.WriteLine(BillingNumber);

                //Console.WriteLine("CallerSpecialNumber:" + BitConverter.ToString(glCdr.CallerSpecialNumber));
                //Console.WriteLine("CalleeSpecialNumber:" + BitConverter.ToString(glCdr.CalleeSpecialNumber));
                //Console.WriteLine("ConnectionNumberType:" + Conver16To10(glCdr.ConnectionNumberType).ToString());

                //Console.Write("TakeLength:");
                //foreach (byte b in glCdr.TakeLength)
                //{
                //    // 需要确认这个是取3个的和还是？
                //    var bstr = b.ToString("X2");

                //    Console.Write(Convert.ToInt32(bstr, 16).ToString() + " ");
                //}
                //Console.WriteLine();

                //Console.WriteLine("ConnectionNnumber:" + BitConverter.ToString(glCdr.ConnectionNnumber));

                //Console.WriteLine("CIC:" + BitConverter.ToString(glCdr.CIC));

                //Console.WriteLine("Other:" + BitConverter.ToString(glCdr.Other));

                sendCdrs.Add(sendCdr);
                if (isDebug)
                {
                    CreateInLog(sendCdr.CALLING_NUMBER + " " + sendCdr.CALLED_NUMBER + " " + sendCdr.TYPE + " " + sendCdr.ISFREE);
                }
                if (sendCdrs.Count >= 20)
                {
                    // TODO do post
                    ListsConvertToJson t = new ListsConvertToJson();
                    string json = t.ConvertJson(sendCdrs, "SendCDR");
                    if (Post(url, json))
                    {
                        Thread.Sleep(1 * 1000 * 2);
                        postCount += sendCdrs.Count;
                        CreateInLog("本次成功处理记录:" + postCount.ToString());
                       
                        sendCdrs = new List<object>();// 重新来过
                    }
                    else
                    {
                        sendCdrs = new List<object>();
                        break;
                    }

                }

                if (isDebug && postCount >= 100)
                {
                    break;
                }
            }
            
            //swGl40.Close();
            glbr.Close();

            ListsConvertToJson t1 = new ListsConvertToJson();
            string jsonParams = t1.ConvertJson(sendCdrs, "SendCDR");
            if (Post(url, jsonParams))
            {
                postCount += sendCdrs.Count;
            }
            CreateInLog("高凌话单：" + glCdrFile + "本次处理执行完毕!合计处理：" + postCount.ToString() + "条！");
            return postCount;
        }

        public static void TryReconnectDrive(string remotePath, string localpath,string userName,string password)
        {
            int disconnetRes = NetworkConnection.Disconnect(localpath);
            CreateInLog("Try Disconnect Driver: " + localpath + "    " + disconnetRes.ToString());
            int status = NetworkConnection.Connect(remotePath, localpath, userName, password);
            if (status == (int)ERROR_ID.ERROR_SUCCESS)
            {
                CreateInLog("Reconnect Driver: " + localpath + " Success!");
            }
            else
            {
                CreateInLog("Reconnect Driver: " + localpath + " Fail! Status:"+status.ToString());
            }
        }
        /// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{

            bool isRuned;
            Mutex mutex = new Mutex(true, "OnlyRunOneInstance", out isRuned);
            if (!isRuned)
            {
                CreateInLog("服务程序已启动，请不要重复启动！！！");
                Console.WriteLine("请按任意键退出本次启动！");
                Console.ReadLine();
                Environment.Exit(0); 
            }

            bool bRet = SetConsoleCtrlHandler(cancelHandler, true);

            if (bRet == false) //安装事件处理失败
            {
                CreateInLog("安装事件处理失败,程序依然继续运行....");
            }

            Console.Title = VERSION;
            DisbleQuickEditMode();
            DisbleClosebtn();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CloseConsole);

			string configPath = Directory.GetCurrentDirectory() + @"\config.ini";    // 需要引用System.IO;
			string tempCdrFile = Directory.GetCurrentDirectory() + @"\_temp_cdr"; // 临时cdr
            string zxLockFile = Directory.GetCurrentDirectory() + @"\_zxj10_lock"; // 记录中兴zxj10当前处理进度
            string glLockFile = Directory.GetCurrentDirectory() + @"\_gl04_lock"; //  记录高凌04当前处理进度


			IniFile config = new IniFile(configPath);
			//string test = config.IniReadValue("db","host");

            string apiUri = config.IniReadValue("general", "apiUri");
            string retryConfUri = config.IniReadValue("general", "retryConfUri");
			string pbxtype = config.IniReadValue("general","pbxtype");
            bool debugMode = config.IniReadValue("general", "debugmode") == "true"?true:false;
            int intervalBetween = int.Parse(config.IniReadValue("general", "intervalBetween"));
            string EXCHANGE = config.IniReadValue("general", "exchange");

            // 加载中兴继续0话单运行时数据
            string[] jx10cdrdir = config.IniReadValue("general", "jx10cdrdir").Split(';');
            int[] jx10PostCounts = new int[jx10cdrdir.Length];
            string lastLockContent = GetLockContent(zxLockFile);
            string[] lastLockContents = lastLockContent.Split(',');
            DateTime lastZxPostTime = lastLockContents.Length > 0 && !string.IsNullOrEmpty(lastLockContents[0]) ? Convert.ToDateTime(lastLockContents[0]) : DateTime.Now;
            for (int lock_m = 0; lock_m < jx10PostCounts.Length; lock_m++)
            {
                if (lock_m < lastLockContents.Length && lastLockContents.Length > 1)
                {
                    jx10PostCounts[lock_m] = Convert.ToInt32(lastLockContents[lock_m + 1]);
                }
            }



            // 加载高凌04话单运行时数据
            string[] gl04cdrdir = config.IniReadValue("general", "gl04cdrdir").Split(';');
            int[] gl04PostCounts = new int[gl04cdrdir.Length];
            string lastGl04LockContent = GetLockContent(glLockFile);
            string[] lastGl04LockContents = lastGl04LockContent.Split(',');
            DateTime lastGLPostTime = lastGl04LockContents.Length > 0 && !string.IsNullOrEmpty(lastGl04LockContents[0]) ? Convert.ToDateTime(lastGl04LockContents[0]) : DateTime.Now;
            for (int lock_m = 0; lock_m < gl04PostCounts.Length; lock_m++)
            {
                if (lock_m < lastGl04LockContents.Length && lastGl04LockContents.Length > 1)
                {
                    gl04PostCounts[lock_m] = Convert.ToInt32(lastGl04LockContents[lock_m + 1]);
                }
            }

            string[] lastProcessZXFileName = new string[jx10cdrdir.Length];
            string[] lastProcessGLFileName = new string[gl04cdrdir.Length];

            #region 话单处理逻辑
            CreateInLog("话单分析处理服务，启动成功！");
            
			while(true){
                try {
                    var now = DateTime.Now;
                    var iszxi10 = false;
                    var isgl04 = false;
                    #region 中兴话单读取
                    int compZxPostTime = DateDiff(lastZxPostTime, now);
                    if (compZxPostTime > 0)
                    {
                        CreateInLog("中兴话单处理发现尚有积压未处理的天数:" + compZxPostTime.ToString());
                    }
                    DateTime doneZxDateTime = compZxPostTime > 0 ? lastZxPostTime : now;
                    string todayZXBillFile = "JF" + doneZxDateTime.Year.ToString() + doneZxDateTime.Month.ToString().PadLeft(2, '0') + ".B" + doneZxDateTime.Day.ToString().PadLeft(2, '0');

                    for (var i = 0; i < jx10cdrdir.Length; i++)
                    {
                        if (jx10cdrdir[i] != "" && jx10cdrdir[i] != null)
                        {
                            CreateInLog("开始中兴话单分析！话单位置:" + jx10cdrdir[i]);
                            bool isNextDay = false;


                            string zxCdrFile = jx10cdrdir[i] + @"\" + todayZXBillFile;
                            // 文件不存在尝试重连一次网络驱动器
                        
                            if (!File.Exists(zxCdrFile))
                            {
                                string remote = config.IniReadValue("general", "jx10dirremote"+i.ToString());
                                string username = config.IniReadValue("general", "jx10dirusername" + i.ToString());
                                string passwd = config.IniReadValue("general", "jx10dirpassword" + i.ToString());
                                TryReconnectDrive(remote, jx10cdrdir[i], username, passwd);
                            }
                             
                            // 日期变更前最后处理一次前次文件
                            if (lastProcessZXFileName[i] != null && todayZXBillFile != lastProcessZXFileName[i] && File.Exists(zxCdrFile))
                            {
                                zxCdrFile = jx10cdrdir[i] + @"\" + lastProcessZXFileName[i];
                                isNextDay = true;
                            }

                            int doneRecords = HandleZXJx10(apiUri,zxCdrFile, tempCdrFile,EXCHANGE, jx10PostCounts[i] + 1,debugMode);

                            if (isNextDay || compZxPostTime > 0)
                            {
                                jx10PostCounts[i] = 0;
                                lastProcessZXFileName[i] = null;// 这个没必要
                                CreateInLog("=====中兴话单全部分析完毕：" + zxCdrFile+"=====");
                            }
                            else
                            {
                                jx10PostCounts[i] = jx10PostCounts[i] + doneRecords;
                                lastProcessZXFileName[i] = todayZXBillFile;// 程序内部记住上一次处理文件名
                            }

                            string lockContent = string.Empty;
                            for (int j = 0; j < jx10PostCounts.Length; j++)
                            {
                                lockContent += jx10PostCounts[j];
                                if (j < jx10PostCounts.Length - 1)
                                    lockContent += ",";
                            }
                            RecordLock(doneZxDateTime,zxLockFile, lockContent);

                            #region retry month cdr
                            var retyrData = GetRetryAndInterval(retryConfUri);
                            if (retyrData.Code == 200)
                            {
                                if (retyrData.Year > 1999 && retyrData.Month > 1 && retyrData.Month < 13)
                                {
                                    for (var day = 1; day < 32; day++)
                                    {
                                        string retryBillFile = "JF" + retyrData.Year.ToString() + retyrData.Month.ToString().PadLeft(2, '0') + ".B" + day.ToString().PadLeft(2, '0');
                                        string retryCdrFile = jx10cdrdir[i] + @"\" + retryBillFile;

                                        if (File.Exists(retryCdrFile))
                                        {
                                            HandleZXJx10(apiUri, retryCdrFile, tempCdrFile,EXCHANGE, 1, debugMode);
                                        }
                                    }

                                }
                                if (retyrData.Interval > 5)
                                {
                                    intervalBetween = retyrData.Interval;
                                    config.IniWriteValue("general", "intervalBetween", intervalBetween.ToString());
                                }
                            }
                            #endregion

                        }
                    }

                    if (compZxPostTime > 0)
                    {
                        lastZxPostTime = lastZxPostTime.AddDays(1);
                    }

                    #endregion

                    #region 高凌话单读取
                    int compGLPostTime = DateDiff(lastGLPostTime, now);
                    if (compGLPostTime > 0)
                    {
                        CreateInLog("高凌话单处理发现尚有积压未处理的天数:" + compGLPostTime.ToString());
                    }

                    DateTime doneGLDateTime = compGLPostTime > 0 ? lastGLPostTime : now;
                    string todayGLBillFile = doneGLDateTime.Year.ToString() + doneGLDateTime.Month.ToString().PadLeft(2, '0') + doneGLDateTime.Day.ToString().PadLeft(2, '0') + ".cdr";

                   

                    for (var i = 0; i < gl04cdrdir.Length; i++)
                    {
                        if (gl04cdrdir[i] != "" && gl04cdrdir[i] != null)
                        {
                            CreateInLog("开始高凌话单分析！话单位置:" + gl04cdrdir[i]);
                        
                            bool isNextDay = false;
                            string glCdrFile = gl04cdrdir[i] + @"\" + todayGLBillFile;
                            // 文件不存在尝试重连一次网络驱动器
                            //if (!File.Exists(glCdrFile))
                            //{
                            //    TryReconnectDrive(@"\\10.211.55.5\mssql", "T:", "Administrator", "123456");
                            //}
                            // 日期变更前最后处理一次前次文件
                            if (lastProcessGLFileName[i] != null && todayGLBillFile != lastProcessGLFileName[i] && File.Exists(glCdrFile))
                            {
                                glCdrFile = gl04cdrdir[i] + @"\" + lastProcessGLFileName[i];
                                isNextDay = true;
                            }

                            int doneRecords = HandleGL04(apiUri, glCdrFile, tempCdrFile,EXCHANGE, gl04PostCounts[i] + 1,debugMode);

                            if (isNextDay || compGLPostTime > 0)
                            {
                                gl04PostCounts[i] = 0;
                                lastProcessGLFileName[i] = null;// 这个没必要
                                CreateInLog("=====高凌话单:" + glCdrFile +"全部处理完毕=====");
                            }
                            else
                            {
                                gl04PostCounts[i] = gl04PostCounts[i] + doneRecords;
                                lastProcessGLFileName[i] = todayGLBillFile;// 程序内部记住上一次处理文件名
                                CreateInLog("=====高凌话单:" + glCdrFile + "已经处理"+gl04PostCounts[i].ToString()+"条=====");
                            }

                            string lockContent = string.Empty;
                            for (int j = 0; j < gl04PostCounts.Length; j++)
                            {
                                lockContent += gl04PostCounts[j];
                                if (j < gl04PostCounts.Length - 1)
                                    lockContent += ",";
                            }
                            RecordLock(doneGLDateTime, glLockFile, lockContent);

                            #region retry month cdr

                            var retyrData = GetRetryAndInterval(retryConfUri);
                            if (retyrData.Code == 200)
                            {
                                if (retyrData.Year > 1999 && retyrData.Month > 1 && retyrData.Month < 13)
                                {
                                    for (var day = 1; day < 32; day++)
                                    {
                                        string retryBillFile = retyrData.Year.ToString() + retyrData.Month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0') + ".cdr"; ;
                                        string retryCdrFile = gl04cdrdir[i] + @"\" + retryBillFile;

                                        if (File.Exists(retryCdrFile))
                                        {
                                            HandleGL04(apiUri, retryCdrFile, tempCdrFile, EXCHANGE, 1, debugMode);
                                        }
                                    }

                                }
                                if (retyrData.Interval > 5)
                                {
                                    intervalBetween = retyrData.Interval;
                                    config.IniWriteValue("general", "intervalBetween", intervalBetween.ToString());
                                }
                            }
                            #endregion

                        }
                    }

                    if (compGLPostTime > 0)
                    {
                        lastGLPostTime = lastGLPostTime.AddDays(1);
                    }
                    #endregion

                    
                }
                catch (Exception ex)
                {
                    CreateInLog("处理异常："+ex.Message);
                }

                Thread.Sleep(intervalBetween * 1000 * 60);

            }
            #endregion
		}
	}
}
