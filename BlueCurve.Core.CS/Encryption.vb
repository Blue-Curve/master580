Imports System
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class Encryption
    ' <summary>
    ' Encrypts specified plaintext using Rijndael symmetric key algorithm
    ' and returns a base64-encoded result.
    ' </summary>
    ' <param name="plainText">
    ' Plaintext value to be encrypted.
    ' </param>
    ' <param name="passPhrase">
    ' Passphrase from which a pseudo-random password will be derived. The 
    ' derived password will be used to generate the encryption key. 
    ' Passphrase can be any string. In this example we assume that this 
    ' passphrase is an ASCII string.
    ' </param>
    ' <param name="saltValue">
    ' Salt value used along with passphrase to generate password. Salt can 
    ' be any string. In this example we assume that salt is an ASCII string.
    ' </param>
    ' <param name="hashAlgorithm">
    ' Hash algorithm used to generate password. Allowed values are: "MD5" and
    ' "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
    ' </param>
    ' <param name="passwordIterations">
    ' Number of iterations used to generate password. One or two iterations
    ' should be enough.
    ' </param>
    ' <param name="initVector">
    ' Initialization vector (or IV). This value is required to encrypt the 
    ' first block of plaintext data. For RijndaelManaged class IV must be 
    ' exactly 16 ASCII characters long.
    ' </param>
    ' <param name="keySize">
    ' Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
    ' Longer keys are more secure than shorter keys.
    ' </param>
    ' <returns>
    ' Encrypted value formatted as a base64-encoded string.
    ' </returns>
    Private passPhrase As String = "Gy&k(dsfP"                   ' can be any string
    Private saltValue As String = "P£H7Koiop"                 ' can be any string
    Private HashAlgorithm As String = "SHA1"                  ' can be "MD5"
    Private passwordIterations As String = 7                   ' can be any number
    Private initVector As String = "o*u2c3DL3$F6g7I9"          ' must be 16 bytes
    Private keySize As String = 256                            ' can be 192 or 128
    Public Function Encrypt(ByVal plainText As String) As String
        ' Convert strings into byte arrays.
        ' Let us assume that strings only contain ASCII codes.
        ' If strings include Unicode characters, use Unicode, UTF7, or UTF8 
        ' encoding.
        Dim initVectorBytes As Byte()
        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

        Dim saltValueBytes As Byte()
        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

        ' Convert our plaintext into a byte array.
        ' Let us assume that plaintext contains UTF8-encoded characters.
        Dim plainTextBytes As Byte()
        plainTextBytes = Encoding.UTF8.GetBytes(plainText)

        ' First, we must create a password, from which the key will be derived.
        ' This password will be generated from the specified passphrase and 
        ' salt value. The password will be created using the specified hash 
        ' algorithm. Password creation can be done in several iterations.
        Dim password As Rfc2898DeriveBytes
        password = New Rfc2898DeriveBytes(passPhrase, _
                                           saltValueBytes, _
                                           passwordIterations)

        ' Use the password to generate pseudo-random bytes for the encryption
        ' key. Specify the size of the key in bytes (instead of bits).
        Dim keyBytes As Byte()
        keyBytes = password.GetBytes(keySize / 8)

        ' Create uninitialized Rijndael encryption object.
        Dim symmetricKey As RijndaelManaged
        symmetricKey = New RijndaelManaged

        ' It is reasonable to set encryption mode to Cipher Block Chaining
        ' (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC

        ' Generate encryptor from the existing key bytes and initialization 
        ' vector. Key size will be defined based on the number of the key 
        ' bytes.
        Dim encryptor As ICryptoTransform
        encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)

        ' Define memory stream which will be used to hold encrypted data.
        Dim memoryStream As memoryStream
        memoryStream = New memoryStream

        ' Define cryptographic stream (always use Write mode for encryption).
        Dim cryptoStream As cryptoStream
        cryptoStream = New cryptoStream(memoryStream, _
                                        encryptor, _
                                        CryptoStreamMode.Write)
        ' Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)

        ' Finish encrypting.
        cryptoStream.FlushFinalBlock()

        ' Convert our encrypted data from a memory stream into a byte array.
        Dim cipherTextBytes As Byte()
        cipherTextBytes = memoryStream.ToArray()

        ' Close both streams.
        memoryStream.Close()
        cryptoStream.Close()

        ' Convert encrypted data into a base64-encoded string.
        Dim cipherText As String
        cipherText = Convert.ToBase64String(cipherTextBytes)

        ' Return encrypted string.
        Encrypt = cipherText
    End Function

    ' <summary>
    ' Decrypts specified ciphertext using Rijndael symmetric key algorithm.
    ' </summary>
    ' <param name="cipherText">
    ' Base64-formatted ciphertext value.
    ' </param>
    ' <param name="passPhrase">
    ' Passphrase from which a pseudo-random password will be derived. The 
    ' derived password will be used to generate the encryption key. 
    ' Passphrase can be any string. In this example we assume that this 
    ' passphrase is an ASCII string.
    ' </param>
    ' <param name="saltValue">
    ' Salt value used along with passphrase to generate password. Salt can 
    ' be any string. In this example we assume that salt is an ASCII string.
    ' </param>
    ' <param name="hashAlgorithm">
    ' Hash algorithm used to generate password. Allowed values are: "MD5" and
    ' "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
    ' </param>
    ' <param name="passwordIterations">
    ' Number of iterations used to generate password. One or two iterations
    ' should be enough.
    ' </param>
    ' <param name="initVector">
    ' Initialization vector (or IV). This value is required to encrypt the 
    ' first block of plaintext data. For RijndaelManaged class IV must be 
    ' exactly 16 ASCII characters long.
    ' </param>
    ' <param name="keySize">
    ' Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
    ' Longer keys are more secure than shorter keys.
    ' </param>
    ' <returns>
    ' Decrypted string value.
    ' </returns>
    ' <remarks>
    ' Most of the logic in this function is similar to the Encrypt 
    ' logic. In order for decryption to work, all parameters of this function
    ' - except cipherText value - must match the corresponding parameters of 
    ' the Encrypt function which was called to generate the 
    ' ciphertext.
    ' </remarks>
    Public Function Decrypt(ByVal cipherText As String) As String
      
        ' Convert strings defining encryption key characteristics into byte
        ' arrays. Let us assume that strings only contain ASCII codes.
        ' If strings include Unicode characters, use Unicode, UTF7, or UTF8
        ' encoding.
        Dim initVectorBytes As Byte()
        initVectorBytes = Encoding.ASCII.GetBytes(initVector)

        Dim saltValueBytes As Byte()
        saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

        ' Convert our ciphertext into a byte array.
        Dim cipherTextBytes As Byte()
        cipherTextBytes = Convert.FromBase64String(cipherText)

        ' First, we must create a password, from which the key will be 
        ' derived. This password will be generated from the specified 
        ' passphrase and salt value. The password will be created using
        ' the specified hash algorithm. Password creation can be done in
        ' several iterations.
        Dim password As Rfc2898DeriveBytes
        password = New Rfc2898DeriveBytes(passPhrase, _
                                           saltValueBytes, _
                                           passwordIterations)

        ' Use the password to generate pseudo-random bytes for the encryption
        ' key. Specify the size of the key in bytes (instead of bits).
        Dim keyBytes As Byte()
        keyBytes = password.GetBytes(keySize / 8)

        ' Create uninitialized Rijndael encryption object.
        Dim symmetricKey As RijndaelManaged
        symmetricKey = New RijndaelManaged

        ' It is reasonable to set encryption mode to Cipher Block Chaining
        ' (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC

        ' Generate decryptor from the existing key bytes and initialization 
        ' vector. Key size will be defined based on the number of the key 
        ' bytes.
        Dim decryptor As ICryptoTransform
        decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

        ' Define memory stream which will be used to hold encrypted data.
        Dim memoryStream As memoryStream
        memoryStream = New memoryStream(cipherTextBytes)

        ' Define memory stream which will be used to hold encrypted data.
        Dim cryptoStream As cryptoStream
        cryptoStream = New cryptoStream(memoryStream, _
                                        decryptor, _
                                        CryptoStreamMode.Read)

        ' Since at this point we don't know what the size of decrypted data
        ' will be, allocate the buffer long enough to hold ciphertext;
        ' plaintext is never longer than ciphertext.
        Dim plainTextBytes As Byte()
        ReDim plainTextBytes(cipherTextBytes.Length)

        ' Start decrypting.
        Dim decryptedByteCount As Integer
        decryptedByteCount = cryptoStream.Read(plainTextBytes, _
                                               0, _
                                               plainTextBytes.Length)

        ' Close both streams.
        memoryStream.Close()
        cryptoStream.Close()

        ' Convert decrypted data into a string. 
        ' Let us assume that the original plaintext string was UTF8-encoded.
        Dim plainText As String
        plainText = Encoding.UTF8.GetString(plainTextBytes, _
                                            0, _
                                            decryptedByteCount)

        ' Return decrypted string.
        Decrypt = plainText
    End Function
