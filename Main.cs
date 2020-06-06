using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using System.Collections.Generic;

using System.Diagnostics;


namespace Bill
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass
	{
		public static int Conver16To10(byte[] b)
		{
			return Convert.ToInt32(BitConverter.ToString(b), 16);
		}

		// ������ұ߲�0,Ĭ�ϣ�Ҳ���Ը�����Ҫ���벹λ
		public static string Conver16To2Left(byte[] b, char c )
		{
			string str = Convert.ToString(Conver16To10(b), 2);
			return str.PadRight(8 - str.Length, c);
		}
		// �Ҷ�����߲�0Ĭ�ϣ�Ҳ���Ը�����Ҫ���벹λ
		public static string Conver16To2Right(byte[] b, char c)
		{
			string str = Convert.ToString(Conver16To10(b), 2);
			return str.PadLeft(8 - str.Length, c);
		}


		// �ұ߲�0,Ĭ�ϣ�Ҳ���Ը�����Ҫ���벹λ
		public static string Conver16To2Left2(byte b, char c)
		{
			string bstr = b.ToString("X2");
			//Console.WriteLine("test:" + bstr);
			string str = Convert.ToString(Convert.ToInt32(bstr, 16), 2);
			return str.PadRight(8, c);
		}

		// // ��߲�0,Ĭ�ϣ�Ҳ���Ը�����Ҫ���벹λ
		public static string Conver16To2Right2(byte b, char c)
		{
			string bstr = b.ToString("X2");
			//Console.WriteLine("test:" + bstr);
			string str = Convert.ToString(Convert.ToInt32(bstr, 16), 2);
			return str.PadLeft(8, c);
		}



		private static byte ConvertBCD(byte b)//byteת��ΪBCD��
		{
			//����λ  
			byte b1 = (byte)(b / 10);
			//����λ  
			byte b2 = (byte)(b % 10);
			return (byte)((b1 << 4) | b2);
		}

		/// <summary>  
		/// ��BCDһ�ֽ�����ת����byte ʮ��������  
		/// </summary>  
		/// <param name="b" />�ֽ���  
		/// <returns>����ת�����BCD��</returns>  
		public static byte ConvertBCDToInt(byte b)
		{
			//����λ  
			byte b1 = (byte)((b >> 4) & 0xF);
			//����λ  
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



        #region �رտ���̨ ���ٱ༭ģʽ������ģʽ
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
            mode &= ~ENABLE_QUICK_EDIT_MODE;//�Ƴ����ٱ༭ģʽ
            mode &= ~ENABLE_INSERT_MODE;      //�Ƴ�����ģʽ
            SetConsoleMode(hStdin, mode);
        }
        #endregion

        #region ���ÿ���̨���� ���ùرհ�ť
        const string VERSION = "��������������v1.0";

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        static void DisbleClosebtn()
        {
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
        /// ������־�ļ�,ÿ��һ��
        /// </summary>
        /// <param name="logContent">��־���� </param>
        public static void CreateInLog(string logContent)
        {
            string logDir = Directory.GetCurrentDirectory() + @"\logs\";
            string filePath = logDir.Trim();//·������ E:\
            filePath = filePath.EndsWith(@"\") ? filePath : (filePath + @"\");
            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);//�жϸ�·���Ƿ���ڣ��������򴴽���·���ļ���
            }

            DateTime dtNow = DateTime.Now;//��ȡ��ǰ����
            string dateString = dtNow.Year.ToString();
            dateString += dtNow.Month.ToString().Length < 2 ? ("0" + dtNow.Month.ToString()) : (dtNow.Month.ToString());
            dateString += dtNow.Day.ToString().Length < 2 ? ("0" + dtNow.Day.ToString()) : (dtNow.Day.ToString());  //�����ڸ�ʽת��yyyyMMdd�ĸ�ʽ���磺20170921

            string filename = filePath + "Log_" + dateString + ".txt"; //�����ļ���
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
            sw.WriteLine(dataRow.ToString());//д������
            sw.Close();

            Console.WriteLine(dataRow.ToString());
        }


        #region ������ͼ�رճ���
        //���û������ť�رչر�Consoleʱ��ϵͳ�ᷢ�ʹ���Ϣ
         private const int CTRL_CLOSE_EVENT = 2;
        //Ctrl+C��ϵͳ�ᷢ�ʹ���Ϣ
        private const int CTRL_C_EVENT = 0;
         //Ctrl+break��ϵͳ�ᷢ�ʹ���Ϣ
        private const int CTRL_BREAK_EVENT = 1;
         //�û��˳���ע������ϵͳ�ᷢ�ʹ���Ϣ
         private const int CTRL_LOGOFF_EVENT = 5;
         //ϵͳ�رգ�ϵͳ�ᷢ�ʹ���Ϣ
        private const int CTRL_SHUTDOWN_EVENT = 6;
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            CreateInLog("����ǿ�ƹر�:" + CtrlType.ToString()); //Ctrl+C�ر� 
            switch (CtrlType)
            {
                case CTRL_C_EVENT:
                    Console.WriteLine("0���߱�ǿ�ƹر�"); //Ctrl+C�ر� 
                    break;
                case CTRL_CLOSE_EVENT:
                    Console.WriteLine("2���߱�ǿ�ƹر�");//������̨�رհ�ť�ر� 
                    break;
            }
            return false; 
        } 
        #endregion
        public static void Post(string url){
			try
			{		
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				string postData = "thing1=hello";
				postData += "&thing2=world";

				Byte[] data = Encoding.ASCII.GetBytes(postData);

				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = data.Length;
				request.Timeout = 60 * 1000;

				using (Stream stream = request.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}

				WebResponse response = (HttpWebResponse)request.GetResponse();

				string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
				Console.WriteLine("Post ResponseString:"+ responseString);
			}
			catch (Exception e) 
			{
				Console.WriteLine("Generic Exception Handler: {0}", e.ToString());
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
                CreateInLog("����������������벻Ҫ�ظ�����������");
                Console.WriteLine("�밴������˳�����������");
                Console.ReadLine();
                Environment.Exit(0); 
            }




            bool bRet = SetConsoleCtrlHandler(cancelHandler, true);

            if (bRet == false) //��װ�¼�����ʧ��
            {
                CreateInLog("��װ�¼�����ʧ��,������Ȼ��������....");
            }



            Console.Title = VERSION;
            DisbleQuickEditMode();
            DisbleClosebtn();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CloseConsole);

			string configPath = Directory.GetCurrentDirectory() + @"\config.ini";    // ��Ҫ����System.IO;
			string tempCdrFile = Directory.GetCurrentDirectory() + @"\_temp_cdr"; // ��ʱcdr

			IniFile config = new IniFile(configPath);
			//string test = config.IniReadValue("db","host");
			
			string pbxtype = config.IniReadValue("general","pbxtype");
			string cdrdir = config.IniReadValue("general","cdrdir");

            CreateInLog("��������������������ɹ���");

