using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CrackerJac
{
	public class HashCracker
	{
		public HashAlgorithm HashAlgorithm { get; private set; }

		public HashCracker(string algoString = "MD5")
		{
			HashAlgorithm = ((HashAlgorithm)CryptoConfig.CreateFromName(algoString));
		}

		public void StartDictionaryAttack(Stream dictionary, string[] IDs, string[] hashes)
		{
			if (IDs.Length != hashes.Length)
				throw new Exception(string.Format("IDs and hashes must be the same length! Got {0} IDs and {1} hashes!", IDs.Length, hashes.Length));
			StreamReader reader = new StreamReader(dictionary);
			int len = IDs.Length;
			for (int i = 0; i < len; i++)
			{
				string hash = hashes[i];
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					string entry = reader.ReadLine();
					if (Hash(entry) == hash)
					{
						OnHashCracked(new HashCrackedEventArgs { Hash = hash, ID = IDs[i], PlainText = entry });
						break;
					}
				}
				reader.BaseStream.Position = 0;
			}
		}

		public event EventHandler<HashCrackedEventArgs> HashCracked;
		protected virtual void OnHashCracked(HashCrackedEventArgs e)
		{
			var handler = HashCracked;
			if (handler != null)
				handler(this, e);
		}

		public string Hash(string text)
		{
			if (text == null)
				return string.Empty;
			return BitConverter.ToString(HashAlgorithm.ComputeHash(new UTF8Encoding().GetBytes(text))).Replace("-", string.Empty).ToLower();
		}
	}
}