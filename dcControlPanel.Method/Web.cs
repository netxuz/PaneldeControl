using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace dcControlPanel.Method
{
  public class Usuario
  {
    private string pCodUsuario = string.Empty;
    public string CodUsuario { get { return pCodUsuario; } set { pCodUsuario = value; } }

    private string pNombres = string.Empty;
    public string Nombres { get { return pNombres; } set { pNombres = value; } }

    private string pTipo = string.Empty;
    public string Tipo { get { return pTipo; } set { pTipo = value; } }

    private string pEmail = string.Empty;
    public string Email { get { return pEmail; } set { pEmail = value; } }

    private string pFono = string.Empty;
    public string Fono { get { return pFono; } set { pFono = value; } }

    private string pImagen = string.Empty;
    public string Imagen { get { return pImagen; } set { pImagen = value; } }

    private string pCodNkey = string.Empty;
    public string CodNkey { get { return pCodNkey; } set { pCodNkey = value; } }

    private string pTipoUsuario = string.Empty;
    public string TipoUsuario { get { return pTipoUsuario; } set { pTipoUsuario = value; } }

    private string pNKeyUsuario = string.Empty;
    public string NKeyUsuario { get { return pNKeyUsuario; } set { pNKeyUsuario = value; } }

    private string pCodTipoUsuario = string.Empty;
    public string CodTipoUsuario { get { return pCodTipoUsuario; } set { pCodTipoUsuario = value; } }

    public Usuario()
    {
    }

  }

  public class Web
  {

    public string GetDatRefreshPage()
    {
      AppSettingsReader appReader = new AppSettingsReader();
      return appReader.GetValue("setuprefreshpage", typeof(string)).ToString();
    }

    public Usuario GetObjUsuario()
    {
      Usuario oIsUsuario;
      if ((HttpContext.Current.Session["USUARIO"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Session["USUARIO"].ToString())))
      {
        oIsUsuario = (Usuario)HttpContext.Current.Session["USUARIO"];
      }
      else
      {
        oIsUsuario = new Usuario();
      }
      return oIsUsuario;

    }

    public string GetIpUsuario()
    {
      string sIpUsuario = ((HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) && (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString())) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString() : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());
      return sIpUsuario;
    }

    public void ValidaSessionAdm()
    {
      bool bValida = false;
      if ((HttpContext.Current.Session["USUARIO"] == null) || (HttpContext.Current.Session["AdministradorPanel"] == null) || (string.IsNullOrEmpty(HttpContext.Current.Session["USUARIO"].ToString())) || (string.IsNullOrEmpty(HttpContext.Current.Session["AdministradorPanel"].ToString())))
        bValida = true;
      else
        if (HttpContext.Current.Session["AdministradorPanel"].ToString() != "1")
        bValida = true;

      if (bValida)
        HttpContext.Current.Response.Redirect("redirection.htm");

    }

    public void ValidaUserPanel()
    {

      if ((HttpContext.Current.Session["USUARIO"] == null) || (string.IsNullOrEmpty(HttpContext.Current.Session["USUARIO"].ToString())))
        HttpContext.Current.Response.Redirect("redirection.htm");

    }

    public Usuario ValidaUserAppReport()
    {
      Usuario oIsUsuario = null;
      if ((HttpContext.Current.Session["USUARIO"] != null))
      {
        oIsUsuario = GetObjUsuario();
        if (string.IsNullOrEmpty(oIsUsuario.CodNkey))
          HttpContext.Current.Response.Redirect("redirection.htm");
      }
      else
      {
        HttpContext.Current.Response.Redirect("redirection.htm");
      }

      return oIsUsuario;
    }

    public string GetData(string sData)
    {
      string sRetorno = String.Empty;

      try
      {
        if (HttpContext.Current.Request.Form.Count != 0)
        {
          sRetorno = Convert.ToString(HttpContext.Current.Request.Form[sData]);
        }
        else if (HttpContext.Current.Request.QueryString.Count != 0)
        {
          sRetorno = Convert.ToString(HttpContext.Current.Request.QueryString[sData]);
        }
        return sRetorno;
      }
      catch { return sRetorno; }
    }

    public string GetSession(string sData)
    {
      string sRetorno = string.Empty;
      if (HttpContext.Current.Session[sData] != null)
        sRetorno = HttpContext.Current.Session[sData].ToString();

      return sRetorno;
    }

    public bool ValidaMail(string sMail)
    {
      bool bSuccess = false;
      Regex r = new Regex("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
      //Regex r = new Regex("/^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,4})+$/", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
      bSuccess = r.Match(sMail).Success;

      return bSuccess;
    }

    public DataTable DeserializarTbl(string cPath, string cFile)
    {
      if (!string.IsNullOrEmpty(cPath))
      {

        StringBuilder oFolder = new StringBuilder();
        oFolder.Append(cPath);
        oFolder.Append(@"\binary\");
        if (File.Exists(oFolder.ToString() + cFile))
        {
          IFormatter oBinFormat = new BinaryFormatter();
          Stream oFileStream = new FileStream(oFolder.ToString() + cFile, FileMode.Open, FileAccess.Read, FileShare.Read);
          DataTable oData = (DataTable)oBinFormat.Deserialize(oFileStream);
          oFileStream.Close();
          return oData;
        }
        return new DataTable();
      }
      return new DataTable();
    }

    public byte[] GetImageBytes(Stream stream)
    {
      byte[] buffer;

      using (Bitmap image = ResizeImage(stream))
      {
        using (MemoryStream ms = new MemoryStream())
        {
          image.Save(ms, ImageFormat.Png);

          //return the current position in the stream at the beginning
          ms.Position = 0;

          buffer = new byte[ms.Length];
          ms.Read(buffer, 0, (int)ms.Length);
          return buffer;
        }
      }
    }

    public byte[] GetImageBytes(Stream stream, int height, int width)
    {
      byte[] buffer;

      using (Bitmap image = ResizeImage(stream, height, width))
      {
        using (MemoryStream ms = new MemoryStream())
        {
          image.Save(ms, ImageFormat.Png);

          //return the current position in the stream at the beginning
          ms.Position = 0;

          buffer = new byte[ms.Length];
          ms.Read(buffer, 0, (int)ms.Length);
          return buffer;
        }
      }
    }

    public Bitmap ResizeImage(Stream stream)
    {
      System.Drawing.Image originalImage = Bitmap.FromStream(stream);

      Bitmap scaledImage = new Bitmap(originalImage.Width, originalImage.Height);

      using (Graphics g = Graphics.FromImage(scaledImage))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);

        return scaledImage;
      }

    }

    public Bitmap ResizeImage(Stream stream, int height, int width)
    {

      System.Drawing.Image originalImage = Bitmap.FromStream(stream);

      //int height = 300;
      //int width = 300;

      double ratio = Math.Min(originalImage.Width, originalImage.Height) / (double)Math.Max(originalImage.Width, originalImage.Height);

      if (originalImage.Width > originalImage.Height)
      {
        height = Convert.ToInt32(height * ratio);
      }
      else
      {
        width = Convert.ToInt32(width * ratio);
      }

      Bitmap scaledImage = new Bitmap(width, height);

      using (Graphics g = Graphics.FromImage(scaledImage))
      {
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.DrawImage(originalImage, 0, 0, width, height);

        return scaledImage;
      }

    }

    #region Encriptar
    /// <summary>
    /// Método para encriptar un texto plano usando el algoritmo (Rijndael).
    /// Este es el mas simple posible, muchos de los datos necesarios los
    /// definimos como constantes.
    /// </summary>
    /// <param name="textoQueEncriptaremos">texto a encriptar</param>
    /// <returns>Texto encriptado</returns>
    public string Crypt(string textoQueEncriptaremos)
    {
      return Crypt(textoQueEncriptaremos, "icommunity75dc@avz10", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
    }
    /// <summary>
    /// Método para encriptar un texto plano usando el algoritmo (Rijndael)
    /// </summary>
    /// <returns>Texto encriptado</returns>
    public string Crypt(string textoQueEncriptaremos, string passBase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
    {
      byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
      byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
      byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
      PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
      byte[] keyBytes = password.GetBytes(keySize / 8);
      RijndaelManaged symmetricKey = new RijndaelManaged()
      {
        Mode = CipherMode.CBC
      };
      ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
      cryptoStream.FlushFinalBlock();
      byte[] cipherTextBytes = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      string cipherText = Convert.ToBase64String(cipherTextBytes);
      return cipherText;
    }
    #endregion

    #region Desencriptar
    /// <summary>
    /// Método para desencriptar un texto encriptado.
    /// </summary>
    /// <returns>Texto desencriptado</returns>
    public string UnCrypt(string textoEncriptado)
    {
      return UnCrypt(textoEncriptado, "icommunity75dc@avz10", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
    }
    /// <summary>
    /// Método para desencriptar un texto encriptado (Rijndael)
    /// </summary>
    /// <returns>Texto desencriptado</returns>
    public string UnCrypt(string textoEncriptado, string passBase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
    {
      byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
      byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
      byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);
      PasswordDeriveBytes password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
      byte[] keyBytes = password.GetBytes(keySize / 8);
      RijndaelManaged symmetricKey = new RijndaelManaged()
      {
        Mode = CipherMode.CBC
      };
      ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
      MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
      CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] plainTextBytes = new byte[cipherTextBytes.Length];
      int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
      memoryStream.Close();
      cryptoStream.Close();
      string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
      return plainText;
    }
    #endregion

    public string getColorArrowCalidad(double dValorActual, double dValueAnterior)
    {

      if (dValorActual < dValueAnterior)
      {
        return "images/flechas/flecharojaabajo.jpg";
      }
      else if (dValorActual > dValueAnterior)
      {
        return "images/flechas/flechaverdearriba.jpg";
      }
      else
      {
        return "images/flechas/flechaverdederecha.jpg";
      }
    }

    public string getColorArrowImpecabilidad(double dValorActual, double dValueAnterior)
    {
      if (dValorActual < dValueAnterior)
      {
        return "images/flechas/flechaverdeabajo.jpg";
      }
      else if (dValorActual > dValueAnterior)
      {
        return "images/flechas/flecharojaarriba.jpg";
      }
      else
      {
        return "images/flechas/flechaverdederecha.jpg";
      }
    }

    public string getColor(string sColor)
    {
      string sDato = string.Empty;
      switch (sColor)
      {
        case "V":
          sDato = "tbDatVerde";
          break;
        case "A":
          sDato = "tbDatAmarilla";
          break;
        case "R":
          sDato = "tbDatRojo";
          break;
      }
      return sDato;
    }

    public string getColorNumAvance(double dValor, DataTable dtValores)
    {
      if (dValor <= double.Parse(dtValores.Rows[0]["valor_bajo"].ToString()))
      {
        return getColor(dtValores.Rows[0]["colorbajo"].ToString());
      }
      else if ((dValor > double.Parse(dtValores.Rows[0]["valor_bajo"].ToString())) && (dValor < double.Parse(dtValores.Rows[0]["valor_alto"].ToString())))
      {
        return getColor(dtValores.Rows[0]["colormedio"].ToString());
      }
      else
      {
        return getColor(dtValores.Rows[0]["coloralto"].ToString());
      }
    }

    public string fUnCrypt(string txtInp)
    {
      string txtOut = string.Empty;
      char strCaracter;
      int lngCodigo = 0;
      int lnPos = 1;
      int Acum = 0;
      char cDato = ' ';

      if (txtInp.Length > 0)
      {
        //Acum = Asc(Mid(txtInp, 1, 1)) / 2;
        cDato = Convert.ToChar(txtInp.Substring(0, 1));
        Acum = ((int)cDato) / 2;
        do
        {
          //lngCodigo = Asc(Mid(txtInp, lnPos, 1)) - Acum
          lngCodigo = (int)(Convert.ToChar(txtInp.Substring(lnPos, 1))) - Acum;
          if (lngCodigo < 1)
          {
            lngCodigo = lngCodigo + 255;
          }
          strCaracter = (char)lngCodigo;
          txtOut = txtOut + strCaracter.ToString();
          //Acum = (Asc(Mid(txtInp, lnPos, 1)) - Acum) + Acum;
          Acum = (int)(Convert.ToChar(txtInp.Substring(lnPos, 1))) + Acum;
          lnPos++;
        } while (lnPos <= txtInp.Length);
      }

      return txtOut;
    }
  }
}
