using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReplaceText;

namespace ReplaceText
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 0)
			{
				Console.WriteLine("args error: incorrect parameters.");
				return;
			}

			// 
			DoReplace();

			Console.ReadLine();
		}

		static void DoReplace()
		{
			string[] strFile = { "t", "s" };
			string strCurDir = System.Environment.CurrentDirectory;
			// target text file 
			SystemTipsMgr targetTips = new SystemTipsMgr();
			targetTips.LoadSysTip(strCurDir + "\\" + strFile[0] + ".txt");
			if (targetTips.m_listDuplicatedKey.Count != 0)
			{
				targetTips.LogFile_DuplicateKey(strCurDir + "\\" + strFile[0] + "_log.txt");
			}

			// source text file 
			SystemTipsMgr sourceTips = new SystemTipsMgr();
			sourceTips.LoadSysTip(strCurDir + "\\" + strFile[1] + ".txt");

			//////////////////////////////////////////////////////////////////////////
			bool bReplace = false;
			int nCount = 0;
			// Find and replace 
			foreach (KeyValuePair<string, string> kvp in targetTips.m_dicOriginal)
			{
				bReplace = false;
				// Find the key with value in the source file 1 
				string sValue = sourceTips.GetOriginalValue(kvp.Key);
				if (sValue != null)
				{
					bReplace = true;
					nCount++;
					// Replace it to target file 
					targetTips.InsertReplace(kvp.Key, sValue);
				}

				if (!bReplace)
				{
					targetTips.InsertReplace(kvp.Key, kvp.Value);
				}
			}

			// Final, create a file 
			if (nCount > 0)
			{
				targetTips.SaveToFile(strCurDir + "\\t_temp.txt");
				Console.WriteLine("Replace " + nCount.ToString() + " times and create a temp file.");
			}
		}

	}
}
