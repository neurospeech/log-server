using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LogServer.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHashService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        string Hash(string text, Encoding encoding);

        /// <summary>
        /// Default Encoding is UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string Hash(string text);

    }

    /// <summary>
    /// 
    /// </summary>
    [DIGlobal(typeof(IHashService))]
    public class HashService: IHashService {


        /// <summary>
        /// 
        /// </summary>
        public HashService()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Hash(string text)
        {
            return Hash(text, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string Hash(string text, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));


            var sha = System.Security.Cryptography.SHA256.Create();

            var hash = sha.ComputeHash(encoding.GetBytes(text));

            return Convert.ToBase64String(hash);
        }
    }
}