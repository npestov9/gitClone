using System.Security.Cryptography;
using System.Text;

namespace GitClone;

public class HashGenerator
{
    public string GenHash(string type, string content)
    {
        string strValue = type + " " + content.Length + " " + content;

        var byteVal = ASCIIEncoding.ASCII.GetBytes(strValue);

        var byteHash = new MD5CryptoServiceProvider().ComputeHash(byteVal);

        string hexaHash = ByteArrToString(byteHash);

        return hexaHash;

    }

    private string ByteArrToString(byte[] arrInput)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length; i++)
        {
            sOutput.Append((arrInput[i].ToString("X2")));
        }

        return sOutput.ToString();
    }
}