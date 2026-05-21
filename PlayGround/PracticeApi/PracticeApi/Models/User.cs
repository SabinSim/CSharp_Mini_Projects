namespace PracticeApi.Models
{
    // 유저 데이터 모델 (DB 테이블 한 행 = C# 객체 하나)
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";       // 이름 (필수)

        public string Email { get; set; } = "";      // 이메일 (필수)

        public int Age { get; set; }                 // 나이 (1~120)

        public string Role { get; set; } = "user";  // 역할: "user" 또는 "admin"
    }
}