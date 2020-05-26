using System;
namespace mr
{
    public class Settings
    {
        string token = "Token Here";
        string prefix = "!";

        public string getToken() {
            return token;
        }

        public string getPrefix() {
            return prefix;
        }
    }
}
