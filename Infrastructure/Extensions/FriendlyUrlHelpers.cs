using System.Text;

namespace QAP4.Extensions
{
    public class FriendlyUrlHelpers
    {
        public static string GetFriendlyTitle(string title, bool remapToAscii = true, int maxlength = 255)
		{
			if (title == null)
			{
				return string.Empty;
			}

			var slug = StringExtensions.StringToSlug(title, remapToAscii, maxlength);
			return slug;
		}

		
    }
}