//			Post("http://vv.video.qq.com/checktime?otype=json");

			//
			// TODO: Add code to start application here
            //
            #region ���˻�����ȡ
            if (pbxtype == "zxj10")
			{
                CreateInLog("��ʼ���˻�������������λ��:" + cdrdir);
				while(true){
                    try {
                        var now = DateTime.Now;
                        string todayBillFile = "JF" + now.Year.ToString() + now.Month.ToString().PadLeft(2, '0') + ".B" + now.Day.ToString().PadLeft(2, '0');
                        string zxCdrFile = Directory.GetCurrentDirectory() + @"\" + todayBillFile;

                        string zxj10BillFile = Directory.GetCurrentDirectory() + @"\test_zxj10.txt";

                        File.Delete(tempCdrFile);
                        File.Copy(zxCdrFile, tempCdrFile);
                        Console.WriteLine("{0} copied to {1}", zxCdrFile, tempCdrFile);


                        FileStream fsZxj10 = new FileStream(zxj10BillFile, FileMode.Create, FileAccess.Write);
                        fsZxj10.SetLength(0);

                        StreamWriter swZxj10 = new StreamWriter(fsZxj10, System.Text.Encoding.UTF8);

                        if (!File.Exists(tempCdrFile))
                        {
                            CreateInLog("�����ļ�������:" + tempCdrFile);
                            CreateInLog("�밴�������������");

                            Console.ReadLine();
                            Environment.Exit(0);
                        }

                        BinaryReader zxbr = new BinaryReader(new FileStream(tempCdrFile, FileMode.Open));
                        CreateInLog("��ʼ�������˻���:" + zxCdrFile);
                        CreateInLog("�ܼ�¼��:" + (zxbr.BaseStream.Length / 123).ToString());
                        DateTime ZXTime = new DateTime(1994, 1, 1);

                        while (zxbr.BaseStream.Position < zxbr.BaseStream.Length)
                        {
                            //Console.WriteLine("Remain Bytes:" + (zxbr.BaseStream.Length - zxbr.BaseStream.Position).ToString());
                            ZXJX10CDR zxCdr = new ZXJX10CDR();

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
                            string startStr = BitConverter.ToString(zxCdr.StartTime);
                            string[] startStrs = startStr.Split('-');
                            string startStrRevert = startStrs[3] + startStrs[2] + startStrs[1] + startStrs[0];
                            long startSec = Convert.ToInt64(startStrRevert, 16);
                            DateTime newDate = ZXTime.AddSeconds(startSec);
                            // �������  ΪʲôҪ��ת���ַ���

                            swZxj10.Write("StartTime:" + newDate.ToString("yyyy-MM-dd HH:mm:ss"));


                            string endStr = BitConverter.ToString(zxCdr.EndTime);
                            string[] endStrs = endStr.Split('-');
                            string endStrRevert = endStrs[3] + endStrs[2] + endStrs[1] + endStrs[0];
                            long endSec = Convert.ToInt64(endStrRevert, 16);
                            newDate = ZXTime.AddSeconds(endSec);

                            swZxj10.Write(" EndTime:" + newDate.ToString("yyyy-MM-dd HH:mm:ss"));

                            swZxj10.Write(" Duration:" + (endSec - startSec).ToString());

                            // swZxj10.Write(" CallerNumer Original:");
                            // swZxj10.Write(BitConverter.ToString(zxCdr.CallerNumber));


                            swZxj10.Write(" CallerNumer:");

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
                            swZxj10.Write(CallerNumber);



                            // swZxj10.Write(" CalleeNumer Original:");
                            // swZxj10.Write(BitConverter.ToString(zxCdr.CalleeNumber));

                            swZxj10.Write(" CalleeNumber:");

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
                            swZxj10.Write(CalleeNumber);
                            swZxj10.WriteLine();


                        }
                        swZxj10.Close();
                        zxbr.Close();
                        CreateInLog("ִ����ϣ��ȴ���һ��.");
                    }
                    catch (Exception ex)
                    {
                        CreateInLog("�������"+ex.Message);
                    }
					
                    Thread.Sleep(1 * 1000 * 60);
				
				}


            }
            #endregion

            #region ���軰����ȡ
            else if(pbxtype == "gl04")
			{
				string gl40BillFile = Directory.GetCurrentDirectory() + @"\test_ngl04.txt";
						
				FileStream fsGl40 = new FileStream(gl40BillFile, FileMode.Create, FileAccess.Write);
				fsGl40.SetLength(0);
				StreamWriter swGl40 = new StreamWriter(fsGl40, System.Text.Encoding.UTF8);


				string glCdrFile = Directory.GetCurrentDirectory() +@"\" +config.IniReadValue("general","testglcdr");

				if (!File.Exists(glCdrFile))
				{
					Console.WriteLine("�����ļ�������:"+glCdrFile);
					Console.WriteLine("�밴�س�����������");
			
					Console.ReadLine();
					System.Environment.Exit(0);
				}


				BinaryReader br = new BinaryReader(new FileStream(glCdrFile, FileMode.Open));
				Console.WriteLine("��ʼ��������NGL04����:"+glCdrFile);
				Console.WriteLine("�ܼ�¼��:" + (br.BaseStream.Length / 96).ToString());

				while (br.BaseStream.Position < br.BaseStream.Length)
				{

					// Console.WriteLine("Remain Bytes:"+ (br.BaseStream.Length - br.BaseStream.Position).ToString());

					GaoLingCDR glCdr = new GaoLingCDR();
                    
					glCdr.LetterCode = br.ReadBytes(1);
					glCdr.ModuleNumber = br.ReadBytes(1);
					glCdr.CallCategory = br.ReadBytes(1);
					glCdr.CallerAreaCode = br.ReadBytes(1);
					glCdr.CalleeAreaCode = br.ReadBytes(1);
					glCdr.EnterDirectionSign = br.ReadBytes(1); // ��־����/·�ɺ�
					glCdr.BillingType = br.ReadBytes(1); //
					glCdr.FreeOCharge = br.ReadBytes(1);
					glCdr.StationOrNewServiceType = br.ReadBytes(1);
					glCdr.CallerAndCalleeAddress = br.ReadBytes(1);
					glCdr.CalleeNumber = br.ReadBytes(12);
					glCdr.CallerUserType = br.ReadBytes(1);
					glCdr.BillingSign = br.ReadBytes(1);
					glCdr.CallEndDate = br.ReadBytes(4);
					glCdr.OutTo = br.ReadBytes(1);
					glCdr.Duration = br.ReadBytes(3);
					glCdr.HangupReason = br.ReadBytes(1);
					glCdr.HangupTime = br.ReadBytes(3);
					glCdr.CallerNumer = br.ReadBytes(10);
					glCdr.CTXCommunityNo = br.ReadBytes(1);
					glCdr.CTXGroupNo = br.ReadBytes(1);
					glCdr.ISDNType = br.ReadBytes(1);
					glCdr.AlternateNumber = br.ReadBytes(1);

					glCdr.NewBusinesIdentity = br.ReadBytes(7);
					glCdr.UUS1 = br.ReadBytes(1);
					glCdr.UUS2 = br.ReadBytes(1);
					glCdr.BillingAddressNature = br.ReadBytes(1);
					glCdr.BillingNumber = br.ReadBytes(10);
					glCdr.CallerSpecialNumber = br.ReadBytes(4);
					glCdr.CalleeSpecialNumber = br.ReadBytes(4);
					glCdr.ConnectionNumberType = br.ReadBytes(1);

					glCdr.TakeLength = br.ReadBytes(3);
					glCdr.ConnectionNnumber = br.ReadBytes(8);
					glCdr.CIC = br.ReadBytes(3);
					glCdr.Other = br.ReadBytes(3); //�ĵ���¼�д���Ӧ����93-95

                    

					//Convert.ToInt32("28de1212", 16);
					//Console.WriteLine(Convert.ToInt32(BitConverter.ToString(glCdr.CallCategory); 16));

					string callEndStr = BitConverter.ToString(glCdr.CallEndDate);
					string[] callEndStrs = callEndStr.Split('-');
					callEndStr = callEndStrs[0]+callEndStrs[1]+"-"+callEndStrs[2]+"-"+callEndStrs[3];

					string hangupTimeStr = BitConverter.ToString(glCdr.HangupTime);
					string[] hangupTimeStrs = hangupTimeStr.Split('-');
					hangupTimeStr = hangupTimeStrs[0].PadLeft(2,'0')+":"+hangupTimeStrs[1].PadLeft(2,'0')+":"+ Convert.ToInt32(hangupTimeStrs[2], 16).ToString().PadLeft(2,'0');



					DateTime ngl04HangupTime = Convert.ToDateTime(callEndStr+" "+hangupTimeStr);
                    
					int durationIndex = 0;
					int[] durationMeight = new int[3]{1000000,1000,1};
					int duration = 0;

					foreach (byte b in glCdr.Duration)
					{
						// ��Ҫȷ�������ȡ3���ĺͻ��ǣ�
						string bstr = b.ToString("X2");
						int bint = Convert.ToInt32(bstr, 16);
						duration += durationMeight[durationIndex] * bint;
						durationIndex++;

                        
					}
					swGl40.Write(" StartTime:" +ngl04HangupTime.AddSeconds(0-duration).ToString("yyyy-MM-dd HH:mm:ss"));
					swGl40.Write(" HangupTime:"+ngl04HangupTime.ToString("yyyy-MM-dd HH:mm:ss"));

					swGl40.Write(" Duration:");
					swGl40.Write(duration.ToString());

					swGl40.Write(" CallerNumer Original:");
					swGl40.Write(BitConverter.ToString(glCdr.CallerNumer));


					swGl40.Write(" CallerNumer:");

					string CallerNumber = "";
					for (int i = 0; i < glCdr.CallerNumer.Length; i++)
					{
						string str = Conver16To2Right2(glCdr.CallerNumer[i],'0');
						if(str == "00000000")
						{
							break;
						}

						CallerNumber += ConverGLBCD(str.Substring(0, 4), 8);
						CallerNumber += ConverGLBCD(str.Substring(4, 4), 8);

					}
					swGl40.Write(CallerNumber);



					swGl40.Write(" CalleeNumber Original:");
					swGl40.Write(BitConverter.ToString(glCdr.CalleeNumber));

					swGl40.Write(" CalleeNumber:");

					string CalleeNumber = "";
					for (int i = 0; i < glCdr.CalleeNumber.Length; i++)
					{
						string str = Conver16To2Right2(glCdr.CalleeNumber[i],'0');
						if(str == "00000000")
						{
							break;
						}

						CalleeNumber += ConverGLBCD(str.Substring(0, 4), 8);
						CalleeNumber += ConverGLBCD(str.Substring(4, 4), 8);

					}
					swGl40.Write(CalleeNumber);
					swGl40.WriteLine();




					// �����ǲ��Ի��õ�����־��� ��Ҫɾ��

					//Console.WriteLine("LetterCode:" + Conver16To10(glCdr.LetterCode).ToString());
					//Console.WriteLine("ModuleNumber:" + Conver16To10(glCdr.ModuleNumber).ToString());
					//Console.WriteLine("CallCategory:" + Conver16To10(glCdr.CallCategory).ToString());
					//Console.WriteLine("CallerAreaCode:" + Conver16To10(glCdr.CallerAreaCode).ToString());
					//Console.WriteLine("CallerAreaCode:" + Conver16To10(glCdr.CallerAreaCode).ToString());
					//Console.WriteLine("CalleeAreaCode:" + Conver16To10(glCdr.CalleeAreaCode).ToString());
					//Console.WriteLine("EnterDirectionSign:" + Conver16To10(glCdr.EnterDirectionSign).ToString());
					//Console.WriteLine("BillingType:" + Conver16To10(glCdr.BillingType).ToString());
					//Console.WriteLine("FreeOCharge:" + Conver16To10(glCdr.FreeOCharge).ToString());
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

					//Console.WriteLine("CallerUserType:" + Conver16To10(glCdr.CallerUserType).ToString());

					//Console.WriteLine("BillingSign:" + Conver16To10(glCdr.BillingSign).ToString());

					//Console.WriteLine("CallEndDate:" + BitConverter.ToString(glCdr.CallEndDate));

					//Console.WriteLine("OutTo:" + Conver16To10(glCdr.OutTo).ToString());


					//Console.Write("Duration:");
					//foreach (byte b in glCdr.Duration)
					//{
					//    // ��Ҫȷ�������ȡ3���ĺͻ��ǣ�
					//    var bstr = b.ToString("X2");

					//    Console.Write(Convert.ToInt32(bstr, 16).ToString() + " ");
					//}
					//Console.WriteLine();


					//Console.WriteLine("HangupReason:" + Conver16To10(glCdr.HangupReason).ToString()); //��Ҫ������ ��ʱ���� TODO
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
					//    // ��Ҫȷ�������ȡ3���ĺͻ��ǣ�
					//    var bstr = b.ToString("X2");

					//    Console.Write(Convert.ToInt32(bstr, 16).ToString() + " ");
					//}
					//Console.WriteLine();

					//Console.WriteLine("ConnectionNnumber:" + BitConverter.ToString(glCdr.ConnectionNnumber));

					//Console.WriteLine("CIC:" + BitConverter.ToString(glCdr.CIC));

					//Console.WriteLine("Other:" + BitConverter.ToString(glCdr.Other));





				}
				swGl40.Close();
				br.Close();


            }
            #endregion
            
            Console.WriteLine("�밴�س�����������");
			Console.ReadLine();
		}
	}
}
