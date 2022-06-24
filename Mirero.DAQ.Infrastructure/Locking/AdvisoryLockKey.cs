using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mirero.DAQ.Infrastructure.Locking;

public class AdvisoryLockKey : ILockKey
{
    private readonly long _key;
    private readonly KeyEncoding _keyEncoding;

    public AdvisoryLockKey(long key)
    {
        _key = key;
        _keyEncoding = KeyEncoding.Int64;
    }

    public AdvisoryLockKey(string keyName, bool allowHashing = false)
    {
        if (keyName == null) { throw new ArgumentNullException(nameof(keyName)); }

        if (TryEncodeAscii(keyName, out this._key))
        {
            this._keyEncoding = KeyEncoding.Ascii;
        }
        else if (allowHashing)
        {
            this._key = HashString(keyName);
            this._keyEncoding = KeyEncoding.Int64;
        }
        else
        {
            throw new FormatException(
                $"Name '{keyName}' could not be encoded as a {nameof(AdvisoryLockKey)}." +
                $" Please specify {nameof(allowHashing)} or use a 0-{MaxAsciiLength} character string using only ASCII characters."
                );
        }
    }

    internal bool HasSingleKey => this._keyEncoding == KeyEncoding.Int64;

    public object Key => this._key;

    internal (int key1, int key2) Keys => SplitKeys(this._key);

    public bool Equals(AdvisoryLockKey that) => this.ToTuple().Equals(that.ToTuple());

    public override bool Equals(object? obj) => obj is AdvisoryLockKey that && this.Equals(that);

    public override int GetHashCode() => this.ToTuple().GetHashCode();

    public static bool operator ==(AdvisoryLockKey a, AdvisoryLockKey b) => a.Equals(b);

    public static bool operator !=(AdvisoryLockKey a, AdvisoryLockKey b) => !(a == b);

    private (long, bool) ToTuple() => (this._key, this.HasSingleKey);

    public override string ToString() => this._keyEncoding switch
    {
        KeyEncoding.Int64 => ToHashString(this._key),
        KeyEncoding.Ascii => ToAsciiString(this._key),
        _ => throw new InvalidOperationException()
    };

    private static (int key1, int key2) SplitKeys(long key) => ((int)(key >> (8 * sizeof(int))), unchecked((int)(key & uint.MaxValue)));

    #region ---- Ascii ----
    // ASCII 인코딩은 동작 설명.
    // 각 ASCII 문자는 7비트로 9개 문자, 총 63비트를 허용. (long 범위(64bit)안에서 처리하기 위함.)
    // 길이가 다른 문자열을 '\0'으로 구분하기 위해 문자열이 0으로 끝난 후 나머지 비트를 1로 채운다.
    // 따라서 최종 64비트 값은 7 비트의 0 ~ 9개의 char 값(7 * length) 다음에 0,
    // (63 - (7 * length)) 개수의 1로 구성된다.

    private const int AsciiCharBits = 7;
    private const int MaxAsciiValue = (1 << AsciiCharBits) - 1;             // 아스키 최댓값 : 1111111
    internal const int MaxAsciiLength = (8 * sizeof(long)) / AsciiCharBits; // 아스키 코드 문자열 최대 길이 : 9

    private static bool TryEncodeAscii(string name, out long key)
    {
        if (name.Length > MaxAsciiLength)
        {
            key = default;
            return false;
        }

        var result = 0L;
        foreach (var @char in name)
        {
            // 문자가 아스키 코드 범위를 벗어날 경우.
            if (@char > MaxAsciiValue)
            {
                key = default;
                return false;
            }
            // 7 비트씩 문자를 입력.
            result = (result << AsciiCharBits) | @char;
        }

        result <<= 1; // 문자열의 끝을 입력하기 위한 0 입력.
        for (var i = name.Length; i < MaxAsciiLength; ++i)
        {
            // 7 비트씩 나머지 비트를 1로 채운다.
            result = (result << AsciiCharBits) | MaxAsciiValue;
        }

        key = result;
        return true;
    }

    private static string ToAsciiString(long key)
    {
        var remainingKeyBits = unchecked((ulong)key);

        // 1로 채워진 빈공간 제거.
        var length = MaxAsciiLength;
        while ((remainingKeyBits & MaxAsciiValue) == MaxAsciiValue)
        {
            --length;
            remainingKeyBits >>= AsciiCharBits;
        }

        // 문자열의 끝 0 확인하여 0이 아닌 경우 오류 발생.
        if ((remainingKeyBits & 1) != 0)
        {
            throw new InvalidOperationException("last padding bit should be zero.");
        }

        remainingKeyBits >>= 1; // 0 제거.

        // 7 비트씩 확인하여 char 배열에 입력.
        var chars = new char[length];
        for (var i = length - 1; i >= 0; --i)
        {
            chars[i] = (char)(remainingKeyBits & MaxAsciiValue);
            remainingKeyBits >>= AsciiCharBits;
        }

        return new string(chars, startIndex: 0, length);
    }
    #endregion

    #region ---- Hashing ----

    private static long HashString(string name)
    {
        // SHA1의 해시 결과가 너무 커서 잘라야 한다.
        // 권장되는 방법이며 더 적은 바이트를 사용하기 때문에 해시를 약화시키지 않는다.

        using var sha1 = SHA1.Create();
        var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(name));

        // long size만큼만 사용.
        var result = 0L;
        for (var i = sizeof(long) - 1; i >= 0; --i)
        {
            result = (result << 8) | hashBytes[i];
        }
        return result;
    }

    private static string ToHashString(long key) => key.ToString("x16", NumberFormatInfo.InvariantInfo);
    #endregion

    private enum KeyEncoding
    {
        Int64 = 0,
        Ascii,
    }
}