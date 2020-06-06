using System;
using System.Runtime.InteropServices; 
using System.Text;
namespace Bill
{
	/// <summary>
	/// INI�ļ��Ĳ�����
	/// </summary>
	public class IniFile
	{
		public string Path;
		public IniFile(string path)
		{
			this.Path = path;
		}
		#region ������дINI�ļ���API���� 
		[DllImport("kernel32")] 
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath); 

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath); 

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
		#endregion

		/// <summary>
		/// дINI�ļ�
		/// </summary>
		/// <param name="section">����</param>
		/// <param name="key">��</param>
		/// <param name="iValue">ֵ</param>
		public void IniWriteValue(string section, string key, string iValue) 
		{
			WritePrivateProfileString(section, key, iValue, this.Path);
		}

		/// <summary>
		/// ��ȡINI�ļ�
		/// </summary>
		/// <param name="section">����</param>
		/// <param name="key">��</param>
		/// <returns>���صļ�ֵ</returns>
		public string IniReadValue(string section, string key) 
		{ 
			StringBuilder temp = new StringBuilder(255); 

			int i = GetPrivateProfileString(section, key, "", temp, 255, this.Path); 
			return temp.ToString();
		}

		/// <summary>
		/// ��ȡINI�ļ�
		/// </summary>
		/// <param name="Section">�Σ���ʽ[]</param>
		/// <param name="Key">��</param>
		/// <returns>����byte���͵�section����ֵ��</returns>
		public byte[] IniReadValues(string section, string key)
		{
			byte[] temp = new byte[255];

			int i = GetPrivateProfileString(section, key, "", temp, 255, this.Path);
			return temp;
		}
	}
}