End Class


#Region "  Hash"

''' <summary>
''' Hash functions are fundamental to modern cryptography. These functions map binary 
''' strings of an arbitrary length to small binary strings of a fixed length, known as 
''' hash values. A cryptographic hash function has the property that it is computationally
''' infeasible to find two distinct inputs that hash to the same value. Hash functions 
''' are commonly used with digital signatures and for data integrity.
''' </summary>

'==========================================
' Blue curve Limited 2010
' Module:         Encryption
' Type:           CS
' Desciption:     Hashing class
' Public Methods: bc_cs_encyption_hash
'                 
' Version:        1.0
' Change history:
'
'==========================================

Public Class bc_cs_encyption_hash

    ''' <summary>
    ''' Type of hash; some are security oriented, others are fast and simple
    ''' </summary>
    Public Enum Provider
        ''' <summary>
        ''' Cyclic Redundancy Check provider, 32-bit
        ''' </summary>
        CRC32
        ''' <summary>
        ''' Secure Hashing Algorithm provider, SHA-1 variant, 160-bit
        ''' </summary>
        SHA1
        ''' <summary>
        ''' Secure Hashing Algorithm provider, SHA-2 variant, 256-bit
        ''' </summary>
        SHA256
        ''' <summary>
        ''' Secure Hashing Algorithm provider, SHA-2 variant, 384-bit
        ''' </summary>
        SHA384
        ''' <summary>
        ''' Secure Hashing Algorithm provider, SHA-2 variant, 512-bit
        ''' </summary>
        SHA512
        ''' <summary>
        ''' Message Digest algorithm 5, 128-bit
        ''' </summary>
        MD5
    End Enum

    Private _Hash As HashAlgorithm
    Private _HashValue As New bc_cs_encyption_data

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Instantiate a new hash of the specified type
    ''' </summary>
    Public Sub New(ByVal p As Provider)
        Select Case p
            Case Provider.CRC32
                _Hash = New CRC32
            Case Provider.MD5
                _Hash = New MD5CryptoServiceProvider
            Case Provider.SHA1
                _Hash = New SHA1Managed
            Case Provider.SHA256
                _Hash = New SHA256Managed
            Case Provider.SHA384
                _Hash = New SHA384Managed
            Case Provider.SHA512
                _Hash = New SHA512Managed
        End Select
    End Sub

    ''' <summary>
    ''' Returns the previously calculated hash
    ''' </summary>
    Public ReadOnly Property Value() As bc_cs_encyption_data
        Get
            Return _HashValue
        End Get
    End Property

    ''' <summary>
    ''' Calculates hash on a stream of arbitrary length
    ''' </summary>
    Public Function Calculate(ByRef s As System.IO.Stream) As bc_cs_encyption_data
        _HashValue.Bytes = _Hash.ComputeHash(s)
        Return _HashValue
    End Function

    ''' <summary>
    ''' Calculates hash for fixed length <see cref="Data"/>
    ''' </summary>
    Public Function Calculate(ByVal d As bc_cs_encyption_data) As bc_cs_encyption_data
        Return CalculatePrivate(d.Bytes)
    End Function

    ''' <summary>
    ''' Calculates hash for a string with a sufix salt value. 
    ''' A "salt" is random data sufixed to every hashed value to prevent 
    ''' common dictionary attacks.
    ''' </summary>
    Public Function Calculate(ByVal d As bc_cs_encyption_data, ByVal salt As bc_cs_encyption_data) As bc_cs_encyption_data
        Dim nb(d.Bytes.Length + salt.Bytes.Length - 1) As Byte
        d.Bytes.CopyTo(nb, 0)
        salt.Bytes.CopyTo(nb, d.Bytes.Length)
        Return CalculatePrivate(nb)
    End Function

    ''' <summary>
    ''' Calculates hash for an array of bytes
    ''' </summary>
    Private Function CalculatePrivate(ByVal b() As Byte) As bc_cs_encyption_data
        _HashValue.Bytes = _Hash.ComputeHash(b)
        Return _HashValue
    End Function

