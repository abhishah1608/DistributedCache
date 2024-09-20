using DistributedCache.Enum;
using System.Text;

namespace DistributedCache.Helper
{
    public class QueryBuilderCls
    {

        public static string GenerateQuery(string key)
        {
            StringBuilder sb = new StringBuilder();
            if(key == Etable.APIErrorDetails.ToString())
            {
                sb.Append(@"Select * from APIErrorDetails WITH(NOLOCK)");
            }

            return sb.ToString();
        }
    }
}
