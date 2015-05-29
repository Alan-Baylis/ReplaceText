using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReplaceText
{
	public class SystemTipsMgr
	{
		public static string S_KEY_NewLine = "\r\n";
		public List<string> m_lineInFile = new List<string>();
		public Dictionary<string, string> m_dicOriginal = new Dictionary<string, string>();

		private Dictionary<string, string> m_dicReplace = new Dictionary<string, string>();
		public List<string> m_listDuplicatedKey = new List<string>();

		public void LoadSysTip(string strFilePath)
		{
			if (File.Exists(strFilePath))
			{
				using (StreamReader sr = new StreamReader(strFilePath, Encoding.GetEncoding("utf-8")))
				{
					ClearAll();
					char[] trimStart = { ' ', '\t' };
					char[] trimEnd = { ' ', '\r', '\n', '\t' };

					string strLine = null;
					bool bReplace = false;
					while ((strLine = sr.ReadLine()) != null)
					{
						bReplace = false;
						if (strLine.Length > 0)
						{
							int equalIndex = strLine.IndexOf("=");
							if (equalIndex != -1)
							{
								bReplace = true;
								string strKey = strLine.Substring(0, equalIndex);
								strKey = strKey.TrimEnd(trimEnd);
								strKey = strKey.TrimStart(trimStart);

								string strValue = strLine.Substring(equalIndex + 1);
								//strValue = strValue.TrimEnd(trimEnd);
								//strValue = strValue.TrimStart(trimStart);

								if (!m_dicOriginal.ContainsKey(strKey))
								{
									m_dicOriginal.Add(strKey, strValue);
									m_lineInFile.Add(strKey);
								}
								else
								{
									m_listDuplicatedKey.Add(strKey);
								}
							}
						}

						if (!bReplace)
						{
							m_lineInFile.Add(strLine);
						}
					}
					sr.Close();
					SyncDictionary_O2P();
				}
			}
		}

		public void ClearAll()
		{
			m_lineInFile.Clear();
			m_dicOriginal.Clear();
			m_dicReplace.Clear();
			m_listDuplicatedKey.Clear();
		}

		public void SyncDictionary_O2P()
		{
			m_dicReplace.Clear();
			foreach (KeyValuePair<string, string> kvp in m_dicOriginal)
			{
				m_dicReplace[kvp.Key] = kvp.Value;
			}
		}

		public string GetOriginalValue(string key)
		{
			if (m_dicOriginal.ContainsKey(key))
			{
				return m_dicOriginal[key];
			}
			return null;
		}

		public bool InsertReplace(string strKey, string strValue)
		{
			bool bReplace = m_dicReplace.ContainsKey(strKey);
			m_dicReplace[strKey] = strValue;

			return bReplace;
		}
		
		public void SaveToFile(string strFilePath)
		{
			using (StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.GetEncoding("utf-8")))
			{
				foreach (string strText in m_lineInFile)
				{
					if (m_dicReplace.ContainsKey(strText))
					{
						sw.WriteLine(strText + "=" + m_dicReplace[strText]);
					}
					else
					{
						sw.WriteLine(strText);
					}
				}
				sw.Flush();
				sw.Close();
			}
		}

		public void LogFile_DuplicateKey(string strFilePath)
		{
			using (StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.GetEncoding("utf-8")))
			{
				foreach (string strKey in m_listDuplicatedKey)
				{
					sw.WriteLine(strKey);
				}
				sw.Flush();
				sw.Close();
			}
		}
	}
}
