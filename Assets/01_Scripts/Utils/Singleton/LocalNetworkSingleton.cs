using Unity.Netcode;

namespace Utils
{
    public class LocalNetworkSingleton<T> : BaseNetworkSingleton<T> where T : NetworkBehaviour
    {
        public static T Instance
        {
            get
            {
                return BaseInstance;
            }
        }
    }
}