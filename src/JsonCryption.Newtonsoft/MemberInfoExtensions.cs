using System.Reflection;

namespace JsonCryption.Newtonsoft
{
    internal static class MemberInfoExtensions
    {
        public static bool ShouldEncrypt(this MemberInfo info) => info.GetCustomAttribute<EncryptAttribute>(inherit: true) != null;
    }
}
