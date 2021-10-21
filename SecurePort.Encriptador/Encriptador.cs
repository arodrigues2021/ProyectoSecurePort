using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurePort.Encriptador
{

	public class encriptador
	{
		
		#region "Estructura y variables privadas"


		internal const string Claveencriptacion = "CLAVEENCRIPTACION";
		#endregion

		#region "Funciones de ecriptación y desencriptación"

		/// <summary> 
		/// Encriptacion
		/// </summary> 
		/// <param name="input">String to encrypt</param> 
		/// <returns>Encrypted string</returns> 
		public string Encriptacion(string input, string password = "CLAVEENCRIPTACION")
		{

			byte[] utfData = UTF8Encoding.UTF8.GetBytes(input);
			byte[] saltBytes = Encoding.UTF8.GetBytes(password);
			string encryptedString = null;

			using (AesManaged aes = new AesManaged()) {
				Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, saltBytes);

				aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
				aes.KeySize = aes.LegalKeySizes[0].MaxSize;
				aes.Key = rfc.GetBytes(aes.KeySize / 8);
				aes.IV = rfc.GetBytes(aes.BlockSize / 8);

				using (ICryptoTransform encryptTransform = aes.CreateEncryptor()) {
					using (MemoryStream encryptedStream = new MemoryStream()) {
						using (CryptoStream encryptor = new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write)) {
							encryptor.Write(utfData, 0, utfData.Length);
							encryptor.Flush();
							encryptor.Close();

							byte[] encryptBytes = encryptedStream.ToArray();
							encryptedString = Convert.ToBase64String(encryptBytes);
						}
					}
				}
			}

			return encryptedString;

		}

		/// <summary> 
		/// DesEncriptacion a string 
		/// </summary> 
		/// <param name="input">Input string in base 64 format</param> 
		/// <returns>Decrypted string</returns> 
		public string DesEncriptacion(string input, string password = "CLAVEENCRIPTACION")
		{

			byte[] encryptedBytes = Convert.FromBase64String(input);
			byte[] saltBytes = Encoding.UTF8.GetBytes(password);
			string decryptedString = null;

			using (AesManaged  Aes = new AesManaged())
			{
				Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, saltBytes);
				Aes.BlockSize = Aes.LegalBlockSizes[0].MaxSize;
				Aes.KeySize = Aes.LegalKeySizes[0].MaxSize;
				Aes.Key = rfc.GetBytes(Aes.KeySize / 8);
				Aes.IV = rfc.GetBytes(Aes.BlockSize / 8);

				using (ICryptoTransform decryptTransform = Aes.CreateDecryptor()) {
					using (MemoryStream decryptedStream = new MemoryStream()) {
						CryptoStream decryptor = new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
						decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
						decryptor.Flush();
						decryptor.Close();

						byte[] decryptBytes = decryptedStream.ToArray();
						decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
					}
				}
			}

			return decryptedString;

		}

		#endregion

	}

}
   
 
