namespace App1
{
    public class SearchRequest
    {
        public Type type;
        public string valueName;

        private const string GROUP_URL = "https://api.guap.ru/rasp/custom/get-sem-rasp/group";
        private const string TEACHER_URL = "https://api.guap.ru/rasp/custom/get-sem-rasp/prep";

        public SearchRequest(Type type, string valueName)
        {
            this.type = type;
            this.valueName = valueName;
        }

        public enum Type
        {
            Group,
            Teacher
        }

        public string GetURL()
        {
            string valueID = type == Type.Group ? Settings.Model.groupIdByName[valueName] : Settings.Model.teacherIdByName[valueName];
            return (type == Type.Group ? GROUP_URL : TEACHER_URL) + valueID;
        }
        public static SearchRequest GetHome()
        {
            return new SearchRequest(Type.Group, "М251");
        }
    }
}
