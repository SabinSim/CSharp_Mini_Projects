namespace PracticeApi.Models
{
    // ════════════════════════════════════════════════════════════════
    // 👤 User 클래스: 사용자 정보를 담는 모델
    // ════════════════════════════════════════════════════════════════
    public class User
    {
        public int    Id    { get; set; }
        public string Name  { get; set; } = "";
        public string Email { get; set; } = "";
        public int    Age   { get; set; }
        public string Role  { get; set; } = "user";  // "user" 또는 "admin"
    }
}