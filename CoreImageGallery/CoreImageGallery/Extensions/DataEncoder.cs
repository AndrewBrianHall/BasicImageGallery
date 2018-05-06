namespace CoreImageGallery
{
    public static class Base64Encoder
    {
        public static string GetBase64String(string input)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(input);
            var base64 = System.Convert.ToBase64String(bytes);
            return base64;
        }

        public static string DecodeBase64String(string input)
        {
            var bytes = System.Convert.FromBase64String(input);
            var output = System.Text.Encoding.ASCII.GetString(bytes);
            return output;
        }
    }
}
