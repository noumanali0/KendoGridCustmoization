using System.Web;

namespace RemoteBindingGrid.HTMLHelpers
{
    public class HelperResult : IHtmlString
    {

        public string ToHtmlString()
        {
            return ToString();
        }
    }
}