#Region "  CRC32 HashAlgorithm"
    Private Class CRC32
        Inherits HashAlgorithm

        Private result As Integer = &HFFFFFFFF

        Protected Overrides Sub HashCore(ByVal array() As Byte, ByVal ibStart As Integer, ByVal cbSize As Integer)
            Dim lookup As Integer
            For i As Integer = ibStart To cbSize - 1
                lookup = (result And &HFF) Xor array(i)
                result = ((result And &HFFFFFF00) \ &H100) And &HFFFFFF
                result = result Xor crcLookup(lookup)
            Next i
        End Sub

        Protected Overrides Function HashFinal() As Byte()
            Dim b() As Byte = BitConverter.GetBytes(Not result)
            Array.Reverse(b)
            Return b
        End Function

        Public Overrides Sub Initialize()
            result = &HFFFFFFFF
        End Sub

        Private crcLookup() As Integer = { _
            &H0, &H77073096, &HEE0E612C, &H990951BA, _
            &H76DC419, &H706AF48F, &HE963A535, &H9E6495A3, _
            &HEDB8832, &H79DCB8A4, &HE0D5E91E, &H97D2D988, _
            &H9B64C2B, &H7EB17CBD, &HE7B82D07, &H90BF1D91, _
            &H1DB71064, &H6AB020F2, &HF3B97148, &H84BE41DE, _
            &H1ADAD47D, &H6DDDE4EB, &HF4D4B551, &H83D385C7, _
            &H136C9856, &H646BA8C0, &HFD62F97A, &H8A65C9EC, _
            &H14015C4F, &H63066CD9, &HFA0F3D63, &H8D080DF5, _
            &H3B6E20C8, &H4C69105E, &HD56041E4, &HA2677172, _
            &H3C03E4D1, &H4B04D447, &HD20D85FD, &HA50AB56B, _
            &H35B5A8FA, &H42B2986C, &HDBBBC9D6, &HACBCF940, _
            &H32D86CE3, &H45DF5C75, &HDCD60DCF, &HABD13D59, _
            &H26D930AC, &H51DE003A, &HC8D75180, &HBFD06116, _
            &H21B4F4B5, &H56B3C423, &HCFBA9599, &HB8BDA50F, _
            &H2802B89E, &H5F058808, &HC60CD9B2, &HB10BE924, _
            &H2F6F7C87, &H58684C11, &HC1611DAB, &HB6662D3D, _
            &H76DC4190, &H1DB7106, &H98D220BC, &HEFD5102A, _
            &H71B18589, &H6B6B51F, &H9FBFE4A5, &HE8B8D433, _
            &H7807C9A2, &HF00F934, &H9609A88E, &HE10E9818, _
            &H7F6A0DBB, &H86D3D2D, &H91646C97, &HE6635C01, _
            &H6B6B51F4, &H1C6C6162, &H856530D8, &HF262004E, _
            &H6C0695ED, &H1B01A57B, &H8208F4C1, &HF50FC457, _
            &H65B0D9C6, &H12B7E950, &H8BBEB8EA, &HFCB9887C, _
            &H62DD1DDF, &H15DA2D49, &H8CD37CF3, &HFBD44C65, _
            &H4DB26158, &H3AB551CE, &HA3BC0074, &HD4BB30E2, _
            &H4ADFA541, &H3DD895D7, &HA4D1C46D, &HD3D6F4FB, _
            &H4369E96A, &H346ED9FC, &HAD678846, &HDA60B8D0, _
            &H44042D73, &H33031DE5, &HAA0A4C5F, &HDD0D7CC9, _
            &H5005713C, &H270241AA, &HBE0B1010, &HC90C2086, _
            &H5768B525, &H206F85B3, &HB966D409, &HCE61E49F, _
            &H5EDEF90E, &H29D9C998, &HB0D09822, &HC7D7A8B4, _
            &H59B33D17, &H2EB40D81, &HB7BD5C3B, &HC0BA6CAD, _
            &HEDB88320, &H9ABFB3B6, &H3B6E20C, &H74B1D29A, _
            &HEAD54739, &H9DD277AF, &H4DB2615, &H73DC1683, _
            &HE3630B12, &H94643B84, &HD6D6A3E, &H7A6A5AA8, _
            &HE40ECF0B, &H9309FF9D, &HA00AE27, &H7D079EB1, _
            &HF00F9344, &H8708A3D2, &H1E01F268, &H6906C2FE, _
            &HF762575D, &H806567CB, &H196C3671, &H6E6B06E7, _
            &HFED41B76, &H89D32BE0, &H10DA7A5A, &H67DD4ACC, _
            &HF9B9DF6F, &H8EBEEFF9, &H17B7BE43, &H60B08ED5, _
            &HD6D6A3E8, &HA1D1937E, &H38D8C2C4, &H4FDFF252, _
            &HD1BB67F1, &HA6BC5767, &H3FB506DD, &H48B2364B, _
            &HD80D2BDA, &HAF0A1B4C, &H36034AF6, &H41047A60, _
            &HDF60EFC3, &HA867DF55, &H316E8EEF, &H4669BE79, _
            &HCB61B38C, &HBC66831A, &H256FD2A0, &H5268E236, _
            &HCC0C7795, &HBB0B4703, &H220216B9, &H5505262F, _
            &HC5BA3BBE, &HB2BD0B28, &H2BB45A92, &H5CB36A04, _
            &HC2D7FFA7, &HB5D0CF31, &H2CD99E8B, &H5BDEAE1D, _
            &H9B64C2B0, &HEC63F226, &H756AA39C, &H26D930A, _
            &H9C0906A9, &HEB0E363F, &H72076785, &H5005713, _
            &H95BF4A82, &HE2B87A14, &H7BB12BAE, &HCB61B38, _
            &H92D28E9B, &HE5D5BE0D, &H7CDCEFB7, &HBDBDF21, _
            &H86D3D2D4, &HF1D4E242, &H68DDB3F8, &H1FDA836E, _
            &H81BE16CD, &HF6B9265B, &H6FB077E1, &H18B74777, _
            &H88085AE6, &HFF0F6A70, &H66063BCA, &H11010B5C, _
            &H8F659EFF, &HF862AE69, &H616BFFD3, &H166CCF45, _
            &HA00AE278, &HD70DD2EE, &H4E048354, &H3903B3C2, _
            &HA7672661, &HD06016F7, &H4969474D, &H3E6E77DB, _
            &HAED16A4A, &HD9D65ADC, &H40DF0B66, &H37D83BF0, _
            &HA9BCAE53, &HDEBB9EC5, &H47B2CF7F, &H30B5FFE9, _
            &HBDBDF21C, &HCABAC28A, &H53B39330, &H24B4A3A6, _
            &HBAD03605, &HCDD70693, &H54DE5729, &H23D967BF, _
            &HB3667A2E, &HC4614AB8, &H5D681B02, &H2A6F2B94, _
            &HB40BBE37, &HC30C8EA1, &H5A05DF1B, &H2D02EF8D}

        Public Overrides ReadOnly Property Hash() As Byte()
            Get
                Dim b() As Byte = BitConverter.GetBytes(Not result)
                Array.Reverse(b)
                Return b
            End Get
        End Property
    End Class

