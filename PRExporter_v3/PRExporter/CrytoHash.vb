
Namespace SimpleControls.SimpleCrypto
    Public Class CryptoHash

        Public Key As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal HashSelected As CryptoHash.Hashes)
        End Sub

        Public Function GetHashInBytes(ByVal data As Byte()) As Byte()

        End Function

        Public Function GetHashInBytes(ByVal data As String) As Byte()

        End Function

        Public Function GetHashInString(ByVal data As Byte()) As String

        End Function

        Public Function GetHashInString(ByVal data As String) As String

        End Function

        Public Enum Hashes
            CRC32 = 0
            HMACSHA1 = 1
            MACTripleDES = 2
            MD5 = 3
            SHA1 = 4
            SHA256 = 5
            SHA384 = 6
            SHA512 = 7
        End Enum

    End Class
End Namespace
