using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Mulholland.Core
{
	/// <summary>
	/// Provides encryption utilities.
	/// </summary>
	public class Cryptographer
	{
		/// <summary>
		/// Encrypts a string using DES.
		/// </summary>
		/// <param name="input">Value to encrypt.</param>
		/// <param name="key">Key.</param>
		/// <returns>Encrypted string.</returns>
		public string Encrypt(string input, string key)
		{
			string result = null;

			TripleDESCryptoServiceProvider des = null;
			MD5CryptoServiceProvider hashmd5 = null;
			byte[] pwdhash;
			byte[] buff;

			try
			{
				hashmd5 = new MD5CryptoServiceProvider();
				pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
				des = new TripleDESCryptoServiceProvider();
				des.Key = pwdhash;
				des.Mode = CipherMode.ECB; //CBC, CFB
				buff = ASCIIEncoding.ASCII.GetBytes(input);
				result = Convert.ToBase64String( des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
			}
			finally 
			{
				if (hashmd5 != null)
					hashmd5.Clear();
				if (des != null)
					des.Clear();
			}

			return result;
		}


		/// <summary>
		/// Decrypts a DES encrypted string.
		/// </summary>
		/// <param name="input">Encrypted string.</param>
		/// <param name="key">Key.</param>
		/// <returns>Decrypted string.</returns>
		public string Decrypt(string input, string key)
		{
			string result = null;

			TripleDESCryptoServiceProvider des = null;
			MD5CryptoServiceProvider hashmd5 = null;
			byte[] pwdhash;
			byte[] buff;

			try
			{
				hashmd5 = new MD5CryptoServiceProvider();
				pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
				des = new TripleDESCryptoServiceProvider();
				des.Key = pwdhash;
				des.Mode = CipherMode.ECB; //CBC, CFB
				buff = Convert.FromBase64String(input);
				result = ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
			}
			finally 
			{
				if (hashmd5 != null)
					hashmd5.Clear();
				if (des != null)
					des.Clear();
			}

			return result;
		}


		/// <summary>
		/// Generates a hash value using MD5 and the specified encoding.
		/// </summary>
		/// <param name="key">Key to generate hash on.</param>
		/// <param name="encoding">Encoding to use.</param>
		/// <returns>Hash.</returns>
		/// <overloaded/>
		public string Hash(string key, Encoding encoding)
		{
			string result = null;
			
			MD5CryptoServiceProvider hashmd5 = null;

			try
			{
                byte[] byteResult = new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));

                for (int n = 0; n <= byteResult.Length - 1; n++)
                {
                    // Bitwise AND - set high bit to x7F ( ASCII 127 )
                    byteResult[n] = (byte)(byteResult[n] & 0x7F);
                }

                return encoding.GetString(byteResult);

                /*
				hashmd5 = new MD5CryptoServiceProvider();
				result = encoding.GetString(hashmd5.ComputeHash(encoding.GetBytes(key)));						
                 */
			}
			finally 
			{
				//dispose objects
				if (hashmd5 != null)
					hashmd5.Clear();
			}

			return result;
		}


		/// <summary>
		/// Generates a hash value using MD5 and ASCII encoding.
		/// </summary>
		/// <param name="key">Key to generate hash on.</param>
		/// <returns>Hash.</returns>
		/// <overloaded/>
		public string Hash(string key)
		{
			return Hash(key, ASCIIEncoding.ASCII);
		}

	}
}