#End Region

End Class
#End Region

#Region "  Data"

'==========================================
' Blue curve Limited 2010
' Module:         Encryption
' Type:           CS
' Desciption:     Hashing Data class
' Public Methods: bc_cs_encyption_data
'                 
' Version:        1.0
' Change history:
'
'=====================================

''' <summary>
''' represents Hex, Byte, Base64, or String data to encrypt/decrypt;
''' use the .Text property to set/get a string representation 
''' use the .Hex property to set/get a string-based Hexadecimal representation 
''' use the .Base64 to set/get a string-based Base64 representation 
''' </summary>

Public Class bc_cs_encyption_data
    Private _b As Byte()
    Private _MaxBytes As Integer = 0
    Private _MinBytes As Integer = 0
    Private _StepBytes As Integer = 0

    ''' <summary>
    ''' Determines the default text encoding across ALL Data instances
    ''' </summary>
    Public Shared DefaultEncoding As Text.Encoding = System.Text.Encoding.GetEncoding("Windows-1252")

    ''' <summary>
    ''' Determines the default text encoding for this Data instance
    ''' </summary>
    Public Encoding As Text.Encoding = DefaultEncoding

    ''' <summary>
    ''' Creates new, empty encryption data
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Creates new encryption data with the specified byte array
    ''' </summary>
    Public Sub New(ByVal b As Byte())
        _b = b
    End Sub

    ''' <summary>
    ''' Creates new encryption data with the specified string; 
    ''' will be converted to byte array using default encoding
    ''' </summary>
    Public Sub New(ByVal s As String)
        Me.Text = s
    End Sub

    ''' <summary>
    ''' Creates new encryption data using the specified string and the 
    ''' specified encoding to convert the string to a byte array.
    ''' </summary>
    Public Sub New(ByVal s As String, ByVal encoding As System.Text.Encoding)
        Me.Encoding = encoding
        Me.Text = s
    End Sub

    ''' <summary>
    ''' returns true if no data is present
    ''' </summary>
    Public ReadOnly Property IsEmpty() As Boolean
        Get
            If _b Is Nothing Then
                Return True
            End If
            If _b.Length = 0 Then
                Return True
            End If
            Return False
        End Get
    End Property

    ''' <summary>
    ''' allowed step interval, in bytes, for this data; if 0, no limit
    ''' </summary>
    Public Property StepBytes() As Integer
        Get
            Return _StepBytes
        End Get
        Set(ByVal Value As Integer)
            _StepBytes = Value
        End Set
    End Property

    ''' <summary>
    ''' allowed step interval, in bits, for this data; if 0, no limit
    ''' </summary>
    Public Property StepBits() As Integer
        Get
            Return _StepBytes * 8
        End Get
        Set(ByVal Value As Integer)
            _StepBytes = Value \ 8
        End Set
    End Property

    ''' <summary>
    ''' minimum number of bytes allowed for this data; if 0, no limit
    ''' </summary>
    Public Property MinBytes() As Integer
        Get
            Return _MinBytes
        End Get
        Set(ByVal Value As Integer)
            _MinBytes = Value
        End Set
    End Property

    ''' <summary>
    ''' minimum number of bits allowed for this data; if 0, no limit
    ''' </summary>
    Public Property MinBits() As Integer
        Get
            Return _MinBytes * 8
        End Get
        Set(ByVal Value As Integer)
            _MinBytes = Value \ 8
        End Set
    End Property

    ''' <summary>
    ''' maximum number of bytes allowed for this data; if 0, no limit
    ''' </summary>
    Public Property MaxBytes() As Integer
        Get
            Return _MaxBytes
        End Get
        Set(ByVal Value As Integer)
            _MaxBytes = Value
        End Set
    End Property

    ''' <summary>
    ''' maximum number of bits allowed for this data; if 0, no limit
    ''' </summary>
    Public Property MaxBits() As Integer
        Get
            Return _MaxBytes * 8
        End Get
        Set(ByVal Value As Integer)
            _MaxBytes = Value \ 8
        End Set
    End Property

    ''' <summary>
    ''' Returns the byte representation of the data; 
    ''' This will be padded to MinBytes and trimmed to MaxBytes as necessary!
    ''' </summary>
    Public Property Bytes() As Byte()
        Get
            If _MaxBytes > 0 Then
                If _b.Length > _MaxBytes Then
                    Dim b(_MaxBytes - 1) As Byte
                    Array.Copy(_b, b, b.Length)
                    _b = b
                End If
            End If
            If _MinBytes > 0 Then
                If _b.Length < _MinBytes Then
                    Dim b(_MinBytes - 1) As Byte
                    Array.Copy(_b, b, _b.Length)
                    _b = b
                End If
            End If
            Return _b
        End Get
        Set(ByVal Value As Byte())
            _b = Value
        End Set
    End Property

    ''' <summary>
    ''' Sets or returns text representation of bytes using the default text encoding
    ''' </summary>
    Public Property Text() As String
        Get
            If _b Is Nothing Then
                Return ""
            Else
                '-- need to handle nulls here; oddly, C# will happily convert
                '-- nulls into the string whereas VB stops converting at the
                '-- first null!
                Dim i As Integer = Array.IndexOf(_b, CType(0, Byte))
                If i >= 0 Then
                    Return Me.Encoding.GetString(_b, 0, i)
                Else
                    Return Me.Encoding.GetString(_b)
                End If
            End If
        End Get
        Set(ByVal Value As String)
            _b = Me.Encoding.GetBytes(Value)
        End Set
    End Property

    ''' <summary>
    ''' Sets or returns Hex string representation of this data
    ''' </summary>
    Public Property Hex() As String
        Get
            Return bc_cs_encyption_utils.ToHex(_b)
        End Get
        Set(ByVal Value As String)
            _b = bc_cs_encyption_utils.FromHex(Value)
        End Set
    End Property

    ''' <summary>
    ''' Sets or returns Base64 string representation of this data
    ''' </summary>
    Public Property Base64() As String
        Get
            Return bc_cs_encyption_utils.ToBase64(_b)
        End Get
        Set(ByVal Value As String)
            _b = bc_cs_encyption_utils.FromBase64(Value)
        End Set
    End Property

    ''' <summary>
    ''' Returns text representation of bytes using the default text encoding
    ''' </summary>
    Public Shadows Function ToString() As String
        Return Me.Text
    End Function

    ''' <summary>
    ''' returns Base64 string representation of this data
    ''' </summary>
    Public Function ToBase64() As String
        Return Me.Base64
    End Function

    ''' <summary>
    ''' returns Hex string representation of this data
    ''' </summary>
    Public Function ToHex() As String
        Return Me.Hex
    End Function

