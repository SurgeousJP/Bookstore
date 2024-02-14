namespace BookCatalog.API.Extensions
{
    public class StringExtension
    {
        public static string ToTsQuerySearchString(string searchWord)
        {
            var searchTags = searchWord.Split('+');

            for (int i = 0; i < searchTags.Length; i++)
            {
                searchTags[i] = '\'' + searchTags[i] + '\'';
            }

            return String.Join(" & ", searchTags);
        }
    }
}