End Class

#End Region

#Region "  Utils"

'==========================================
' Blue curve Limited 2010
' Module:         Encryption
' Type:           CS
' Desciption:     Encryptio Utils class
' Public Methods: bc_om_encyption_utils
'                 
' Version:        1.0
' Change history:
'
'=====================================

''' <summary>
''' Friend class for shared utility methods used by multiple Encryption classes
''' </summary>
Friend Class bc_cs_encyption_utils

    ''' <summary>
    ''' converts an array of bytes to a string Hex representation
    ''' </summary>
    Friend Shared Function ToHex(ByVal ba() As Byte) As String
        If ba Is Nothing OrElse ba.Length = 0 Then
            Return ""
        End If
        Const HexFormat As String = "{0:X2}"
        Dim sb As New StringBuilder
        For Each b As Byte In ba
            sb.Append(String.Format(HexFormat, b))
        Next
        Return sb.ToString
    End Function

    ''' <summary>
    ''' converts from a string Hex representation to an array of bytes
    ''' </summary>
    Friend Shared Function FromHex(ByVal hexEncoded As String) As Byte()
        If hexEncoded Is Nothing OrElse hexEncoded.Length = 0 Then
            Return Nothing
        End If
        Try
            Dim l As Integer = Convert.ToInt32(hexEncoded.Length / 2)
            Dim b(l - 1) As Byte
            For i As Integer = 0 To l - 1
                b(i) = Convert.ToByte(hexEncoded.Substring(i * 2, 2), 16)
            Next
            Return b
        Catch ex As Exception
            Throw New System.FormatException("The provided string does not appear to be Hex encoded:" & _
                Environment.NewLine & hexEncoded & Environment.NewLine, ex)
        End Try
    End Function

    ''' <summary>
    ''' converts from a string Base64 representation to an array of bytes
    ''' </summary>
    Friend Shared Function FromBase64(ByVal base64Encoded As String) As Byte()
        If base64Encoded Is Nothing OrElse base64Encoded.Length = 0 Then
            Return Nothing
        End If
        Try
            Return Convert.FromBase64String(base64Encoded)
        Catch ex As System.FormatException
            Throw New System.FormatException("The provided string does not appear to be Base64 encoded:" & _
                Environment.NewLine & base64Encoded & Environment.NewLine, ex)
        End Try
    End Function

    ''' <summary>
    ''' converts from an array of bytes to a string Base64 representation
    ''' </summary>
    Friend Shared Function ToBase64(ByVal b() As Byte) As String
        If b Is Nothing OrElse b.Length = 0 Then
            Return ""
        End If
        Return Convert.ToBase64String(b)
    End Function

End Class

#End Region
